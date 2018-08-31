using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour {
	public Animator anim;

	private string levelToLoad;

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (SceneManager.GetActiveScene().buildIndex != 0)
				FadeToLevel("Menu");
		}
	}
	
	public void FadeToLevel(string sceneName) {
		levelToLoad = sceneName;
		anim.SetTrigger("FadeOut");
	}

	public void OnFadeComplete() {
		SceneManager.LoadScene(levelToLoad);
	}

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}
