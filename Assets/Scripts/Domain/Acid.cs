using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Acid : MonoBehaviour {

    private void Start()
    {
        Grid grid = Utils.FindComponent<Grid>("Board");
        Position pos = grid.CoordToPos(transform.position, false);
        if (grid.InBounds(pos))
        { // TODO
            grid.acidBlocks.Add(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
