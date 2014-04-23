using UnityEngine;
using System.Collections;

public class ColoredPiece : Piece {

	public Color color;

	// Use this for initialization
	void Awake () {
		base.Awake ();

		this.renderer.material.color = color;
	}
}
