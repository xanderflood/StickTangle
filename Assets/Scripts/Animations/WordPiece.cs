using UnityEngine;
using System.Collections;

public class WordPiece : MonoBehaviour {

	public GameObject letterModel;
	public string word;

	LetterPiece[] gos;

	// Use this for initialization
	void Start () {

		gos = new LetterPiece[word.Length];
		int i = 0;
		Vector3 position = transform.position;
		foreach (char l in word) {

			gos[i] = (((GameObject)Instantiate(letterModel, position, Quaternion.identity))
			          .transform.FindChild("Block").GetComponent<LetterPiece>());
			gos[i].letter = l.ToString();
			
			gos[i].BGcolor = RandomColor(0.3f);
			gos[i].fontColor = RandomColor(1f);

			gos[i].gameObject.transform.parent.parent = gameObject.transform;

			position.x += 2f;
			++i;
		}
	}

	public static Color RandomColor(float a = 1f) {
		Color c;
		c.r = Random.Range(0f, 1f);
		c.b = Random.Range(0f, 1f);
		c.g = Random.Range(0f, 1f);
		c.a = a;
		return c;
	}
}
