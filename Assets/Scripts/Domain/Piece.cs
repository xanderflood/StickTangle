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

	public int row, col;

	protected void Start() {
		grid = Utils.FindComponent<Grid>("Board");
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
	}

	public IEnumerator move(int dr, int dc) {
		inMotion = true;
		
		grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Empty));
		row += dr;
		col += dc;
		grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Player));
		
		Vector3 to = grid.PosToCoord(row, col, layer);
		
		Vector3 velocity = speed * (to - transform.position).normalized;
		while (transform.position != to) {
			transform.position += velocity;
			yield return null;
		}
		transform.position = to;
		
		inMotion = false;
	}
}
