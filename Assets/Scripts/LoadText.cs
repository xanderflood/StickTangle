using UnityEngine;
using System.Collections;

public class LoadText : MonoBehaviour {
	private void Start() {
		if (LevelManager.modeling)
			return;

		//Utils.FindComponent<LevelManager>("LevelManager").SetText();
	}
}
