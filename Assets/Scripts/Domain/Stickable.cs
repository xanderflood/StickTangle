using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Stickable : Piece {

	private MusicSelector music;
	
	private void Start() {
		music = Utils.FindComponent<MusicSelector>("Music");
		grid.SetSquare(row, col, new Square(SquareType.Stickable));
		
		Sticker s = Utils.FindComponent<Sticker>("Player");
		s.stickableMap.Add(new Position(row, col), this);
	}

	public override void DestroyPiece() {
		music.playAcid ();
		gameObject.renderer.enabled = false;
		StartCoroutine (AdvanceAcid ());
	}

	private IEnumerator AdvanceAcid() {
		yield return new WaitForSeconds(1.0f);
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		Destroy(this.gameObject);
	}
	
}
