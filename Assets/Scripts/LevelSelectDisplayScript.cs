using UnityEngine;
using System.Collections;

public class LevelSelectDisplayScript : MonoBehaviour {

	public string text = "";
	public GUIStyle centering;
	public Texture img;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUI.Label (new Rect (Screen.width/2-50, Screen.height/2-125, 100, 50), text, centering);
		GUI.DrawTexture (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 200), img);
	}
}
