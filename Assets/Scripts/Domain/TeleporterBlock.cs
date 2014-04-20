using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleporterBlock : Piece {


    protected override void Awake()
    {
        base.Awake();
        Color temp = new Color();
        temp.r = 230f/255;
        temp.g = 230f/255;
        temp.b = 0f;
        temp.a = 1;
        this.renderer.material.color = temp;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
