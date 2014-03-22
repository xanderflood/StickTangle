﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class MoveableObject : MonoBehaviour {
	private const float speed = 0.1f;

	private int row, col;
	private int layer = -2;

	private Grid grid;

	public static Dictionary<Position, MoveableObject> pieces = new Dictionary<Position, MoveableObject>();

	private void Start() {
		grid = GameObject.Find("Board").GetComponent<Grid>();
		DebugUtils.Assert(grid);
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
		grid.SetSquare(row, col, new Square(SquareType.Stickable));
		pieces.Add(pos, this);
	}

	public IEnumerator move(int dr, int dc) {
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
	}
}
