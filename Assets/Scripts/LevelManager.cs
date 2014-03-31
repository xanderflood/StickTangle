﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public AudioClip restart;
	private static string[] messages = {
		"Level1",
		"Level2",
		"Level3",
		"Level4",
		"Level5",
		"Level6",
		"Level7",
		"Level8",
		"Level9",
        "Level10"
	};

	private int level;

	private void Start() {
		level = Application.loadedLevel;
		TextMesh mesh = Utils.FindComponent<TextMesh>("Narrator");
		mesh.text = messages[level - 1];

	}

	public void AdvanceLevel() {
		Application.LoadLevel(++level);
	}

	public void Restart() {
		Application.LoadLevel(level);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(DelayRestart());
		}
	}

	private IEnumerator DelayRestart() {
		audio.PlayOneShot (restart);
		yield return new WaitForSeconds(0.75f);
		Restart ();
	}
}
