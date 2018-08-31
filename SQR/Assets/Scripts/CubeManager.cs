using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SwipeDirection {
	None = 0,
	Left = 1,
	Right = 2,
	Up = 3,
	Down = 4,
	Click = 16,
}

public class CubeManager : MonoBehaviour {
	public GameObject[] arrows;

	// Components of Cube	
	private Animator anim;
	private BoxCollider2D coll;
	private int myPlace;
	
	// Variables used to detect swipe
	private SwipeDirection Direction{set;get;} 	// Swipe Direction
	private Vector3 touchPosition;     // Start position of swipe
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 50.0f;
	private bool inBox;		// Is the swipe within the cube?

	private float speedMult;  // speed multiplier of animation

	private bool arrowOn;   // Does the player want arrows?

	const float ANIMTIME = 1.0f / 60.0f * 22; // length of 22 frames at 60fps
	const float DEFAULT = 1.5f; // multiplier used when player swipes

	// Colors the arrows properly and sets them to inactive
	void SetArrows() {
		arrowOn = PlayerPrefsX.GetBool("Arrow", true);
		Color col = ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme", "WINTER")][2];
		foreach (GameObject g in arrows) {
			g.GetComponent<SpriteRenderer>().color = col;
			g.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		coll = GetComponentInChildren<BoxCollider2D>();
		inBox = false;
		speedMult = 0.5f;
		SetSpeed(speedMult);
		SetArrows();
	}
	
	// Update is called once per frame
	void Update () {
		// If it's not the player's turn don't do anything
		if (!GameManager.instance.playerTurn) {
			return;
		}
		// Check for player input
		Direction = SwipeDirection.None;
		DetectSwipe();
		
		// If Swiping occured
		if (Direction != SwipeDirection.None) {
			// Animation occurs in direction of player swipe
			AnimateSpin((int) Direction, DEFAULT);
			StartCoroutine(CheckMove((int)Direction));
		}
	}

	// Checks whether the move was right or wrong
	IEnumerator CheckMove(int dir) {
		yield return new WaitForSeconds(AnimationTime());
		// If the swipe was correct, add time
		if (GameManager.instance.IsCorrect(myPlace, dir)) {
			TimeManager.myTimer.addTime();
			// If all correct motions have been done, update score, reset time, and player turn is false
			if (GameManager.instance.IsTurnOver()) {
				GameManager.instance.EndPlayer();
			}
		}
		// If the swipe was incorrect, subtract time, shake cube
		else {
			WrongMove();
		}
	}

	// called on wrong move. Shakes cube and subtracts time
	void WrongMove() {
		TimeManager.myTimer.minusTime();
		iTween.ShakePosition(gameObject, iTween.Hash("x", .5, "time", 1.0));
	}

	// Checks for swipe 
	void DetectSwipe() {
		if (Input.GetMouseButtonDown (0)) {
			touchPosition = Input.mousePosition;
			//Debug.Log(myPlace + ": " + myBounds);
			//Debug.Log("Mouse Position: " + Camera.main.ScreenToWorldPoint(touchPosition));
			// is start position within box?
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPosition);
			worldPos.z = 0;
			if(coll.bounds.Contains(worldPos)) {
				inBox = true;
				//Debug.Log("Contains Point");
			}
			else
				inBox = false;
		}

		// update the box if swipe started in box
		else if (Input.GetMouseButtonUp(0) && inBox) {
			//Debug.Log("Swiping");
			// get difference between start and end position
			Vector3 deltaSwipe = touchPosition - Input.mousePosition;

			if (Mathf.Abs(deltaSwipe.x) > Mathf.Abs(deltaSwipe.y) && Mathf.Abs(deltaSwipe.x) > swipeResistanceX) {
				// Swipe on the X axis 
				Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
			}
			else if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY) {
				// Swipe on the Y axis 
				Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
			}
		}
	}

	// converts state into its corresponding string
	string Translate(int state) {
		switch (state) {
			case 1:
				return "Left";
			case 2:
				return "Right";
			case 3:
				return "Up";
			case 4:
				return "Down";
			default:
				return "Left";
		}
	}

	// called by GameManager to set cube to placeth in its List
	public void setPlace(int place) {
		myPlace = place;
	}

	// called by gameManager to show the move to player
	public void DisplaySpin(int state, float s) {
		AnimateSpin(state, s);
		if (arrowOn)
			StartCoroutine(ShowArrow(state));
	}

	// Set animator to correct animation with speed s
	void AnimateSpin(int state, float s) {
		SetSpeed(s);
		SoundManager.instance.Play("swipe" + state);
		anim.SetTrigger(Translate(state));
	}

	// set the state-th arrow to active and deactive after animation is over
	IEnumerator ShowArrow(int state) {
		arrows[state-1].SetActive(true);
		yield return new WaitForSeconds(AnimationTime());
		arrows[state-1].SetActive(false);
	}

	// length of the animation given the speedMultiplier
	public float AnimationTime() {
		return ANIMTIME / speedMult;
	}

	// Set animation speed of the cubes to s
	void SetSpeed(float s) {
		speedMult = s;
		anim.SetFloat("SpeedMult", speedMult);
	}

	// Makes cubes disappear/reappear
	public void CubeAway() {
		bool shrunk = anim.GetBool("Shrink");
		if (!shrunk)
			iTween.ShakePosition(gameObject, iTween.Hash("x", .8, "time", 1.0));
		anim.SetBool("Shrink", !shrunk);
	}

	// Sets the position of the cube to pos
	public void ChangeLocation(Vector3 pos) {
		transform.position = pos;
	}
}
