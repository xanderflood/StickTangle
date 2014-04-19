using UnityEngine;
using UnityEditor;
using System.Collections;

// Note: All transforms should be in the Resources folder and have the same name as the selected GameObjects
// You can temporarily move them to the Resources folder and move them back.
// TODO: Find a better way to get the prefab 
public class RevertTransform : MonoBehaviour {
	[MenuItem("Tools/Revert Transform in All Scenes")]
	public static void Revert()	{
		GameObject[] selection = Selection.gameObjects;
		if (selection.Length > 0) {
			// Get names of GameObjects to revert
			string[] names = new string[selection.Length];
			for (int i = 0; i < selection.Length; i++) {
				names[i] = selection[i].gameObject.name;
			}

			// Loop through all scenes
			foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes) {
				if (s.enabled) {
					EditorApplication.OpenScene(s.path);
					foreach (string name in names) {
						GameObject obj = GameObject.Find(name);
						if (obj != null) {
							string sceneName = s.path.Substring(s.path.LastIndexOf('/') + 1);
							Debug.Log("Reverting transform for " + name + " in scene " + sceneName);

                            GameObject parent = GameObject.Find("Goals");
                            GameObject[] objs = GameObject.FindObjectOfType(typeof(obj));
                            for (int i = 0; i < objs.Length; i++) {
                                obj = objs[i];
							    GameObject newObj = Resources.Load<GameObject>(name);
							    newObj = (GameObject) Instantiate(newObj);
                            
							    newObj.transform.position = obj.transform.position;
                                newObj.name = obj.name;
                                newObj.transform.parent = parent.transform;
                                DestroyImmediate(obj);
                            }
						    //EditorApplication.SaveScene();  
                            return;
						}
					}
				}
			}
		} else {
			Debug.Log("Nothing selected");
		}
	}
}
