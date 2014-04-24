using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DissolveAnimation : MonoBehaviour {

	public List<GameObject> squares;
	public int dr;
	public int dc;

	public Piece owner;

	public Acid target;

	bool animating;
	
	// Update is called once per frame
	void Update () {

		if (!animating) {
			StartCoroutine(Dissolve());
			//StartCoroutine(Move());
		}
	}

//	IEnumerator Move() {
//
//		Vector3 disp = new Vector3(dc, dr, 0);
//
//		Vector3 pos;
//		for (int i = 0; i < 1.0f/Piece.speed; ++i) {
//			pos = gameObject.transform.position;
//			pos += disp * Piece.speed;
//			gameObject.transform.position = pos;
//
//			yield return true;
//		}
//	}

	IEnumerator Dissolve() {
		animating = true;

		foreach (GameObject sq in squares)
			if (sq != null)
				sq.renderer.material.color = owner.renderer.material.color;

		//owner.renderer.enabled = false;
		owner.transform.FindChild("Quad").renderer.enabled = false;

		StartCoroutine (fadeObject (target.gameObject, 3f));
		StartCoroutine (fadeObject (owner.gameObject, 3f));

		for (int i = 0; i < 4; ++i) {
			dissolveRow(i);

			yield return new WaitForSeconds(0.0025f/Piece.speed);

			Color c = target.renderer.material.color;
			c.a -= 0.25f;
			target.renderer.material.color = c;
		}

		yield return new WaitForSeconds(.3f);

		GameObject.Destroy(owner.gameObject);
		GameObject.Destroy(target.gameObject);

		//gameObject.transform.parent.gameObject.GetComponent<Stickable>().DestroyPiece();
		GameObject.Destroy(gameObject);
	}

	public static IEnumerator fadeObject(GameObject go, float rate) {

		Color col = go.renderer.material.color;
		while (col.a > -1) {
			col.a -= rate*Time.deltaTime;
			go.renderer.material.color = col;

			yield return true;
		}

		GameObject.Destroy(go);
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
			if (squares[pos] != null) {
				DissolveOneSquare dos = squares[pos].GetComponent<DissolveOneSquare>();
				dos.dissolving = true;
			}
			pos += jump;
		}
	}
}
