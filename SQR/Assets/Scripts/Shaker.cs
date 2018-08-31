using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {
	private bool shaking = false;

	private float speed = 1.0f;
	private float shakeAmt = 1.0f;
	float x0;

	void Update () {
		if (shaking) {
			//Vector2 newPos = Random.insideUnitCircle * (Time.deltaTime * shakeAmt);
			
			Vector3 newPos = transform.position;
			newPos.x = x0 + Mathf.Sin(Time.time * speed) * shakeAmt;
			//newPos.x = newPos.x + transform.position.x;
			//newPos.y = transform.position.y;

			transform.position = newPos;
		}
	}

	public void ShakeMe() {
		StartCoroutine(ShakeNow());
	}

	IEnumerator ShakeNow() {
		Debug.Log("Start Shaking");
		Vector3 originalPos = transform.position;
		x0 = originalPos.x;
		if (!shaking)
			shaking = true;
		
		yield return new WaitForSeconds(0.5f);
		shaking = false;
		transform.position = originalPos;
		Debug.Log("Done SHaking");
	}
}
