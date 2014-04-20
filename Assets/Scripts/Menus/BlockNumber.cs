using UnityEngine;
using System.Collections.Generic;

public class BlockNumber : MonoBehaviour {

	// Between 1 and 9
	public int num;
	int cur = -1;

	public List<GameObject> numModels;

	GameObject numDisplay;

	// Use this for initialization
	void Start () {
		Update();
	}
	
	// Update is called once per frame
	void Update () {
		if (cur == num)
			return;

		Destroy(numDisplay);
		numDisplay = (GameObject)Instantiate(numModels[num - 1]);
		numDisplay.transform.parent = transform;
		numDisplay.transform.position = transform.position;
		numDisplay.transform.localScale = new Vector3(1, 1, 1);
		cur = num;
	}
}
