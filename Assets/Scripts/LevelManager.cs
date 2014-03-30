using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
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

	private void Update() {
		if (Input.GetKey(KeyCode.R)) {
			Application.LoadLevel(level);
		}
	}
}
