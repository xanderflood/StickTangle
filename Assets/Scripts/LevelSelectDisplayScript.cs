using UnityEngine;
using System.Collections;
using LevelState = XmlLoader.LevelState;

public class LevelSelectDisplayScript : MonoBehaviour {

	public string text = "";
	public GUIStyle centering;
	public Texture img;

	public LevelState level;

	public GameObject outline;

	public BlockNumber numberDisplay;

	// false if displaying a stage, true for a level
	public bool mode;

	public int stageNum;

	int curID = -1;
	bool staging = true;

	GameObject levelGO;

	Rect viewport;

	void Start() {
		viewport = new Rect(10, 10, 10, 10);
		viewport.x = 0.38f;
		viewport.y = 0.1f;
		viewport.width = 0.25f;
		viewport.height = 0.25f;

		outline.SetActive(false);
	}

	// Update is called once per frame
	void OnGUI() {
		GUI.Label (new Rect (Screen.width/2-100, Screen.height/2-150, 100, 50), text, centering);

		if (!mode) {
			GUI.DrawTexture (new Rect (Screen.width / 2 - 100,
			                           Screen.height / 2 - 100, 200, 200), img);
			staging = true;
			GameObject.Destroy(levelGO);
			outline.SetActive(false);
		} else {
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 90, 200, 200),
			           "Bronze: " + level.bronzeMoves, centering);
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 200),
			           "Silver: " + level.bronzeMoves, centering);
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 10, 200, 200),
			           "Gold: " + level.bronzeMoves, centering);

			StartCoroutine(loadScreencap());
		}
		
		numberDisplay.num = stageNum;
	}

	IEnumerator loadScreencap() {

		if (staging || curID != level.id) {
			staging = false;
			curID = level.id;
			GameObject.Destroy(levelGO);
			
			Application.LoadLevelAdditive(level.id);

			yield return true;

			GameObject camObj = GameObject.Find("Camera").gameObject;
			GameObject.Destroy(camObj.GetComponent<AudioListener>());
			Camera cam = camObj.GetComponent<Camera>();

			levelGO = GameObject.Find ("Level");
			levelGO.transform.position = new Vector3(1000, 1000, 0);

			cam.rect = viewport;
			
			outline.SetActive(true);
		}
	}
}
