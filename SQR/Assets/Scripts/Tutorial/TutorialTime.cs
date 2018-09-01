using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTime: MonoBehaviour {
	public Image timer;		// Visual representation of the timer

	private float maxTime = 20.0f;
	private float activeTime;  // real time left on timer
	private float displayTime; // actual time displayed

	private bool started;   // has the timer started?
	private bool redBar = false;   // what is the color of the bar
	private bool isBlinking = false;  // Is the timer blinking?
	//private Color myColor;

	private Color myCol;
	// Use this for initialization
	void Awake () {
		started = false;
		activeTime = 0;
		displayTime = activeTime;
		myCol = ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme", "WINTER")][2];
		timer.color = myCol;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDisplay();
		SetTimer();

		// if the player is not active don't do anything
		if (!Tutorial.instance.playerTurn) {
			started = false;
			return;
		}

		if (!started) {
			started = true;
			timer.color = Color.white;
			activeTime = maxTime;
		}

		activeTime -= Time.deltaTime;
		
		if (!isBlinking && activeTime < maxTime * 0.25f) {
			StartCoroutine(Blink());
		}

		if (activeTime <= 0) {
			activeTime = 0;
		}
	}

	// allow displayTime to catch up to activeTime
	void UpdateDisplay() {
		if (displayTime < activeTime) {
			displayTime += maxTime / 45f;
			if (displayTime > activeTime) 
				displayTime = activeTime;
		}
		else if (displayTime > activeTime) {
			displayTime -= Time.deltaTime * 5;
			if (displayTime < activeTime) 
				displayTime = activeTime;
		} 
		
	}

	// change the image to match timer
	void SetTimer() {
		timer.fillAmount = displayTime/maxTime;
	}

	// add time to timer
	public void addTime() {
		activeTime += maxTime * 0.1f;
		Debug.Log("Time Added");
		if (activeTime >= maxTime) 
			activeTime = maxTime;
	}

	// reduce time available
	public void minusTime() {
		//iTween.ShakePosition(gameObject, iTween.Hash("x", .1, "time", 0.8));
		StartCoroutine(Wrong());
		activeTime -= maxTime * 0.1f;
		if (activeTime <= 0)
			activeTime = 0.0f;
	}

	// reset the timer to max time
	public void resetTime() {
		timer.color = myCol;
		// float deltaTime = maxTime - activeTime;
		activeTime = maxTime;
	}

	// let the timer switch between red and white
	IEnumerator Blink() {
		// Debug.Log("blinking");
		isBlinking = true;
		if (redBar) {
			timer.color = Color.white;
			redBar = false;
		}
		else {
			timer.color = ColorPicker.RED;
			redBar = true;
		}
		yield return new WaitForSeconds(0.2f);
		isBlinking = false;
	}

	IEnumerator Wrong() {
		timer.color = ColorPicker.RED;
		yield return new WaitForSeconds(0.5f);
		timer.color = Color.white;
	}
}
