using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Stickable : Piece {
	private void Start() {
		grid.SetSquare(row, col, new Square(SquareType.Stickable));
		
		Sticker s = Utils.FindComponent<Sticker>("Player");
		s.stickableMap.Add(new Position(row, col), this);
	}

	protected override void DestroyPiece() {
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		Destroy(this.gameObject);
	}
}
