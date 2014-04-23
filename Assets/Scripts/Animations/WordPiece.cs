using UnityEngine;
using System.Collections;

public class WordPiece : MonoBehaviour {

	public GameObject letterModel;
	public string word;

	LetterPiece[] gos;
	public LetterPiece[] Letters { get { return gos; } }

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
	
	public void DeleteLastLetter() {
		word = word.Remove(word.Length - 1);
	}
	
	public LetterPiece SeverFirstLetter() {
		word = word.Remove(0, 1);

		LetterPiece lp = gos[0];
		LetterPiece[] temp = new LetterPiece[word.Length];

		for (int i = 0; i < word.Length; ++i)
			temp[i] = gos[i + 1];
		//lp.transform.parent = null;

		gos = temp;
		return lp;
	}

	public static Color RandomColor() {
		return RandomColor(1f);
	}

	public static Color RandomColor(float a) {
		Color c;
		c.r = Random.Range(0f, 1f);
		c.b = Random.Range(0f, 1f);
		c.g = Random.Range(0f, 1f);
		c.a = a;
		return c;
	}
}
