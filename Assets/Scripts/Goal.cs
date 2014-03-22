using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	void Start() {
		Grid g = GameObject.Find("Board").GetComponent<Grid>();
		g.goals.Add(g.CoordToPos(transform.position));
	}
}
