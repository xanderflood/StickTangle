using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;

public class Obstacle : MonoBehaviour {

    public List<Material> CrayonMats;
	public Material CBMat;

    void Awake()
    {
		/*if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode) {
			GetComponent<Renderer>().material = CBMat;
		} else {
	        this.GetComponent<Renderer>().material = CrayonMats[Random.Range(0, CrayonMats.Count)];
	        this.transform.Rotate(0, 0, Random.Range(0, 3) * 90);
		}
        Color temp = new Color();
        temp.r = .1f;
        temp.g = .2f;
        temp.b = .8f;
        temp.a = 1;
        this.GetComponent<Renderer>().material.color = temp;
        */

    }

	private void Start() {

		if (LevelManager.modeling)
			return;

		Grid grid = Utils.FindComponent<Grid>("Board");
		Position pos = grid.CoordToPos(transform.position, false);
		grid.SetSquare(pos, SquareType.Block);	
	}
}
