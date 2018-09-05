using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicShadow : MonoBehaviour {
	public float offset;

	private GameObject shadow;
	private Image myImage = null;
	private Image thisImage = null;
	private Text myText = null;
	private Text thisText;
	private SpriteRenderer mySprite = null;
	private SpriteRenderer thisSprite = null;

	private bool isImage = false;
	private bool isText = false;
	private bool isSprite = false;
	private bool inTransition = false;

	private float t;
	private Color oldCol;
	private Color32 myCol;

	// Use this for initialization
	void Start () {
		shadow = new GameObject("Shadow");
		shadow.tag = "Shadow";
		myCol = (ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme", "WINTER")])[1];
		if (this.GetComponent<Image>() != null) {
			SetImage();
		}
		if (this.GetComponent<Text>() != null) {
			SetText();
			//myText.resizeTextForBestFit = true;
		}
		if (this.GetComponent<SpriteRenderer>() != null) {
			SetSprite();
		}
		SetTransform();
		//myCol = new Color32(100, 100, 100, 255);
	}
	
	// Update is called once per frame
	void Update () {
		if (inTransition) {
			t += Time.deltaTime;
			Color col = Color.Lerp(oldCol, myCol, t);
			if (isText) {
				myText.color = col;
			} 
			if (isImage) {
				myImage.color = col;
			}
			if (isSprite) {
				mySprite.color = col;
			}
		}
		CheckChanges();
	}

	// If the text, image, or sprites are changed, this is called so the shadow matches
	void CheckChanges() {
		if (isText && thisText.text != myText.text) {
			myText.text = thisText.text;
		} 
		if (isImage) {
			if (thisImage.sprite != myImage.sprite) 
				myImage.sprite = thisImage.sprite;
			if (thisImage.transform.localScale != myImage.transform.localScale)
				myImage.transform.localScale = thisImage.transform.localScale;
		}
		if (isSprite) {
			if (thisSprite.sprite != mySprite.sprite) 
				mySprite.sprite = thisSprite.sprite;
			if (thisSprite.transform.localScale != mySprite.transform.localScale)
				mySprite.transform.localScale = thisSprite.transform.localScale;
		}
	}

    // Prepares the shadow for image component
	void SetImage() {
		isImage = true;
		myImage = shadow.AddComponent<Image>();
		thisImage = this.GetComponent<Image>();
		myImage.sprite = this.GetComponent<Image>().sprite;
		myImage.color = myCol;

	}

    // Prepares the shadow for Sprite component
	void SetSprite() {
		isSprite = true;
		mySprite = shadow.AddComponent<SpriteRenderer>();
		mySprite.sortingLayerName = "Shadow";
		thisSprite = this.GetComponent<SpriteRenderer>();
		mySprite.sprite = this.GetComponent<SpriteRenderer>().sprite;
		mySprite.color = myCol;
	}

    // Prepares the shadow for Text component
	void SetText() {
		isText = true;
		myText = shadow.AddComponent<Text>();
		thisText = this.GetComponent<Text>();
		myText.alignment = thisText.alignment;
		myText.font = thisText.font;
		myText.fontSize = thisText.fontSize;
		myText.text = thisText.text;
		myText.color = myCol;
	}

    // Sets location of the shadow relative to the original object, using
    // offset as the basis
	void SetTransform() {
		shadow.transform.SetParent(this.transform.parent);
		Vector3 newPos = this.transform.localPosition;
		shadow.transform.localScale = this.transform.localScale;
		if (this.GetComponent<RectTransform>() != null) {
			RectTransform rt = this.GetComponent<RectTransform>();
			shadow.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;
		}
		newPos.x += offset;
		newPos.y -= offset;
		shadow.transform.localPosition = newPos;
		shadow.transform.SetSiblingIndex(0);
		this.transform.SetSiblingIndex(1);
	}

	// Changes color of shadows
	public void ChangeColor() {
		oldCol = myCol;
		myCol = (ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme")])[1];
		StartCoroutine(TransitionColor());
	}

	// Prepares update to change shadow color
	IEnumerator TransitionColor() {
		inTransition = true;
		t = 0f;
		yield return new WaitForSeconds(1.1f);
		inTransition = false;
	}
}
