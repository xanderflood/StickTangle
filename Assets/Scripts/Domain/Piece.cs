using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Piece : MonoBehaviour {
	public int row, col;
	
	protected const float speed = 0.1f;
	protected const int layer = -2;
	protected bool hitAcid = false;
	protected bool inMotion = false;
	protected Grid grid;

	protected void Awake() {
		grid = Utils.FindComponent<Grid>("Board");
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
	}

    public void DestroyAtEndOfMove() {
        hitAcid = true;
    }

	public void ChangePosition(int newRow, int newCol) {
		grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Empty));
		row = newRow;
		col = newCol;
		grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Player));
	}

	protected virtual void DestroyPiece() {
		Log.error("This function should be abstract but Unity is a piece of shit. Don't use me.");
		Utils.Assert(false);
	}

	public IEnumerator Move(int dr, int dc) {
		inMotion = true;

		ChangePosition(row + dr, col + dc);
		
		Vector3 to = grid.PosToCoord(row, col, layer);
		
		Vector3 velocity = speed * (to - transform.position).normalized;
		while (transform.position != to) {
			transform.position += velocity;
			yield return null;
		}
		transform.position = to;
		
		inMotion = false;

        // If we ran into acid, we need to destroy this piece at the end of the animation
        if (hitAcid) {
			DestroyPiece();
        }
	}
}
