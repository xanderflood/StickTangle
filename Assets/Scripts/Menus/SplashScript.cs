using UnityEngine;
using System.Collections;

public class SplashScript : MonoBehaviour {
	

	void Update() {

		if (Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel(1);
		}
	}
}
