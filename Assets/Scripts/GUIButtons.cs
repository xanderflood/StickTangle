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

		if (!LevelManager.modeling) {
			if ((GUI.Button(new Rect(virtualWidth*0.83f, virtualHeight*0.1f,
			                         virtualWidth*0.06f, virtualHeight*0.04f), "Menu")) || (Input.GetKey(KeyCode.Joystick1Button18))){
				Application.LoadLevel("LevelSelect");
			}
			
			if ((GUI.Button(new Rect(virtualWidth*0.83f, virtualHeight*0.15f,
			                         virtualWidth*0.06f, virtualHeight*0.04f), "Skip")) || (Input.GetKey(KeyCode.Joystick1Button17))) {
				lm.AdvanceLevel();
			}
		}

		if ((GUI.Button(new Rect(virtualWidth*0.83f, virtualHeight*0.2f,
		                         virtualWidth*0.06f, virtualHeight*0.04f), "Options"))) {
			lm.LoadOptionsMenu();
		}
	}
}
