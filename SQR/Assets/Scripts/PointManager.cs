using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour {
	public Text highText;
	public Text scoreText;
	public GameObject pointIndicator;

	private int highScore;  // highscore of the player
	private int score = 0;   // Score of the player

    // Prepares score text on startup
	public void SetUpScore() {
		scoreText.text = "" + score;
		highScore = PlayerPrefsX.GetIntArray("HighScores", 0, 4)[PlayerPrefs.GetInt("Difficulty", 1) - 1];
		highText.text = highScore.ToString();
	}

	// Updates text display for score
	void UpdateScore() {
		scoreText.text = score.ToString();
		if (score > highScore) {
			highText.text = score.ToString();
		}
	}

    // Called by gamemanager whenever the player wins a round
    // Displays a +val about the cubes
	IEnumerator ShowAdd(int val) {
		GameObject newPoint = Instantiate(pointIndicator);
		Text p = newPoint.GetComponentInChildren<Text>();
		p.text = "+" + val;
		yield return new WaitForSeconds(0.8f);
		Destroy(newPoint);
	}

    // Prepares the score for new game
	public void ResetScore() {
		score = 0;
		UpdateScore();
	}

	// Increments score by 1
	public void AddScore(int val) {
		score += val;
		StartCoroutine(ShowAdd(val));
		UpdateScore();
	}

	// saves highscore to playerprefs
	public void UpdateHighScore() {
		if (score > highScore) {
			highScore = score;
			int[] highScores = PlayerPrefsX.GetIntArray("HighScores", 0, 4);
			highScores[PlayerPrefs.GetInt("Difficulty", 1)-1] = highScore;
			PlayerPrefsX.SetIntArray("HighScores", highScores);
		}
	}
}
