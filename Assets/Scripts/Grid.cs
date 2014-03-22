using UnityEngine;
using System.Collections;
using System.Text;

public class Grid : MonoBehaviour {

	public const int Dim = 11;

	// Makes all of the position to coordinate computations work out
	private const float magicConst = (Dim - 1) / 2;

	public enum SquareType {
		Player, Empty, Goal
	}

	public class Square {
		public SquareType type;

		public Square(SquareType type) {
			this.type = type;
		}
	}

	public enum Direction {
		Up, Down, Left, Right
	}

	private Position[] directionOffsets = { 
		new Position(0, 1), new Position(0, -1), // Up, Down
		new Position(-1, 0), new Position(1, 0) }; // Left, Right
	
	private Square[,] grid;

	void Awake() {
		grid = new Square[Dim, Dim];
		for (int i = 0; i < Dim; i++) {
			for (int j = 0; j < Dim; j++) {
				SquareType type = SquareType.Empty;
				grid[i, j] = new Square(type);
			}
		}
	}

	/**
	 * Checks all of the squares in direction dir for a square of type type. If found, returns a the position.
	 * Otherwise, returns null.
	 */
	public Position FindSquareOfType(Direction dir, int row, int col, SquareType type) {
		int stepC = directionOffsets[(int) dir].First;
		int stepR = directionOffsets[(int) dir].Second;

		int i;
		for (i = 1; grid[row + stepR * i, col + stepC * i].type != type; i++) {}

		if (i == 1) {
			return null;
		} else {
			i--;
			return new Position(row + stepR * i, col + stepC * i);
		}
	}

	public Position FindSquareOfType(Direction dir, Position pos, SquareType type) {
		return FindSquareOfType(dir, pos.First, pos.Second, type);
	}
		
	/**
	 * Prints the grid to the console.
	 */
	public void Print() {
		StringBuilder sb = new StringBuilder();
		for (int i = Dim - 1; i >= 0; i--) {
			for (int j = 0; j < Dim; j++) {
				switch(grid[i, j].type) {
				case SquareType.Empty:
					sb.Append("E");
					break;
				case SquareType.Goal:
					sb.Append("G");
					break;
				case SquareType.Player:
					sb.Append("P");
					break;
				}
			}
			sb.AppendLine("");
		}

		Debug.Log(sb.ToString());
	}

	public void SetSquare(int row, int col, Square square) {
		BoundsCheck(row, col);

		grid[row, col] = square;
	}

	public Square GetSquare(int row, int col) {
		BoundsCheck(row, col);
		
		return grid[row, col];
	}

	public Pair<SquareType, Position> SquareTypeAt(Direction dir, int row, int col) {
		if (!InBounds(row, col)) {
			return null;
		}

		int stepC = directionOffsets[(int) dir].First;
		int stepR = directionOffsets[(int) dir].Second;
		int r = row + stepR;
		int c = col + stepC;

		Position pos = new Position(r, c);
		return new Pair<SquareType, Position>(grid[r, c].type, pos);
	}

	public bool IsEmpty(int row, int col) {
		BoundsCheck(row, col);

		SquareType t = grid[row, col].type;
		return t == SquareType.Empty;
	}

	public Vector3 PosToCoord(int row, int col) {
		BoundsCheck(row, col);

		return new Vector3(-1 * magicConst + col, -1 * magicConst + row, -1);
	}

	public Position CoordToPos(Vector3 coord) {
		return CoordToPos(coord, true);
	}

	public Position CoordToPos(Vector3 coord, bool boundsCheck) {
		// The 0.5f is for rounding to the nearest int
		int row = (int) (coord.y + magicConst + 0.5f);
		int col = (int) (coord.x + magicConst + 0.5f);

		if (boundsCheck) BoundsCheck(row, col);

		return new Position(row, col);
	}

	private void BoundsCheck(int row, int col) {
		if (!InBounds(row, col)) {
			throw new System.ArgumentOutOfRangeException("Out of bounds: (" + row + ", " + col + ")");
		}
	}

	private bool InBounds(int row, int col) {
		return row < Dim && col < Dim && row >= 0 && col >= 0;
	}
}
