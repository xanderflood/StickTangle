using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

// Note: All transforms should be in the Resources folder and have the same name as the selected GameObjects
// You can temporarily move them to the Resources folder and move them back.
// TODO: Find a better way to get the prefab 
public class MoveStickableHolder : MonoBehaviour {
	[MenuItem("Tools/Move StickableHolder")]
	public static void MoveStickableHolder() {

		// Loop through all enabled scenes
		foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes) {
			if (!s.enabled)
				continue;

			EditorApplication.OpenScene(s.path);

			// Try to find the Player and Level objects, and if you
			// can't, then skip this scene: it's not a level
			GameObject player = GameObject.Find("Player");
			if (player == null) {
				Debug.Log("No player: " + s.path.LastIndexOf('/') + 1);
				continue;
			}
			GameObject level = GameObject.Find("Player");
			if (level == null) {
				Debug.Log("No level: " + s.path.LastIndexOf('/') + 1);
				continue;
			}

			// Grab the holder and move it
			GameObject shldr = player.transform.FindChild("StickableHolder");
			shldr.transform.parent = level;

			EditorApplication.SaveScene();
		}

	}
}
