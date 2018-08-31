using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	public GameObject cubePrefab;
	private List<Vector3> cubeLocations = new List<Vector3>();
	private Color[] colors;
	private int numCubes;
	private float myScale;

	Color[] defaults = {ColorPicker.RED, ColorPicker.YELLOW, ColorPicker.GREEN, ColorPicker.BLUE};

	// Initializes cube locations and scale for the game dependent on numCubes
	void InitialiseProps() {
		switch (numCubes) {
			case 1:
				cubeLocations.Add(new Vector3(0, -7f, 0));
				myScale = 1.8f;
				break;
			case 2:
				cubeLocations.Add(new Vector3(0, -1f, 0));
				cubeLocations.Add(new Vector3(0, -12f, 0));
				myScale = 1.2f;
				break;
			case 3:
				cubeLocations.Add(new Vector3(-5.0f, -2.0f, 0));
				cubeLocations.Add(new Vector3(5.0f, -2.0f, 0));
				cubeLocations.Add(new Vector3(0, -12f, 0));
				myScale = 1.0f;
				break;
			case 4:
				cubeLocations.Add(new Vector3(-5.0f, -2.0f, 0));
				cubeLocations.Add(new Vector3(5.0f, -2.0f, 0));
				cubeLocations.Add(new Vector3(-5.0f, -12.0f, 0));
				cubeLocations.Add(new Vector3(5.0f, -12.0f, 0));
				myScale = 1.0f;
				break;
		}
		Debug.Log("Number of items: " + cubeLocations.Count.ToString());
	}

	// Gets color settings from PlayerPrefsX
	void AssignColor() {
		colors = defaults;
	}

	// Initializes the cubes given num number of cubes
	public void SetupScene(int num) {
		numCubes = num;
		InitialiseProps();
		AssignColor();
		StartCoroutine(PlaceBlocks());
	}

	IEnumerator PlaceBlocks() {
		for (int i = 0; i < numCubes; i++) {
			Debug.Log("index " + i.ToString());
			GameObject newCube = (GameObject) Instantiate(cubePrefab, cubeLocations[i], transform.rotation);
			newCube.transform.localScale = new Vector3(myScale, myScale, myScale);
			newCube.GetComponentInChildren<SpriteRenderer>().color = colors[i];
			GameManager.instance.AddCubeToList(newCube.GetComponent<CubeManager>());
			yield return new WaitForSeconds(0.2f);
		}
	}

	void Shuffle() {
		int n = cubeLocations.Count;
		while (n > 1) {
			n--;
			int k = Random.Range(0, n+1);
			Vector3 v = cubeLocations[k];
			cubeLocations[k] = cubeLocations[n];
			cubeLocations[n] = v;
		}
	}

	public void RandomPositions() {
		Shuffle();
		int i = 0;
		foreach (CubeManager c in GameManager.instance.myCubes) {
			c.ChangeLocation(cubeLocations[i++]);
		}
	}
}
