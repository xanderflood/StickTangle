using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Stickable : Piece {

    protected override void Awake() {
		base.Awake();

        Color temp = new Color();
        temp.r = 1;
        temp.g = 0.5f;
        temp.b = 0.5f;
        temp.a = 1f;
        this.renderer.material.color = temp;
    }
		
	private void Start() {
		grid.SetSquare(row, col, new Square(SquareType.Stickable));
		
		Sticker s = Utils.FindComponent<Sticker>("Player");
		s.stickableMap.Add(new Position(row, col), this);
	}

	public override void DestroyPiece() {
		gameObject.renderer.enabled = false;
        foreach (Transform child in transform) {
            child.gameObject.renderer.enabled = false;
        }
		StartCoroutine(AdvanceAcid());
	}

	private IEnumerator AdvanceAcid() {
		yield return new WaitForSeconds(1.0f);
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		Destroy(this.gameObject);
	}
	
}
