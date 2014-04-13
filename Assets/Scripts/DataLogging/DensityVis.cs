using UnityEngine;
using System.Collections;

public class DensityVis : MonoBehaviour {

	public GameObject signal;
	MeshRenderer[,] mrs;

	// Use this for initialization
	void Start () {

		// Make sure the game isn't really being played
		Utils.FindObject("Player").SetActive(false);
		DataLogger.Active = false;

		// Get the density data
		int[,] densities = DataLogger.densities;

		//And display it
		int dim = Grid.Dim;
		int scale = Max(densities);
		mrs = new MeshRenderer[dim, dim];
		for (int i = 0; i < dim; ++i) {
			for (int j = 0; j < dim; ++j) {
				signal = (GameObject)Instantiate(signal, new Vector3(j, i, -2), Quaternion.identity);
				mrs[i, j] = signal.GetComponent<MeshRenderer>();
				Color c = mrs[i, j].material.color;
				c.a *= densities[i, j]/scale;
				mrs[i, j].material.color = c;
			}
		}
	}

	public static int Max(int[,] arr) {
		return 0;

	}
}
