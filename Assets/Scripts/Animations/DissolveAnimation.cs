using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DissolveAnimation : MonoBehaviour {

	public List<GameObject> squares;
	public int dr;
	public int dc;

	bool animating;
	
	// Update is called once per frame
	void Update () {

		if (!animating) {
			StartCoroutine(Dissolve());
			StartCoroutine(Move());
		}
	}

	IEnumerator Move() {

		Vector3 disp = new Vector3(dc, dr, 0);

		Vector3 pos;
		for (int i = 0; i < 1.0f/Piece.speed; ++i) {
			pos = gameObject.transform.position;
			pos += disp * Piece.speed;
			gameObject.transform.position = pos;

			yield return true;
		}
	}

	IEnumerator Dissolve() {
		animating = true;

		for (int i = 0; i < 4; ++i) {
			dissolveRow(i);

			yield return new WaitForSeconds(0.0025f/Piece.speed);
		}
	}

	void dissolveRow(int rowNum) {

		int pos = 0, jump = 0;
		if (dr < 0) {
			pos = 15 - 4*rowNum; jump = -1;
		}
		if (dr > 0) {
			pos = 4*rowNum; jump = 1;
		}
		if (dc < 0) {
			pos = rowNum; jump = 4;
		}
		if (dr > 0) {
			pos = 15 - rowNum; jump = -4;
		}

		for (int i = 0; i < 4; ++i) {
			DissolveOneSquare dos = squares[pos].GetComponent<DissolveOneSquare>();
			dos.dissolving = true;
			pos += jump;
		}
	}
}
