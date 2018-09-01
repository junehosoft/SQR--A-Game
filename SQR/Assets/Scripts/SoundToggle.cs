using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour {
	public Sprite AudioOn;		// sprite for when audio is one
	public Sprite AudioOff;		// sprite for when audio is off

	private Image myBtn;

	// Use this for initialization
	void Start () {
		myBtn = this.GetComponentInChildren<Image>();
		ToggleSprite();
	}
	
	// Switches the audio sprite between on and off
	public void ToggleSprite() {
		if (PlayerPrefsX.GetBool("SoundOn")) {
			Debug.Log("sprite on");
			myBtn.sprite = AudioOn;
		}
		else {
			Debug.Log("sprite off");
			myBtn.sprite = AudioOff;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
