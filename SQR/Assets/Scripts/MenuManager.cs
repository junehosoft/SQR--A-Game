using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public GameObject settings;
	public Button settingsBtn;
	public GameObject credits;
	public GameObject difficulty;

	private bool creditActive = false;

	// Use this for initialization
	void Awake () {
		HideSettings();
		HideCredits();
		//highText.text = PlayerPrefs.GetInt("HighScore").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (settings.activeSelf)
				HideSettings();
			else if(creditActive)
				HideCredits();
			else{
				Application.Quit();
			}
		}
	}

	public void ShowDifficulty() {
		difficulty.GetComponent<Animator>().SetTrigger("Show");
	}

	public void HideDifficulty() {
		difficulty.GetComponent<Animator>().SetTrigger("Hide");
	}

	public void ShowSettings() {
		settingsBtn.interactable = false;
		settings.SetActive(true);
		HideDifficulty();
	}

	public void HideSettings() {
		settingsBtn.interactable = true;
		settings.SetActive(false);
	}

	public void ShowCredits() {
		credits.transform.position = new Vector3(0, 0, 0);
		creditActive = true;
		HideDifficulty();
	}

	public void HideCredits() {
		credits.transform.position = new Vector3(480, 0, 0);
		creditActive = false;
	}
}
