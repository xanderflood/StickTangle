using UnityEngine;
using System.Collections;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Magnet : Piece {


    protected override void Awake()
    {
        base.Awake();

		Color temp = new Color();
		
		temp.r = 1f;
		temp.g = 0f;
		temp.b = 0f;
		temp.a = 1f;
        this.renderer.material.color = temp;
    }


	private void Start() {
		if (LevelManager.modeling)
			return;

		grid.SetSquare(row, col, new Square(SquareType.Magnet));
		grid.magnetMap.Add(pos, this);
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
