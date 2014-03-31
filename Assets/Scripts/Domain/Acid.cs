using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Acid : MonoBehaviour {
    private void Start() {
    	grid.acidBlocks.Add(this);
    }
}
