using UnityEditor;
using UnityEngine;
using System.Collections;

public class RevertToPrefabState : MonoBehaviour {
	[MenuItem("Tools/Revert to Prefab %r")]
	public static void Revert() {
		GameObject[] selection = Selection.gameObjects;
		
		if (selection.Length > 0) {
			for (int i = 0; i < selection.Length; i++) {
				PrefabUtility.RevertPrefabInstance(selection[i]);
			}
		} else {
			Debug.Log("Nothing selected");
		}
	}
}
