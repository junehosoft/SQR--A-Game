using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {
	//public AudioSource fxSource;
	//public AudioSource musicSource;
	public static SoundManager instance = null;
	
	public Sound[] sounds;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		//DontDestroyOnLoad(gameObject);
		if (!PlayerPrefs.HasKey("SoundOn"))
			PlayerPrefsX.SetBool("SoundOn", true);
		
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
	}
	
	// Plays the given sound 
	public void Play (string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s != null)
			s.source.Play();
	}

	// Plays a sound name at a pitch between min and max
	public void RandomPitch(string name, float min, float max) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s != null) {
			float temp = s.source.pitch;
			s.source.pitch = Random.Range(min, max);
			s.source.Play();
			s.source.pitch = temp;
		}
	}
	/*public void PlaySingle(AudioClip clip) {
		fxSource.clip = clip;
		fxSource.Play();
	} */

	/*public void RandomizeSfx (params AudioClip[] clips) {
		int randomIndex = Random.Range(0, clips.Length);

		fxSource.clip = clips[randomIndex];

		fxSource.Play();
	} */

	// turns sound on and off
	public void ToggleSound() {
		Debug.Log("Sound toggled");
		bool s = PlayerPrefsX.GetBool("SoundOn");
		AudioListener.pause = s;
		PlayerPrefsX.SetBool("SoundOn", !s);
	}
}
