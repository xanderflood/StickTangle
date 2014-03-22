using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sticker : MonoBehaviour {

	private const float speed = 0.1f;
	private const int layer = -2;

	public int row, col;
	public bool inMotion;

	public int currentMove = 0;

	public List<GameObject> pieces = new List<GameObject>();

	private Grid grid;

	void Start() {
		grid = GameObject.Find("Board").GetComponent<Grid>();
		DebugUtils.Assert(grid);
		row = 3;
		col = 5;
	}

	public bool movePieces() {
		int dr = 0;
		int dc = 0;
		if (Input.GetKey(KeyCode.UpArrow)) {
			dr = 1;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			dr = -1;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			dc = 1;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			dc = -1;
		} else {
			return false;
		}

		row += dr;
		col += dc;

		StartCoroutine(move(grid.PosToCoord(row, col, layer)));

		foreach (GameObject piece in pieces) {
			Debug.Log (pieces.Count);
			MoveableObject obj = piece.GetComponent<MoveableObject>();
			obj.move(dr, dc);
		}

		return true;
	}
	
	void Update () {
		// Return if already moving
		if (inMotion)
			return;

		// Return if no arrow key was pressed
		if (!movePieces()) {
			return;
		}

		List<Position> positions = grid.GetStickables(row, col);
		for (int i = 0; i < positions.Count; i++) {
			Debug.Log ("Adding a pieces");
			// TODO: Set stuck mat
			pieces.Add(null); // TODO
		}
	}

	private IEnumerator move(Vector3 to) {
		inMotion = true;
		
		Vector3 velocity = speed * (to - transform.position).normalized;
		while (transform.position != to) {
			transform.position += velocity;
			yield return null;
		}
		transform.position = to;
		
		inMotion = false;
	}
}
