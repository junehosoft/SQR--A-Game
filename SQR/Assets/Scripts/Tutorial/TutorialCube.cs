using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TutorialCube : MonoBehaviour {
	// Components of Cube	
	private Animator anim;
	private BoxCollider2D coll;
	
	// Variables used to detect swipe
	private SwipeDirection Direction{set;get;} 	// Swipe Direction
	private Vector3 touchPosition;     // Start position of swipe
	private float swipeResistanceX = 50.0f;
	private float swipeResistanceY = 50.0f;
	private bool inBox;		// Is the swipe within the cube?

	private float speedMult;  // speed multiplier of animation

	const float ANIMTIME = 1.0f / 60.0f * 22; // length of 22 frames at 60fps
	const float DEFAULT = 1.5f; // multiplier used when player swipes

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		coll = GetComponentInChildren<BoxCollider2D>();
		inBox = false;
		speedMult = 0.5f;
		SetSpeed(speedMult);
	}
	
	// Update is called once per frame
	void Update () {
		// If it's not the player's turn don't do anything
		if (!Tutorial.instance.playerTurn) {
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
		if (dir == 1) {
			Tutorial.instance.Correct();
		}
		// If the swipe was incorrect, subtract time, shake cube
		else {
			WrongMove();
		}
	}

	// called on wrong move. Shakes cube and subtracts time
	void WrongMove() {
		Tutorial.instance.Wrong();
		iTween.ShakePosition(gameObject, iTween.Hash("x", .5, "time", 1.0));
	}

	// Checks for swipe 
	void DetectSwipe() {
		if (Input.GetMouseButtonDown (0)) {
			touchPosition = Input.mousePosition;
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

	// Set animator to correct animation with speed s
	public void AnimateSpin(int state, float s) {
		SetSpeed(s);
		SoundManager.instance.Play("swipe" + state);
		anim.SetTrigger(Translate(state));
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
		anim.SetBool("Shrink", !shrunk);
	}
}
