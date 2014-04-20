using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!LevelManager.modeling)
			return;

		GameObject.Destroy(gameObject.GetComponent<AudioListener>());
		GameObject.Destroy(this);
	}
}
