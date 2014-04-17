using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Obstacle : MonoBehaviour {
	private void Start() {
		Grid grid = Utils.FindComponent<Grid>("Board");
		Position pos = grid.CoordToPos(transform.position, false);
		grid.SetSquare(pos, new Square(SquareType.Block));	
	}
}
