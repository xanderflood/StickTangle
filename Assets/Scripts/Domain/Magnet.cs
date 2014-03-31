using UnityEngine;
using System.Collections;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Magnet : Piece {
	private void Start() {
		grid.SetSquare(row, col, new Square(SquareType.Magnet));
	}
}
