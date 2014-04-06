using UnityEngine;
using System.Collections;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Magnet : Piece {
	private void Start() {
		grid.SetSquare(row, col, new Square(SquareType.Magnet));
		grid.magnetMap.Add(new Position(row, col), this);
	}

	public bool IsMovingAway(int r, int c, int dr, int dc) {
		if (row == r) {
			if (dr == 0) {
				return true;
			}
		} else {
			Utils.Assert(col == c);
			if (dc == 0) {
				return true;
			}
		}
		return false;
	}
}
