using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	void Start() {
		Grid g = Utils.FindComponent<Grid>("Board");
		g.goals.Add(g.CoordToPos(transform.position));
	}
}
