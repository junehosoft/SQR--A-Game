using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DifficultyManager : MonoBehaviour {
	public List<Toggle> myToggles;

	ToggleGroup toggleGroup;

	// Use this for initialization
	void Start () {
		toggleGroup = GetComponent<ToggleGroup>();
		int current = PlayerPrefs.GetInt("Difficulty", 1);
		foreach (Toggle t in myToggles) {
			Debug.Log(current);
			if (current == int.Parse(t.name)) {
				t.isOn = true;
				t.interactable = false;
				Debug.Log("Difficulty is " + t.name);
				break;
			}
		}
	}

	// Sets PlayerPrefs to correct difficulty
	void SetDifficulty() {
		Toggle active = toggleGroup.ActiveToggles().FirstOrDefault();
		int diff = int.Parse(active.name);
		PlayerPrefs.SetInt("Difficulty", diff);
	}

	// Makes the clicked toggle uninteractable and sets new difficulty
	public void ChangeToggle() {
		foreach (Toggle t in myToggles) {
			if (t.isOn) 
				t.interactable = false;
			else
				t.interactable = true;
		}
		SetDifficulty();
		ScoreManager.instance.ChangeBest();
		SoundManager.instance.Play("button1");
	}
}
