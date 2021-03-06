using UnityEngine;
using System.Collections;

using SquareType = Grid.SquareType;

public class Stickable : Piece {

	public GameObject bg;

	public bool original;
	public Material CBMatStuck;
	public Material CBMatUnstuck;

	public float stickingState; //between 0 and 1

	bool stuck;

    protected override void Awake() {

		base.Awake();
		
		if (original) {
			Stick(true);
		} else { 
			Unstick(true);
		}
    }

	public void Update() {
		Color c = new Color (68f/255f,
		                     (20f + stickingState*180f)/255f,
		                     255f);

		GetComponent<Renderer>().material.color = c;
	}

    // TODO: This is duplicated across all subclasses of Piece
	protected override void SetColorBlindMaterial() {
		if (stuck) {
			GetComponent<Renderer>().material = CBMatStuck;
		} else
			GetComponent<Renderer>().material = CBMatUnstuck;
	}
	
	private void Start() {
		if (LevelManager.modeling || original)
			return;

		owner = Utils.FindComponent<Sticker>("Player");

		if (!original) {
			grid.SetSquare(row, col, SquareType.Stickable);
			owner.stickableMap.Add(new Position(row, col), this);
		} else
			grid.SetSquare(row, col, SquareType.Player);

	}
	
	public void Unstick(bool now) {

		/*stuck = false;
		
		Color temp = new Color();
		
		temp.r = 0.5f;
		temp.g = 0.5f;
		temp.b = 0.5f;
		temp.a = 1f;*/

		/*if (now)
			GetComponent<Renderer>().material.color = temp;
		else
			StartCoroutine(ColorFade(temp));*/
		
		if (!LevelManager.modeling && grid.IsNextToMagnet(row, col))
			StopMagnetGlow();

		if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode)
			SetColorBlindMaterial();
	}

	public void Stick(bool now) {

		/*stuck = true;
		
		Color temp = new Color();
		
		temp.r = 0f;
		temp.g = 0f;
		temp.b = 0f;
		temp.a = 1f;*/
		
		/*if (now)
			GetComponent<Renderer>().material.color = temp;
		else
			StartCoroutine(ColorFade(temp));*/

		if (!LevelManager.modeling && grid.IsNextToMagnet(row, col))
			StartMagnetGlow();
		
		if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode)
			SetColorBlindMaterial();
	}

	IEnumerator ColorFade(Color dest) {

		float t = 0f;
		Color init = GetComponent<Renderer>().material.color;
		while (t < 1f) {

			t += 3f*Time.deltaTime;
			GetComponent<Renderer>().material.color = ColorInterp(dest, init, t);

			yield return true;
		}

		GetComponent<Renderer>().material.color = dest;
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
