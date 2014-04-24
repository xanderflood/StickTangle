using UnityEngine;
using System.Collections;

public class DisableWhenModeling : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (LevelManager.modeling)
			GameObject.Destroy(gameObject);
	}
}
