//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//
//// Note: All transforms should be in the Resources folder and have the same name as the selected GameObjects
//// You can temporarily move them to the Resources folder and move them back.
//// TODO: Find a better way to get the prefab 
//public class RevertTransform : MonoBehaviour
//{
//    [MenuItem("Tools/Revert Transform in All Scenes")]
//    public static void Revert()
//    {
//        GameObject[] selection = Selection.gameObjects;
//        if (selection.Length > 0)
//        {
//            // Get names of GameObjects to revert
//            string[] names = new string[selection.Length];
//            for (int i = 0; i < selection.Length; i++)
//            {
//                names[i] = selection[i].gameObject.name;
//            }
//
//            // Loop through all scenes
//            foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
//            {
//                if (s.enabled)
//                {
//                    EditorApplication.OpenScene(s.path);
//                    foreach (string name in names)
//                    {
//                        GameObject obj = GameObject.Find(name);
//                        if (obj != null)
//                        {
//                            string sceneName = s.path.Substring(s.path.LastIndexOf('/') + 1);
//                            Debug.Log("Reverting transform for " + name + " in scene " + sceneName);
//                       
//                            Sticker[] objs = GameObject.FindObjectsOfType(typeof(Sticker)) as Sticker[];
//                            for (int i = 0; i < objs.Length; i++)
//                            {
//                                Sticker newObj = Resources.Load<Sticker>(name);
//                                newObj = (Sticker) Instantiate(newObj);
//
//                                for (int j = 0; j < newObj.CrayonMats.Count; j++)
//                                {
//                                    objs[i].CrayonMats.Add(newObj.CrayonMats[j]);
//                                }
//                                    //objs[i].CrayonMats = newObj.CrayonMats;
//                                DestroyImmediate(newObj.gameObject);
//                            }
//                            EditorApplication.SaveScene();  
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("Nothing selected");
//        }
//    }
//}
