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

	private string previousScene;

	public bool colorblindMode = false;
	
	public static bool modeling = false;
	public static bool optionsScreen = false;

    float virtualWidth = 960.0f; //create gui for this size, use matrix to automaticly scale it
    float virtualHeight = 600.0f;

	public bool InScene() {
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
    /* the nicer way I did text before. Sticking with textmesh to look consistent with press R to restart
    it is increadibly overbuilt what what I'm doing now, if we wind up keeping text in this format I'll clean it up
     */
    void OnGUI()
    {
		if (levelIndex == -1) {
			return;
		}
		if (LevelManager.modeling || LevelManager.optionsScreen)
			return;

         var textStyle = GUI.skin.GetStyle("Label");
         textStyle.alignment = TextAnchor.UpperLeft;
         textStyle.wordWrap = true;
         textStyle.fontSize = 28;
         textStyle.font = JulieFont;
         textStyle.fontStyle = FontStyle.Normal;
         Color gray = new Color();
         gray.r = 71f / 255f;
         gray.g = 71f / 255f;
         gray.b = 71f / 255f;
         gray.a = 1;
         textStyle.normal.textColor = gray;

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
            //plot text;
         offset = 0;
         for (int i = 0; i < text.Length; i++)
         {
             if (text[i].Length > 1)
             {
                 if (i % 2 == 0)
                 {
                     textStyle.font = JulieFont;
                     textStyle.fontStyle = FontStyle.Normal;

                     GUI.Label(new Rect(20, 120 + 130 * offset, 220, 500), text[i], textStyle);
                 }
                 else {
                     textStyle.font = DamienFont;
                     GUI.Label(new Rect(20, 120 + 130 * offset, 220, 500), text[i], textStyle);
                 }
                 offset++;
             }
         }
    }
    
	public void SetText() {

        TextMesh mesh2 = Utils.FindComponent<TextMesh>("LevelText");
		mesh2.text = "Level " + levelStates[levelIndex].stage + "." + levelStates[levelIndex].level;
	}

	public void AdvanceLevel() {
		if (Application.loadedLevelName == "PlayAgain") {
			levelIndex = -1;
			Application.LoadLevel("LevelSelect");
			return;
		}

		DataLogger.Win();
		levelIndex++;
		if (levelIndex >= levelStates.Count) {
			Application.LoadLevel("PlayAgain");
			levelIndex = -1;
		}
		Application.LoadLevel(levelStates[levelIndex].name);
	}

	public void Restart() {
		DataLogger.Restart();
		Application.LoadLevel(Application.loadedLevel);
		restarting = true;
	}


	// These variables should only be used by the following two functions
	Rect oldViewport;
	GameObject selection;
	GameObject player;
	string previousLevel;
	public void LoadOptionsMenu() {

		previousLevel = Application.loadedLevelName;

		Application.LoadLevelAdditive("Options");

		GameObject ambientScene = GameObject.Find("LevelSelect");
		GameObject camera;
		if (ambientScene == null) {
			ambientScene = GameObject.Find("Level");
			camera = GameObject.Find("Camera");
		} else
			camera = GameObject.Find("Main Camera");

		Vector3 pos = ambientScene.transform.position;
		pos.x += 3000;
		ambientScene.transform.position = pos;

		oldViewport = camera.camera.rect;
		camera.camera.rect = new Rect(0, 0, 0, 0);
		
		selection = GameObject.Find("SelectionGUI");
		if (selection != null)
			selection.SetActive(false);

		player = GameObject.Find("Player");
		if (player != null)
			player.SetActive(false);
	}
	
	
	public void ReturnFromOptionsMenu(bool restart) {

		if (restart) {
			Application.LoadLevel(previousLevel);
			return;
		}

		GameObject oScreen = GameObject.Find("OptionsScreen");
		GameObject.Destroy(oScreen);
		
		GameObject ambientScene = GameObject.Find("LevelSelect");
		GameObject camera;
		if (ambientScene == null) {
			ambientScene = GameObject.Find("Level");
			camera = GameObject.Find("Camera");
		} else
			camera = GameObject.Find("Main Camera");

		Vector3 pos = ambientScene.transform.position;
		pos.x -= 3000;
		ambientScene.transform.position = pos;
		
		camera.camera.rect = oldViewport;
		
		if (selection != null)
			selection.SetActive(true);
		if (player != null)
			player.SetActive(true);

		selection = null;
		player = null;
	}

	public LevelState GetLevelState() {
		return levelStates[levelIndex];
	}

	private void Update() {
		if (modeling)
			return;

		if ((Input.GetKeyDown(KeyCode.R)) || (Input.GetKeyDown(KeyCode.Joystick1Button16))) {
			StartCoroutine(DelayRestart());
		}
	}

	private IEnumerator DelayRestart() {
		// Disable movement during restart
		Utils.FindComponent<Sticker>("Player").done = true;
		audio.clip = sounds[UnityEngine.Random.Range(0,sounds.Length)];
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
