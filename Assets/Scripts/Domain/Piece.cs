using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Piece : MonoBehaviour {
	public Position pos = new Position(-1, -1);
    public List<Material> CrayonMats;
	public int row {
		get {
			return pos.Row;
		}
		set {
			pos.Row = value;
		}
	}

	public int col {
		get {
			return pos.Col;
		}
		set {
			pos.Col = value;
		}
	}
	
	public const float speed = 0.1f;
	protected const int layer = -2;
//	protected bool hitAcid = false;
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
		this.renderer.material = CrayonMats[Random.Range(0, CrayonMats.Count)];
        this.transform.Rotate(0,0,Random.Range(0, 3) * 90);

		if (LevelManager.modeling)
			return;
		grid = Utils.FindComponent<Grid>("Board");
		music = Utils.FindComponent<MusicSelector>("Music");
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;

	}

//    public void DestroyAtEndOfMove() {
//        hitAcid = true;
//    }

	public void ChangePosition(int newRow, int newCol) {
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		row = newRow;
		col = newCol;
		grid.SetSquare(row, col, new Square(SquareType.Player));
		// TODO: grid.SetSquare(row, col, new Square(SquareType.Player));
	}

	public void DestroyPiece() {
		
		//owner.Stickables.Remove((Stickable)this);
		
		//		gameObject.renderer.enabled = false;
		//        foreach (Transform child in transform) {
		//            child.gameObject.renderer.enabled = false;
		//        }
		
		Destroy(this.gameObject);
		
		//StartCoroutine(AdvanceAcid());
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

//	public IEnumerator Move(int dr, int dc) {
//		inMotion = true;
//
//		ChangePosition(row + dr, col + dc);
//
//		// If we won't be next to a magnet next time, then stop the glow animation
//		if (grid.IsNextToMagnet(row, col))
//			StartMagnetGlow();
//		else
//			StopMagnetGlow();
//		
//		Vector3 to = grid.PosToCoord(row, col, layer);
//		
//		Vector3 velocity = speed * (to - transform.position).normalized;
//		float distanceTravelled = 0;
//		while (distanceTravelled < 1f) {
//			transform.position += velocity;
//			distanceTravelled += velocity.magnitude;
//			yield return null;
//		}
//		transform.position = to;
//		
//		inMotion = false;
//
//        // If we ran into acid, we need to destroy this piece at the end of the animation
//        if (hitAcid) {
//			music.playAcid();
//			DestroyPiece();
//        }
//
//		foreach (Stickable s in newStickables) {
//            //change the color of the stickable to match sticker color
//            s.renderer.material.color = this.renderer.material.color;
//		}
//
//		newStickables.Clear();
//	}

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
		GameObject activeAnim = (GameObject)Instantiate(AcidAnimation,
		                          	gameObject.transform.position,
		                           	gameObject.transform.rotation);
		activeAnim.transform.parent = transform;
		activeAnim.GetComponent<DissolveAnimation>().owner = this;

		activeAnim.GetComponent<DissolveAnimation>().dr = dr;
		activeAnim.GetComponent<DissolveAnimation>().dc = dc;

		Acid acid = Utils.FindComponent<Acid> ("Acid," + (row + dr) + "," + (col + dc));
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

		glowing = true;
	}
	
	public void StopMagnetGlow() {
		if (!glowing)
			return;
		
		activeGlow.GetComponent<MagnetGlow>().BeginStop();
		glowing = false;
	}
}
