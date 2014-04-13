using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Stickable : Piece {

	public AudioClip acid;

	private void Start() {
		grid.SetSquare(row, col, new Square(SquareType.Stickable));
		
		Sticker s = Utils.FindComponent<Sticker>("Player");
		s.stickableMap.Add(new Position(row, col), this);
	}

	public override void DestroyPiece() {
		audio.PlayOneShot(acid);
		gameObject.renderer.enabled = false;
		StartCoroutine (AdvanceAcid ());
	}

	private IEnumerator AdvanceAcid() {
		yield return new WaitForSeconds(0.35f);
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		Destroy(this.gameObject);
	}
	
}
