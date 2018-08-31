using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	
	public int turns;
	public Text turnText;
	public float displayDelay = 1f;
	public GameObject goPanel;
	public Button rewindBtn;
	public Text rewindText;
	public GameObject cover;

	// Is it the player's turn?
	[HideInInspector] public bool playerTurn;		
	
	private BoardManager myBoard;
	private PointManager myScore;
	private int rewinds;  // Number of rewinds left
	private int difficulty;   // number of cubes for the game

	// Sequence of directions player mut follow
	private List<int> turnValues;   // List that holds the correct inputs
	private int turnIndex = 0;	// index of turnValues
	private int myLength = 0;	// length of turnValues
	private float increment = 0.05f;

	[HideInInspector]
	public List<CubeManager> myCubes;	// Holds all cubemanager instances

	private bool displayingMoves;   // 
	private bool gameOver;

	private float speedMod = 0.5f;

	private Color textColor;

	const int COUNT = 4;	// NUmber of directions is always 4
	
	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		
		myBoard = GetComponent<BoardManager>();
		myScore = GetComponent<PointManager>();
		myCubes = new List<CubeManager>();
		InitGui();
		InitGame();
	}

	// Called after Awake
	void Start() {
		textColor = turnText.color;
	}

	// Initializes all gui elements on the game
	void InitGui() {
		myScore.SetUpScore();
		turnText.text = "" + turns;
		turnValues = new List<int>();
		rewinds = 3;
		UpdateRewind();
		cover.SetActive(false);
		goPanel.SetActive(false);
	}

	// BoardManager places the cubes, and booleans are set to starting state
	void InitGame() {
		difficulty = PlayerPrefs.GetInt("Difficulty", 1);
		myBoard.SetupScene(difficulty);
		playerTurn = false;
		displayingMoves = false;
		gameOver = false;
	}

	// Update is called once per frame
	void Update () {
		if (playerTurn || displayingMoves)
			return;
		if (gameOver) {
			return;
		}
		//RandomList(myLength);
		AddToList();
		StartCoroutine(DisplayMoves());
	}
	
	// Call this to add the passed cube to list of cube objects
	public void AddCubeToList(CubeManager script) {
		myCubes.Add(script);
		script.setPlace(myCubes.Count);
	}

	// Updates text display for turns
	void UpdateTurns() {
		turnText.text = turns.ToString();
	}

	// Show the moves the player has to copy
	IEnumerator DisplayMoves() {
		displayingMoves = true;

		turnText.color = textColor;
		turns = 0;
		UpdateTurns();
		yield return StartCoroutine(TimeManager.myTimer.resetTime());
		yield return new WaitForSeconds(1.0f);
		// yield return new WaitForSeconds(displayDelay);
		for (int i = 0; i < myLength; i++) {
			int cubeNum = turnValues[i] / COUNT;
			int dir = (turnValues[i] % COUNT) + 1;
			myCubes[cubeNum].DisplaySpin(dir, speedMod);
			turns++;
			UpdateTurns();
			yield return new WaitForSeconds(myCubes[cubeNum].AnimationTime() * 1.3f);
		}

		Debug.Log("Done displaying");
		
		if (isRandom()) {
			StartCoroutine(Randomize());
		}
		else {
			StartPlayer();
		}
	}

	// Will the cubes randomize position this round?
	private bool isRandom() {
		float chance;
		float p = Random.Range(0.0f, 1.0f);
		if (myLength == 1 || difficulty == 1) {
			return false;
		}
		else if (difficulty == 2) {
			chance = 0.05f * myLength;
		}
		else if (difficulty == 3) {
			chance = 0.04f * myLength;
		}
		else {
			chance = 0.03f * myLength;
		}
		if (p < chance)
			return true;
		return false;
	}

	// covers the board and randomize the cube position
	IEnumerator Randomize() {
		cover.SetActive(true);
		myBoard.RandomPositions();
		yield return new WaitForSeconds(0.5f);
		cover.SetActive(false);
		StartPlayer();
	}

	//enables player turn
	void StartPlayer() {
		SoundManager.instance.Play("correct");
		playerTurn = true;
		displayingMoves = false;
		turnText.color = Color.white;
	}

	// Ends the player turn after all correct inputs
	public void EndPlayer() {
		myScore.AddScore(myLength);
		TimeManager.myTimer.resetTime();
		speedMod += increment;
		if (myLength >= 15) 
			increment = 0.01f;
		else if (myLength >= 10)
			increment = 0.02f;
		else if (myLength >= 5)
			increment = 0.03f;
		playerTurn = false;
	}

	// Briefly turns the turnText red
	IEnumerator TurnRed() {
		turnText.color = ColorPicker.RED;
		yield return new WaitForSeconds(0.5f);
		turnText.color = Color.white;
	}

	// Check if the player input matches answer based on cube and dir
	public bool IsCorrect(int cube, int dir) {
		int val = turnValues[turnIndex];
		if (cube-1 == val / COUNT && dir-1 == val % COUNT) {
			turnIndex++;
			turns--;
			UpdateTurns();
			return true;
		}
		else {
			StartCoroutine(TurnRed());
			return false;
		}
	}

	// Check if sequence is complete
	public bool IsTurnOver() {
		if (turnIndex >= myLength) {
			turnIndex = 0;
			return true;
		}
		return false;
	}

	void AddToList() {
		myLength++;
		turnValues.Add(Random.Range(0, difficulty * 4));
	}

	// Updates button and text display
	void UpdateRewind() {
		if (rewinds <= 0) {
			rewindBtn.interactable = false;
		}
		else
			rewindBtn.interactable = true;
		rewindText.text = "REWIND(" + rewinds + ")";
	}

	// Replays the set of moves
	public void Rewind() {
		if (!playerTurn)
			return;
		SoundManager.instance.RandomPitch("rewind", 0.4f, 0.8f);
		rewinds--;
		UpdateRewind();
		playerTurn = false;
		turnIndex = 0;
		StartCoroutine(DisplayMoves());
	}

	// Game Over, prepares reset
	public void GameOver() {
		gameOver = true;
		playerTurn = false;
		myScore.UpdateHighScore();
		foreach (CubeManager c in myCubes) {
			c.CubeAway();
		}
		goPanel.SetActive(true);
	}

	// Resets the board
	public void Reset() {
		gameOver = false;
		goPanel.SetActive(false);
		foreach (CubeManager c in myCubes) {
			c.CubeAway();
		}
		myScore.ResetScore();
		turnText.color = textColor;
		myLength = 0;
		turnValues.Clear();
		turnIndex = 0;
		rewinds = 3;
		UpdateRewind();
	}

	// Starts the game where the player got a game over
	public void StartHere() {
		gameOver = false;
		goPanel.SetActive(false);
		foreach (CubeManager c in myCubes) {
			c.CubeAway();
		}
		turnText.color = textColor;
		turnIndex = 0;
		rewinds += 1;
		UpdateRewind();
		StartCoroutine(DisplayMoves());
	}
}
