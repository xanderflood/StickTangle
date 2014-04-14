using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using LevelState = XmlLoader.LevelState;

public class LevelManager : MonoBehaviour {
	public AudioClip restart;

	private List<LevelState> levelStates;
	public int levelIndex = -1;
	bool restarting = false;

	private void Awake() {
		levelStates = XmlLoader.LoadXml("levels.xml");

		for (int i = 0; i < levelStates.Count; i++) {
			if (levelStates[i].name == Application.loadedLevelName) {
				levelIndex = i;
				break;
			}
		}
		Utils.Assert(levelIndex != -1);

		DataLogger.Initialize(this);
	}

	public bool CurrentLevelInRange(string l1, string l2) {
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

	public void SetText() {
		TextMesh mesh = Utils.FindComponent<TextMesh>("Narrator");
		mesh.text = levelStates[levelIndex].narrationText;
		mesh = Utils.FindComponent<TextMesh>("LevelText");
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
		if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(DelayRestart());
		}
	}

	private IEnumerator DelayRestart() {
		// Disable movement during restart
		Utils.FindComponent<Sticker>("Player").done = true;
		Camera.main.audio.PlayOneShot(restart);
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
