using UnityEngine;
using System.Collections;

public class DensityVis : MonoBehaviour {

	public GameObject Signal;
	public bool Active;

	// Use this for initialization
	void Start () {

		if (!Active)
			return;

		// Make sure the game isn't really being played
		Utils.FindObject("Player").SetActive(false);
		DataLogger.Active = false;

		// Get the density data
		int[,] densities = DataLogger.densities;

		//And display it
		int dim = Grid.Dim;
		float scale = Mathf.Max(Max(densities), 1f);
		for (int i = 0; i < dim; ++i) {
			for (int j = 0; j < dim; ++j) {
				Vector3 p = Utils.FindComponent<Grid>("Board").PosToCoord(i, j);
				p.z = -2;
				GameObject go = (GameObject)Instantiate(Signal, p, Quaternion.identity);
				MeshRenderer mr = go.GetComponent<MeshRenderer>();
				Color c = mr.material.color;
				c.a *= densities[i, j]/scale;
				mr.material.color = c;
			}
		}
	}

	public static int Max(int[,] arr) {

		int max = -1;
		for (int i = 0; i < Grid.Dim; ++i)
			for (int j = 0; j < Grid.Dim; ++j)
				if (arr[i,j] > max)
					max = arr[i,j];

		return max;
	}
}
