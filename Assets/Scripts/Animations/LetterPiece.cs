using UnityEngine;
using System.Collections;

public class LetterPiece : Piece {
	
	public Color BGcolor;
	public Color fontColor;
	public string letter;

	public TextMesh tm;

	// Use this for initialization
	protected override void Awake () {

		base.Awake();

		Update();
	}

	void Update() {

		tm.text = letter;
		gameObject.renderer.material.color = BGcolor;
		tm.color = fontColor;
	}
}
