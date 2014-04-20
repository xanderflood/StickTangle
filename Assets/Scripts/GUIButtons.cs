using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {

	private LevelManager lm;

	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
	}
	
	private void OnGUI() {
		if (GUI.Button(new Rect(650, 30, 50, 20), "Menu")) {
			Application.LoadLevel("LevelSelect");
		}
		
		if (GUI.Button(new Rect(720, 30, 50, 20), "Skip")) {
			lm.AdvanceLevel();
		}
	}
}
