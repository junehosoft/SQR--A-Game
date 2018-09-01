using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
	public static Tutorial instance = null;

	public TutorialCube myCube;
	public GameObject scrollView;
	public ScrollManager myScroll;
	public GameObject turns;
	public Button rewindBtn;
	public Text rewindText;
	public GameObject cover;

	public bool playerTurn = false;

	private TutorialTime myTimer;		// Timer object
	private Text turnNum;			// Displays number of turns
	private int oldIndex = 0;			// THe index of the previous page
	private Color textCol;			// Dark color of themeManager

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	void Start() {
		scrollView.SetActive(false);
		cover.SetActive(false);
		turnNum = turns.GetComponentInChildren<Text>();
		myTimer = GetComponent<TutorialTime>();
		textCol = ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme", "WINTER")][2];
		StartCoroutine(WaitForSetup());
	}

	IEnumerator WaitForSetup() {
		myTimer.resetTime();
		yield return new WaitForSeconds(1.2f);
		scrollView.SetActive(true);
		cover.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void ChangePage() {
		StopAllCoroutines();
		int index = myScroll.index;
		//Debug.Log("Changed");
		switch (index) {
			case 0:
				playerTurn = false;
				turnNum.color = textCol;
				StartCoroutine(ShowMove());
				break;
			case 1:
				playerTurn = true;
				turns.SetActive(true);
				turnNum.color = Color.white;
				turnNum.text = "1";
				break;
			case 2:
				playerTurn = false;
				turns.SetActive(false);
				if (oldIndex == 3) 
					myCube.CubeAway();
				break;
			case 3:
				myCube.CubeAway();
				break;
		}
		oldIndex = index;
	}

	IEnumerator ShowMove() {
		myTimer.resetTime();
		yield return new WaitForSeconds(2.0f);
		while (true) {
			myCube.AnimateSpin(1, 1.0f);
			turnNum.text = "1";
			yield return new WaitForSeconds(2.0f);
			turnNum.text = "0";
			yield return new WaitForSeconds(1.0f);
		}
	}

	public void Correct() {
		myTimer.addTime();
		turnNum.text = "0";
	}

	public void Wrong() {
		myTimer.minusTime();
		StartCoroutine(TurnRed());
	}

	IEnumerator TurnRed() {
		turnNum.color = ColorPicker.RED;
		yield return new WaitForSeconds(0.5f);
		turnNum.color = Color.white;
	}
}
