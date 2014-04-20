using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal : MonoBehaviour {


    public List<Material> CrayonMats; 
    void Awake() {

        this.renderer.material = CrayonMats[Random.Range(0, CrayonMats.Count)];
        this.transform.Rotate(0, 0, Random.Range(0, 3) * 90);
        Color temp = new Color();
        temp.r = 0;
        temp.g = .5f;
        temp.b = 0;
        temp.a = 1;
        this.renderer.material.color = temp;
        
    }

	void Start() {
		Grid g = Utils.FindComponent<Grid>("Board");
		g.goals.Add(g.CoordToPos(transform.position));
        g.goalMap.Add(g.CoordToPos(transform.position), this);
	}
}
