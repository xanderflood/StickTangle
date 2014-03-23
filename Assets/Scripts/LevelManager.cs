using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	private int level;

	private void Start() {
		level = Application.loadedLevel;
	}

	public void AdvanceLevel() {
		Application.LoadLevel(++level);
	}
}
