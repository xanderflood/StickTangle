using UnityEngine;
using System.Collections;

public class OptionsButtonForLevelSelect : MonoBehaviour {
	
	private LevelManager lm;
	
	float virtualWidth = 960.0f; //create gui for this size, use matrix to automaticly scale it
	float virtualHeight = 600.0f;
	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
	}
	
	private void OnGUI() {

		if (LevelManager.optionsScreen)
			return;

		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1.0f));

		
		if ((GUI.Button(new Rect(virtualWidth*0.8365f, virtualHeight*0.165f,
		                         virtualWidth*0.08f, virtualHeight*0.0533f), "Options"))) {
			lm.LoadOptionsMenu();
		}
	}
}
