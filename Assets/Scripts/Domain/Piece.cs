using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Piece : MonoBehaviour {
	protected const float speed = 0.1f;
	protected const int layer = -2;
	protected bool inMotion;
	protected Grid grid;
    private bool hitAcid = false;
	public int row, col;

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

	public IEnumerator move(int dr, int dc) {
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

        // if we ran into acid, we need to destroy this piece at the end of the animation
        if (hitAcid) {
            if (this is Sticker)
            {   //if it is the sticker, let the sticker handle swapping itself around
                hitAcid = false;
                ((Sticker)this).swapWithStickable();
            }
            else
            {   //otherwise this is safe to destroy
                Destroy(this.gameObject);
            }
        }
	}
}
