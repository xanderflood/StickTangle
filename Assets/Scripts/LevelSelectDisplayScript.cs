using UnityEngine;
using System.Collections;
using LevelState = XmlLoader.LevelState;

public class LevelSelectDisplayScript : MonoBehaviour {

	public string text = "";
	public GUIStyle centering;
	public Texture img;

	public LevelState level;

	// false if displaying a stage, true for a level
	public bool mode;
	
	// Update is called once per frame
	void OnGUI () {
		GUI.Label (new Rect (Screen.width/2-50, Screen.height/2-148, 100, 50), text, centering);

		if (!mode)
			GUI.DrawTexture (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), img);
		else {
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 70, 200, 200),
			           "Bronze: " + level.bronzeMoves, centering);
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 200),
			           "Silver: " + level.bronzeMoves, centering);
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 200),
			           "Gold: " + level.bronzeMoves, centering);
		}
	}
}
