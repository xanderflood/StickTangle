using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


using LevelState = XmlLoader.LevelState;

public class LevelManager : MonoBehaviour {
    public Font JulieFont;
    public Font DamienFont;
	public AudioClip restart;

	public AudioClip[] sounds;

	private List<LevelState> levelStates;
	private List<string> scenes;
	public int levelIndex = -1;
	bool restarting = false;

	public static bool modeling = false;

    float virtualWidth = 960.0f; //create gui for this size, use matrix to automaticly scale it
    float virtualHeight = 600.0f;

	private bool InScene() {
		return scenes.Contains(Application.loadedLevelName);
	}


	private void Awake() {
		Pair<List<LevelState>, List<string>> result = XmlLoader.LoadXml("levels");
		levelStates = result.First;
		scenes = result.Second;
				
		if (InScene()) {
			return;
		}

		SetIndex(Application.loadedLevelName);

		DataLogger.Initialize(this);
	}

	public void SetIndex(string name) {
		levelIndex = -1;
		for (int i = 0; i < levelStates.Count; i++) {
			if (levelStates[i].name == name) {
				levelIndex = i;
				break;
			}
		}
		Utils.Assert(levelIndex != -1);
	}

	public bool CurrentLevelInRange(string l1, string l2) {
		if (InScene()) {
			return false;
		}

		string[] parts1 = l1.Split('.');
		string[] parts2 = l2.Split('.');
		
		Utils.Assert(parts1.Length == 2);
		Utils.Assert(parts2.Length == 2);

		int stage1 = Convert.ToInt32(parts1[0]);
		int stage2 = Convert.ToInt32(parts2[0]);

		int stage = levelStates[levelIndex].stage;
		if (stage1 < stage && stage < stage2) {
			return false;
		} else if (stage1 == stage && stage == stage2) {
			int level = levelStates[levelIndex].level;
			int level1 = Convert.ToInt32(parts1[1]);
			int level2 = Convert.ToInt32(parts2[1]);
			if (level1 <= level && level <= level2) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

    void OnGUI()
    {
		if (LevelManager.modeling)
			return;

         var DaimenStyle = GUI.skin.GetStyle("Label");
         DaimenStyle.alignment = TextAnchor.UpperLeft;
         DaimenStyle.wordWrap = true;
         DaimenStyle.fontSize = 25;
         DaimenStyle.font = DamienFont;
         DaimenStyle.normal.textColor = Color.black;
         DaimenStyle.fontStyle = FontStyle.Bold;

         //scale the gui stuff with screen size
         GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1.0f));


         string[] text = new string[20]; // longer than we will need
         for (int i = 0; i < text.Length; i++) {
             text[i] = "";   
         }

         int offset = 0;
        // hcorner, vcorner, width, height
         foreach (string s in levelStates[levelIndex].narrationText1)
         {
             text[2 * offset] = s;

             offset++;
         }
         offset = 0;
         foreach (string s in levelStates[levelIndex].narrationText2)
         {

             text[2 * offset + 1] = s;
             offset++;
         }
         var JulieStyle = DaimenStyle;
         JulieStyle.font = JulieFont;
            //plot text;
         offset = 0;
         for (int i = 0; i < text.Length; i++)
         {
             if (text[i].Length > 1)
             {
                 if (i % 2 == 0)
                 {
                     GUI.Label(new Rect(20, 50 + 120 * offset, 220, 500), text[i], JulieStyle);
                 }
                 else {
                     GUI.Label(new Rect(20, 50 + 120 * offset, 220, 500), text[i], DaimenStyle);
                 }
                 offset++;
             }
         }
    }

	public void SetText() {
        /*
		TextMesh mesh = Utils.FindComponent<TextMesh>("Narrator");
		foreach (string s in levelStates[levelIndex].narrationText1) {
            Debug.Log(s);
            mesh.text += s;
            mesh.text += "\n\n";
		}
		foreach (string s in levelStates[levelIndex].narrationText2) {
            mesh.text += s;
            mesh.text += "\n\n";
			Debug.Log(s);
		}
		//mesh.text = levelStates[levelIndex].narrationText;
         * */
        TextMesh mesh = Utils.FindComponent<TextMesh>("LevelText");
		mesh.text = "Level " + levelStates[levelIndex].stage + "." + levelStates[levelIndex].level;
	}

	public void AdvanceLevel() {
		DataLogger.Win();
		levelIndex++;
		if (levelIndex >= levelStates.Count) {
			Application.LoadLevel("PlayAgain");
			Destroy(this);
		}
		Application.LoadLevel(levelStates[levelIndex].name);
	}

	public void Restart() {
		DataLogger.Restart();
		Application.LoadLevel(Application.loadedLevel);
		restarting = true;
	}

	public LevelState GetLevelState() {
		return levelStates[levelIndex];
	}

	private void Update() {
		if (modeling)
			return;

		if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(DelayRestart());
		}
	}

	private IEnumerator DelayRestart() {
		// Disable movement during restart
		Utils.FindComponent<Sticker>("Player").done = true;
		audio.clip = sounds[UnityEngine.Random.Range(0, 3)];
		audio.Play ();
		//Camera.main.audio.PlayOneShot(restart);
		yield return new WaitForSeconds(0.5f);
		Restart();
	}
	
	// Make sure that data gets saved when the player quits
	private void OnDestroy() {
		if (!restarting) {
			DataLogger.Save();
		}
	}
}
