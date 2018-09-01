using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour {
	public Button Right;
	public Button Left;
	public int pages;
	public Scrollbar myBar;
	public GameObject content;
	public GameObject pageView;
	public GameObject dot;

	[HideInInspector]
	public int index = 0;		// page number of tutorial

	private float[] positions;		// normalized position of pages from 0 to 1;
	private Color myCol;		// dark color of theme manager
	private GameObject[] myDots;	// storage for the dots

	// Calculates the x positions of the pages
	void SetPositions() {
		positions = new float[pages];
		float width = 1.0f/(pages-1);
		float val = 0;
		for (int i = 0; i < pages; i++) {
			positions[i] = val;
			val += width;
		} 
		myBar.value = 0;
	}

	// Use this for initialization
	void Awake () {
		SetPositions();
		Left.onClick.AddListener(ScrollLeft);
		Right.onClick.AddListener(ScrollRight);
		Left.interactable = false;
	}
	
	// instantiates the dots that tell what page the viewer is on
	void CreateDots() {
		myDots = new GameObject[pages];
		myCol =  ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme", "WINTER")][2];
		for (int i = 0; i < pages; i++) {
			GameObject newDot = Instantiate(dot, pageView.transform);
			if (i != 0)
				newDot.GetComponent<Image>().color = myCol;
			myDots[i] = newDot;
		}
	}

	void Start() {
		Tutorial.instance.ChangePage();
		CreateDots();
	}

	// Update is called once per frame
	void Update () {
	}

	// Moves the page to the right
	void ScrollRight() {
		myDots[index].GetComponent<Image>().color = myCol;
		index++;
		myDots[index].GetComponent<Image>().color = Color.white;
		myBar.value = positions[index];
		Left.interactable = true;
		if (index == pages-1)
			Right.interactable = false;
	}

	// Moves the page to the left
	void ScrollLeft() {
		myDots[index].GetComponent<Image>().color = myCol;
		index--;
		myDots[index].GetComponent<Image>().color = Color.white;
		myBar.value = positions[index];
		Right.interactable = true;
		if (index == 0)
			Left.interactable = false;
	}
}
