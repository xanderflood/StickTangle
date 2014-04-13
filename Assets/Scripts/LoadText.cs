using UnityEngine;
using System.Collections;

public class LoadText : MonoBehaviour {
	private void Start() {
		Utils.FindComponent<LevelManager>("LevelManager").SetText();
	}
}
