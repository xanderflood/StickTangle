using UnityEngine;
using System.Collections;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Stickable : Piece {

	public bool original;
	public Material CBMatStuck;
	public Material CBMatUnstuck;

	bool stuck;

    protected override void Awake() {

		base.Awake();
		
		if (original) {
			Stick(true);
		} else {
			Unstick(true);
		}
    }

	// TODO: This is duplicated across all subclasses of Piece
	protected override void SetColorBlindMaterial() {
		if (stuck) {
			renderer.material = CBMatStuck;
		} else
			renderer.material = CBMatUnstuck;
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
	
	public void Unstick(bool now) {

		stuck = false;
		
		Color temp = new Color();
		
		temp.r = 0.5f;
		temp.g = 0.5f;
		temp.b = 0.5f;
		temp.a = 1f;

		if (now)
			renderer.material.color = temp;
		else
			StartCoroutine(ColorFade(temp));
		
		if (!LevelManager.modeling && grid.IsNextToMagnet(row, col))
			StopMagnetGlow();

		if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode)
			SetColorBlindMaterial();
	}

	public void Stick(bool now) {

		stuck = true;
		
		Color temp = new Color();
		
		temp.r = 0f;
		temp.g = 0f;
		temp.b = 0f;
		temp.a = 1f;
		
		if (now)
			renderer.material.color = temp;
		else
			StartCoroutine(ColorFade(temp));

		if (!LevelManager.modeling && grid.IsNextToMagnet(row, col))
			StartMagnetGlow();
		
		if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode)
			SetColorBlindMaterial();
	}

	IEnumerator ColorFade(Color dest) {

		float t = 0f;
		Color init = renderer.material.color;
		while (t < 1f) {

			t += 3f*Time.deltaTime;
			renderer.material.color = ColorInterp(dest, init, t);

			yield return true;
		}

		renderer.material.color = dest;
	}

	Color ColorInterp(Color a, Color b, float t) {
		Color c = new Color();
		c.r = t * a.r + (1f - t) * b.r;
		c.g = t * a.g + (1f - t) * b.g;
		c.b = t * a.b + (1f - t) * b.b;
		c.a = t * a.a + (1f - t) * b.a;
		return c;
	}
}
