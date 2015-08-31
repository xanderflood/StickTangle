using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleporterBlock : Piece {

	public Material CBMat;

    protected override void Awake()
    {
        base.Awake();
        Color temp = new Color();
        temp.r = 230f/255;
        temp.g = 230f/255;
        temp.b = 0f;
        temp.a = 1;
        this.GetComponent<Renderer>().material.color = temp;
    }

	// TODO: This is duplicated across all subclasses of Piece
	protected override void SetColorBlindMaterial() {
		GetComponent<Renderer>().material = CBMat;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
