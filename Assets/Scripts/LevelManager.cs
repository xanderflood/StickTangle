using UnityEngine;
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
	bool restarting = false;

	private void Start() {
		level = Application.loadedLevel;
		TextMesh mesh = Utils.FindComponent<TextMesh>("Narrator");
		mesh.text = messages[level - 1];

		DataLogger.Initialize();
	}

	public void AdvanceLevel() {
		DataLogger.Win();
		Application.LoadLevel(++level);
	}

	public void Restart() {
		DataLogger.Restart();
		Application.LoadLevel(level);
		restarting = true;
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
	
	// Make sure that data gets saved when the player quits
	void OnDestroy() {
		if(!restarting)
			DataLogger.Save();
	}
}
