using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public Text highText;

	public static ScoreManager instance = null;

	private int diff;	// difficulty of the game
	private int display;	// displayed score
	private int score;		// actual score
	private int[] bestScores;	// array of all high scores
	private bool inTransition;		// is the score in transition?

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		highText.text = "0";
		diff = PlayerPrefs.GetInt("Difficulty", 1);
		bestScores = PlayerPrefsX.GetIntArray("HighScores", 0, 4);
		score = bestScores[diff-1];
		display = 0;
		inTransition = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (inTransition) {
			if (display < score) 
				display++;
			else if (display > score)
				display--;
			else
				inTransition = false;
			highText.text = display.ToString();
		}
	}

	// Prepares the Update function for changing the highscore
	public void ChangeBest() {
		diff = PlayerPrefs.GetInt("Difficulty", 1);
		score = bestScores[diff-1];
		inTransition = true;
	}
}
