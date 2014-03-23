using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;

public class Sticker : MonoBehaviour {

	public Material stuckMat;

	private const float speed = 0.1f;
	private const int layer = -2;

	public int row, col;
	public bool inMotion;

	private LevelManager lm;

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	private bool done = false;

	public List<Stickable> pieces = new List<Stickable>();
	public Dictionary<Position, Stickable> pieceMap = new Dictionary<Position, Stickable>();

	private Grid grid;

	void Start() {
		lm = Camera.main.GetComponent<LevelManager>(); // TODO
		grid = Utils.FindComponent<Grid>("Board");
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
	}

	private bool isValidSquare(int newR, int newC) {
		if (!grid.InBounds(newR, newC)) {
			return false;
		}

		SquareType type = grid.GetSquare(newR, newC).type;

		return type != SquareType.Block;
	}

	private bool isValidMove(int dr, int dc) {
		int newR = row + dr;
		int newC = col + dc;
		if (!isValidSquare(newR, newC)) {
			return false;
		}

		foreach (Stickable piece in pieces) {
			newR = piece.row + dr;
			newC = piece.col + dc;

			if (!isValidSquare(newR, newC)) {
				return false;
			}
		}

		return true;
	}

	// Returns true if pieces were actually moved
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

		if (!isValidMove(dr, dc)) {
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
		if (inMotion || done)
			return;

		// Return if no arrow key was pressed
		if (!movePieces()) {
			return;
		}
		
		// Check if any new pieces should stick to this one
		List<Stickable> toAdd = new List<Stickable> (); // Can't add pieces while iterating

			//Check for the root
		List<Position> positions = grid.GetStickables (row, col);
		for (int i = positions.Count - 1; i >= 0; i--) {
			Stickable piece;
			pieceMap.TryGetValue (positions [i], out piece);
			if (!piece.stuck) {
				pieces.Add (piece);
				piece.renderer.material = stuckMat;
				piece.stuck = true;
			}
		}

			// Check for the children
		foreach (Stickable curr in pieces) {
			positions = grid.GetStickables (curr.row, curr.col);
			for (int i = positions.Count - 1; i >= 0; i--) {
				Stickable piece;
				pieceMap.TryGetValue (positions [i], out piece);
				if (!piece.stuck) {
					toAdd.Add (piece);
					piece.renderer.material = stuckMat;
					piece.stuck = true;
				}
			}
		}

		pieces.AddRange (toAdd);

		// Check if all goals are covered
		if (grid.CheckAllGoals()) {
			done = true;
			StartCoroutine(AdvanceLevel());
		}
	}

	private IEnumerator AdvanceLevel() {
		yield return new WaitForSeconds(0.5f);
		lm.AdvanceLevel();
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
