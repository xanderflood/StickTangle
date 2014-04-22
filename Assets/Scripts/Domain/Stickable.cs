using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Stickable : Piece {

	public bool original;

    protected override void Awake() {
		base.Awake();

        Color temp = new Color();

		if (original) {
			temp.r = 0f;
			temp.g = 0f;
			temp.b = 0f;
		} else {
			temp.r = 0.5f;
			temp.g = 0.5f;
			temp.b = 0.5f;
		}

		temp.a = 1f;
        this.renderer.material.color = temp;
    }
		
	private void Start() {
		if (LevelManager.modeling || original)
			return;

		owner = Utils.FindComponent<Sticker>("Player");

		if (!original) {
			grid.SetSquare(row, col, new Square(SquareType.Stickable));
			owner.stickableMap.Add(new Position(row, col), this);
		} else
			grid.SetSquare(row, col, new Square(SquareType.Player));

	}

//	private void Update() {
//		//if (fake && !MeshRenderer.enabled)
//		//	DestroyPiece();
//	}


//	private IEnumerator AdvanceAcid() {
//		yield return new WaitForSeconds(1.0f);
//		grid.SetSquare(row, col, new Square(SquareType.Empty));
//		Destroy(this.gameObject);
//	}
	
}
