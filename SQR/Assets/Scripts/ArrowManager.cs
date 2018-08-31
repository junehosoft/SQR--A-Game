using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ArrowManager : MonoBehaviour {
	private Toggle myToggle;

	// Use this for initialization
	void Start () {
		myToggle = GetComponent<Toggle>();
		bool current = PlayerPrefsX.GetBool("Arrow", true);
		if (current) {
			myToggle.isOn = true;
		}
		else {
			myToggle.isOn = false;
		}
	}
	
	// Changed arrow settings
	public void ChangeToggle() {
		if (myToggle.isOn)
			PlayerPrefsX.SetBool("Arrow", true);
		else
			PlayerPrefsX.SetBool("Arrow", false);

		SoundManager.instance.Play("button1");
	}
}
