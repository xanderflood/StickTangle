using UnityEngine;
using System.Collections;

/**
 * Syntactic sugar for a Pair<int, int> representing a board position
 */
public class Position : Pair<int, int>{
	public int Row {
		get {
			return First;
		}
		set {
			First = value;
		}
	}

	public int Col {
		get {
			return Second;
		}
		set {
			Second = value;
		}
	}

	public Position(int row, int col) {
		Row = row;
		Col = col;
	}

	public override string ToString() {
		return string.Format("Position({0}, {1})", First, Second);
	}
}
