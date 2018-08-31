using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeSample : MonoBehaviour {
	Shaker myShake;
	// Use this for initialization
	void Start () {
		myShake = gameObject.GetComponent<Shaker>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shake() {
		myShake.ShakeMe();
	}
}
