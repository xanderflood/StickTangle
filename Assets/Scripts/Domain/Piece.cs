using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Piece : MonoBehaviour {
	public Position pos = new Position(-1, -1);
    public List<Material> CrayonMats;

	public int R;
	public int row {
		get {
			return pos.Row;
		}
		set {
			pos.Row = value;
			R = value;
		}
	}

	public int C;
	public int col {
		get {
			return pos.Col;
		}
		set {
			pos.Col = value;
			C = value;
		}
	}
	
	public const float speed = 0.1f;
	protected const int layer = -2;
	protected bool inMotion = false;
	protected Grid grid;
	protected MusicSelector music;

	protected List<Stickable> newStickables = new List<Stickable>();

	protected Sticker owner;

	public bool glowing;

	protected bool dissolving;
	
	// Animations
	public GameObject AcidAnimation;
	public GameObject MagnetGlowModel;
	public GameObject activeGlow;

	protected virtual void Awake() {
		if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode) {
			SetColorBlindMaterial();
		} else {
			GetComponent<Renderer>().material = CrayonMats[Random.Range(0, CrayonMats.Count)];
	        transform.Rotate(0,0,Random.Range(0, 3) * 90);
		}

		if (LevelManager.modeling || LevelManager.optionsScreen)
			return;
		grid = Utils.FindComponent<Grid>("Board");
		music = Utils.FindComponent<MusicSelector>("Music");
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;

	}

	protected virtual void SetColorBlindMaterial() {
		// Implemented in subclasses
	}

	public void ChangePosition(int newRow, int newCol) {
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		row = newRow;
		col = newCol;
		grid.SetSquare(row, col, new Square(SquareType.Player));
	}

	public bool IsStuckToManget(int dr, int dc) {
		bool stuckToMagnet = false;
		List<Position> magnets = grid.GetMagnets(row, col);
		foreach (Position p in magnets) {
			Magnet m;
			grid.magnetMap.TryGetValue(p, out m);
			Utils.Assert(m);
			
			if (m.IsMovingAway(row, col, dr, dc)) {
				stuckToMagnet = true;
				break;
			}
		}
		return stuckToMagnet;
	}

	// Checks whether this Piece is about to roll over acid;
	// if so, starts the animation immediately
	public bool StartAnimationIfAboutToBeDestroyed(int dr, int dc) {

		if (grid.SquareTypeAt(Grid.displacementToDirection(dr, dc), row, col).First == SquareType.Acid) {

			StartAcidAnimation(dr, dc);
			dissolving = true;
			return true;
		}

		return false;
	}
	
	public void StartAcidAnimation(int dr, int dc) {
	
		StartAcidAnimation(dr, dc, Utils.FindComponent<Acid>("Acid," + (row + dr) + "," + (col + dc)));
	}

	public void StartAcidAnimation(int dr, int dc, Acid acid) {
		GameObject activeAnim = (GameObject)Instantiate(AcidAnimation,
		                          	gameObject.transform.position,
		                           	gameObject.transform.rotation);
		activeAnim.transform.parent = transform;
		activeAnim.transform.localScale = transform.localScale;
		activeAnim.GetComponent<DissolveAnimation>().owner = this;

		activeAnim.GetComponent<DissolveAnimation>().dr = dr;
		activeAnim.GetComponent<DissolveAnimation>().dc = dc;

		activeAnim.GetComponent<DissolveAnimation>().target = acid;
	}
	
	public void StartMagnetGlow() {
		if (glowing)
			return;

		activeGlow = (GameObject)Instantiate(MagnetGlowModel, transform.position, Quaternion.identity);
		
		Vector3 v = activeGlow.transform.position;
		v.z = -2.1f;
		activeGlow.transform.position = v;

		activeGlow.transform.parent = transform;
		activeGlow.transform.localScale = transform.localScale;

		glowing = true;
	}
	
	public void StopMagnetGlow() {
		if (!glowing)
			return;
		
		activeGlow.GetComponent<MagnetGlow>().BeginStop();
		glowing = false;
	}
}
