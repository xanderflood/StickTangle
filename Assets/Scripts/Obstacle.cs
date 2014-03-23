using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Obstacle : MonoBehaviour {
	private void Start() {
		Grid grid = GameObject.Find("Board").GetComponent<Grid>();
		DebugUtils.Assert(grid);
		Position pos = grid.CoordToPos(transform.position, false);
		if (grid.InBounds(pos)) { // TODO
			grid.SetSquare(pos, new Square(SquareType.Block));	
		}
	}
}
