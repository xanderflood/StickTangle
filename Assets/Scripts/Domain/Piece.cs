using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Piece : MonoBehaviour {
	public Position pos = new Position(-1, -1);
	
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
	protected bool hitAcid = false;
	protected bool inMotion = false;
	protected Grid grid;
	
	protected List<Stickable> newStickables = new List<Stickable>();

	public Material stickerMat;

	protected bool glowing;
	
	// Animations
	public GameObject AcidAnimation;
	public GameObject MagnetGlowModel;
	GameObject activeGlow;

	protected void Awake() {
		grid = Utils.FindComponent<Grid>("Board");
		Position pos = grid.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
	}

    public void DestroyAtEndOfMove() {
        hitAcid = true;
    }

	public void ChangePosition(int newRow, int newCol) {
		grid.SetSquare(row, col, new Square(SquareType.Empty));
		row = newRow;
		col = newCol;
		// TODO: grid.SetSquare(row, col, new Square(SquareType.Player));
	}

	public virtual void DestroyPiece() {
		Log.error("This function should be abstract but Unity is a piece of shit. Don't use me.");
		Utils.Assert(false);
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

	public IEnumerator Move(int dr, int dc) {
		inMotion = true;

		ChangePosition(row + dr, col + dc);
		
		Vector3 to = grid.PosToCoord(row, col, layer);
		
		Vector3 velocity = speed * (to - transform.position).normalized;
		while (transform.position != to) {
			transform.position += velocity;
			yield return null;
		}
		transform.position = to;
		
		inMotion = false;

        // If we ran into acid, we need to destroy this piece at the end of the animation
        if (hitAcid) {
			DestroyPiece();
        }

		foreach (Stickable s in newStickables) {
			s.renderer.material = stickerMat;
		}
		newStickables.Clear();
	}

	// Checks whether this Piece is about to roll over acid;
	// if so, starts the animation immediately
	public void StartAnimationIfAboutToBeDestroyed(int dr, int dc) {

		Position dest = new Position(row + dr, col + dc);
		foreach (Acid a in grid.acidBlocks) {

			Position pos = grid.CoordToPos(a.transform.position);

			if (pos.Row == dest.Row && pos.Col == dest.Col) {
				StartAcidAnimation(dr, dc);
				return;
			}
		}
	}

	public void StartAcidAnimation(int dr, int dc) {
		GameObject activeAnim = (GameObject)Instantiate(AcidAnimation,
		                          	gameObject.transform.position,
		                           	gameObject.transform.rotation);

		activeAnim.GetComponent<DissolveAnimation>().dr = dr;
		activeAnim.GetComponent<DissolveAnimation>().dc = dc;
	}
	
	public void StartMagnetGlow() {
		activeGlow = (GameObject)Instantiate(MagnetGlowModel, transform.position, Quaternion.identity);
		
		Vector3 v = activeGlow.transform.position;
		v.z = -2.1f;
		activeGlow.transform.position = v;
		
		glowing = true;
	}
	
	public void StopMagnetGlow() {
		if (!glowing)
			return;
		
		Destroy(activeGlow);
		glowing = false;
	}
}
