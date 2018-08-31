using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour {
	public List<Text> darkTexts;
	public List<Text> lightTexts;
	public List<Image> darkImages;
	public List<Image> lightImages;
	public List<Button> darkButtons;
	public List<DynamicShadow> shadows;
	public Image fade;

	//public static ThemeManager instance = null;

	private Color32[] oldTheme;
	private Color32[] myTheme;
	private string key; // dictionary key for color picker
	private bool inTransition = false;
	private float t;

	// Use this for initialization
	void Start() {
		SetTheme();
	}
	
	// Update is called once per frame
	void Update () {
		if(inTransition) {
			t += Time.deltaTime;
			Color lightCol = Color.Lerp(oldTheme[0], myTheme[0], t);
			Camera.main.backgroundColor = lightCol;
			foreach (Image i in lightImages) {
				i.color = lightCol;
			}
			foreach (Text t in lightTexts) {
				t.color = lightCol;
			}
			Color darkCol = Color.Lerp(oldTheme[2], myTheme[2], t);
			foreach (Text t in darkTexts) {
				t.color = darkCol;
			}
			foreach (Image i in darkImages) {
				i.color = darkCol;
			}
			foreach (Button b in darkButtons) {
				ColorBlock cb = b.colors;
				cb.normalColor = darkCol;
				b.colors = cb;
			}
		}
	}

	// Sets the color of all objects AT START/AWAKE
	void SetTheme() {
		key = PlayerPrefs.GetString("Theme", "WINTER");
		myTheme = ColorPicker.COL_DICT[key];
		Camera.main.backgroundColor = myTheme[0];
		//fade.color = myTheme[0];
		//Debug.Log("Current theme:" + myTheme[0]);
		
		foreach (Text t in darkTexts) {
			t.color = myTheme[2];
		}
		foreach (Image i in darkImages) {
			i.color = myTheme[2];
		}
		foreach (Image i in lightImages) {
			i.color = myTheme[0];
		}
		foreach (Text t in lightTexts) {
			t.color = myTheme[0];
		}
		foreach (Button b in darkButtons) {
			ColorBlock cb = b.colors;
			cb.normalColor = myTheme[2];
			b.colors = cb;
		}
	}

	// Changes the theme of the game to nTheme
	public void ChangeTheme(string nTheme) {
		oldTheme = ColorPicker.COL_DICT[PlayerPrefs.GetString("Theme", "WINTER")];
		myTheme = ColorPicker.COL_DICT[nTheme];
		PlayerPrefs.SetString("Theme", nTheme);
		StartCoroutine(TransitionTheme());
	}

	IEnumerator TransitionTheme() {
		inTransition = true;
		t = 0f;
		SetShadows();
		yield return new WaitForSeconds(1.1f);
		inTransition = false;
	}

	void SetShadows() {
		foreach(DynamicShadow d in shadows) {
			d.ChangeColor();
		}
	}
}
