using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {
	public Button adBtn;
	public Text adText;
	public Image adIcon;
	public Sprite VideoOn;
	public Sprite VideoOff;

	public void ShowRewardedAd() {
		if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
		else {
			Blink();
		}
	}

	void HandleShowResult(ShowResult result) {
		switch(result) {
			case ShowResult.Finished:
				Debug.Log("Advertisement Watched");
				adBtn.interactable = false;
				adIcon.sprite = VideoOff;
				var col = adIcon.color;
				col.a = 0.5f;
				adIcon.color = col;
				GameManager.instance.StartHere();
				break;
			case ShowResult.Skipped:
				adText.text = "PLEASE WATCH THE WHOLE AD FOR THE BONUS";
				Debug.Log("Advertisement Skipped");
				break;
			case ShowResult.Failed:
				adText.text = "AD FAILED TO LOAD";
				Blink();
				Debug.Log("Advertisement Failed");
				break;

		}
	}

	public void ResetAd() {
		adBtn.interactable = true;
		adIcon.sprite = VideoOn;
		var col = adIcon.color;
		col.a = 1.0f;
		adIcon.color = col;
		adText.text = "WATCH AN AD TO START WHERE YOU LEFT OFF";
	}

	IEnumerator Blink() {
		Color col = adIcon.color;
		for (int i = 0; i < 4; i++) {
			if (i % 2 == 0) {
				adIcon.sprite = VideoOff;
				adIcon.color = ColorPicker.RED;
			}
			else {
				adIcon.sprite = VideoOn;
				adIcon.color = col;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
