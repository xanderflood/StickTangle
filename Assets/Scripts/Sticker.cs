using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sticker : MonoBehaviour {

	public Material stuckMat;

	private const float speed = 0.1f;
	private const int layer = -2;

	public int row, col;
	public bool inMotion;

	public int currentMove = 0;

	public List<Stickable> pieces = new List<Stickable>();

	private Grid grid;

	void Start() {
		grid = GameObject.Find("Board").GetComponent<Grid>();
		DebugUtils.Assert(grid);
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
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

		grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Empty));
		row += dr;
		col += dc;
		grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Player));

		StartCoroutine(move(grid.PosToCoord(row, col, layer)));

		foreach (Stickable piece in pieces) {
			StartCoroutine(piece.move(dr, dc));
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
		for (int i = positions.Count - 1; i >= 0; i--) {
			Stickable piece;
			Stickable.pieces.TryGetValue(positions[i], out piece);
			pieces.Add(piece);
			piece.renderer.material = stuckMat;
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
