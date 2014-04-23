using UnityEngine;
using System.Collections;

public class MainMenuAnimation : MonoBehaviour {

	public GameObject RectWord;
	public GameObject StickyWord;

	public float rate;

	void Awake () {
		LevelManager.modeling = true;
	}

	void Start () {
		StartCoroutine(InitialWait());
	}

	IEnumerator InitialWait() {
		yield return new WaitForSeconds(1f);
		StartCoroutine(MoveRect());
		StartCoroutine(MoveSticky());
	}

	IEnumerator MoveRect() {

		Vector3 tmp;

		bool glowing = false;
		//First, move downwards to the magnet
		while(RectWord.transform.position.y > -4) {
			tmp = RectWord.transform.position;
			tmp.y -= rate*Time.deltaTime;
			RectWord.transform.position = tmp;

			if (tmp.y < -3 && !glowing) {
				for (int i = 0; i < 3; ++i)
					RectWord.GetComponent<WordPiece>().Letters[i].StartMagnetGlow();

				glowing = true;
			}

			yield return true;
		}

		// Leave those behind and start fading them out, with the magnets

		// Change the letter 't' to a capital, and move back, but one space further up

		// Initialize the actual GUI
	}

	IEnumerator MoveSticky() {
		
		Vector3 tmp;
		bool dissolving = false;

		//First, move downwards to the magnet
		while(StickyWord.transform.position.x < -4) {
			tmp = StickyWord.transform.position;
			tmp.x += rate*Time.deltaTime;
			StickyWord.transform.position = tmp;
			
			if (tmp.x > -6 && !dissolving) {
				StickyWord.GetComponent<WordPiece>().Letters[5].StartAcidAnimation(0, 1);
				
				dissolving = true;
			}
			
			yield return true;
		}
		// More right towards the acid
		// Burn up the y
		// Move back
		yield return true;
	}
}
