using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {

	private LevelManager lm;

    float virtualWidth = 960.0f; //create gui for this size, use matrix to automaticly scale it
    float virtualHeight = 600.0f;
	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
	}
	
	private void OnGUI() {
		if (LevelManager.modeling)
			return;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1.0f));

 		if ((GUI.Button(new Rect(732, 40, 45, 20), "Menu")) || (Input.GetKey(KeyCode.Joystick1Button18))){
			Application.LoadLevel("LevelSelect");
		}
		
		if ((GUI.Button(new Rect(823, 40, 45, 20), "Skip")) || (Input.GetKey(KeyCode.Joystick1Button17))) {
			lm.AdvanceLevel();
		}
	}
}
