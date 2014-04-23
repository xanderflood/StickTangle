using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {

	private LevelManager lm;

	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
	}
	
	private void OnGUI() {
		if (LevelManager.modeling)
			return;

		if ((GUI.Button(new Rect(650, 30, 50, 20), "Menu")) || (Input.GetKey(KeyCode.Joystick1Button18))){
			Application.LoadLevel("LevelSelect");
		}
		
		if ((GUI.Button(new Rect(720, 30, 50, 20), "Skip")) || (Input.GetKey(KeyCode.Joystick1Button17))) {
			lm.AdvanceLevel();
		}
	}
}
