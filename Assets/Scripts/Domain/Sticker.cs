using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;

public class Sticker : Piece {

	public Material stickerMat;

	public Dictionary<Position, Stickable> stickableMap = new Dictionary<Position, Stickable>();

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	private bool done = false;

	private LevelManager lm;
	private List<Stickable> stickables = new List<Stickable>();
	
	private new void Start() {
		base.Start();
		lm = Camera.main.GetComponent<LevelManager>(); // TODO
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

		foreach (Stickable s in stickables) {
			newR = s.row + dr;
			newC = s.col + dc;

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

		StartCoroutine(move(dr, dc));

		foreach (Stickable s in stickables) {
			StartCoroutine(s.move(dr, dc));
		}

		return true;
	}
	
	private void Update() {
		// Return if already moving
		if (inMotion || done)
			return;

		// Return if no arrow key was pressed
		if (!movePieces()) {
			return;
		}

		// Check for new stickables next to root piece
		List<Stickable> toAdd = GetStickables(row, col);

		// Check for new stickables next to other pieces
		foreach (Stickable s in stickables) {
			toAdd.AddRange(GetStickables(s.row, s.col));
		}

		stickables.AddRange(toAdd);

		// Check if all goals are covered
		if (grid.CheckAllGoals()) {
			done = true;
			StartCoroutine(AdvanceLevel());
		}
	}

	private List<Stickable> GetStickables(int r, int c) {
		List<Stickable> toAdd = new List<Stickable>();
		List<Position> positions = grid.GetStickables(r, c);
		for (int i = 0; i < positions.Count; i++) {
			Stickable s;
			stickableMap.TryGetValue(positions[i], out s);
			Utils.Assert(s);

			toAdd.Add(s);
			s.renderer.material = stickerMat;
			stickableMap.Remove(positions[i]);
			grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Player));
		}

		return toAdd;
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
