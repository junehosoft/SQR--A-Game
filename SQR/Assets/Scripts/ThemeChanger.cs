using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThemeChanger : MonoBehaviour {
	public List<Toggle> myToggles;

	ToggleGroup toggleGroup;
	string myTheme = "";
	bool isChanged = false;

	public ThemeManager tm;

	// Use this for initialization
	void Awake() {
		toggleGroup = GetComponent<ToggleGroup>();
		myTheme = PlayerPrefs.GetString("Theme", "WINTER");
		foreach (Toggle t in myToggles) {
			if (myTheme.Equals(t.name)) {
				t.isOn = true;
				Debug.Log("First Selected is " + t.name);
				break;
			}
		}
	}
	
	void Update() {
		if (isChanged) {
			Debug.Log("Change Theme");
			tm.ChangeTheme(myTheme);
			isChanged = false;
		}
	}

	public void SelectTheme() {
		Toggle active = toggleGroup.ActiveToggles().FirstOrDefault();
		if (active == null)
			return; 
		Debug.Log("Active toggle is: " + active.name);
		if (!active.name.Equals(myTheme)) {
			myTheme = active.name;
			//PlayerPrefs.SetString("Theme", myTheme);
			Debug.Log(active.name);
			isChanged = true;
			Debug.Log("Changed is " + isChanged);
		}
	}
}
