using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Sticker : Piece {

	public Material stickerMat;
	public AudioClip clearGoal;
	public AudioClip teleportSound;
	public AudioClip wallBump;
	public AudioClip acid;
	public AudioClip magnet;

	public Dictionary<Position, Stickable> stickableMap = new Dictionary<Position, Stickable>();

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	public bool done = false;

	private LevelManager lm;
	private List<Stickable> stickables = new List<Stickable>();

	private bool teleporting;
	
	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
	}

	private bool isValidSquare(int newR, int newC) {
		if (!grid.InBounds(newR, newC)) {
			return false;
		}

		SquareType type = grid.GetSquare(newR, newC).type;

		return type != SquareType.Block && type != SquareType.Magnet;
	}


	private bool isValidMove(int dr, int dc) {
		int newR = row + dr;
		int newC = col + dc;
		if (!isValidSquare(newR, newC)) {
			return false;
		}

		foreach (Stickable s in stickables) {
			newR = s.row + dr;
			newC = s.col + dc;

			if (!isValidSquare(newR, newC)) {
				return false;
			}
		}

		return true;
	}

	// Returns true if pieces were actually moved
	public bool movePieces() {
		int dr = 0;
		int dc = 0;
		if (Input.GetKey(KeyCode.UpArrow)) {
			dr = 1;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			dr = -1;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			dc = 1;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			dc = -1;
		} else {
			return false;
		}

		// Start any acid animations
		StartAnimationIfAboutToBeDestroyed(dr, dc);
		foreach (Stickable s in stickables)
			s.StartAnimationIfAboutToBeDestroyed(dr, dc);

		if (!isValidMove(dr, dc)) {
			audio.clip = wallBump;

			if (!audio.isPlaying)
				audio.Play();
			return false;
		}
		
		DataLogger.Move(stickables, this, dr, dc);

		if (!IsStuckToManget(dr, dc)) {
			StartCoroutine(Move(dr, dc));
		} else {
			if (stickables.Count > 0) {
				SwapWithStickable();
			}
		}

		for (int i = stickables.Count - 1; i >= 0; i--) {
			Stickable s = stickables[i];
			if (!s.IsStuckToManget(dr, dc)) {
				StartCoroutine(s.Move(dr, dc));
			} else {
				// Detach stickable
				audio.PlayOneShot(magnet);
				stickableMap.Add(s.pos, s);
				stickables.Remove(s);
				grid.SetSquare(s.pos, new Square(SquareType.Stickable));
			}
		}

		// TODO: Eventually remove this function
		PutPlayersInGrid();

		return true;
	}

	// TODO: Eventually remove this function
	private void PutPlayersInGrid() {
		grid.SetSquare(pos, new Square(SquareType.Player));
		foreach (Stickable s in stickables) {
			grid.SetSquare(s.pos, new Square(SquareType.Player));
		}
	}

	private void Update() {
		// Return if already moving
		if (inMotion || done || teleporting)
			return;

		// Check for valid teleporters
		Teleporter t = grid.CheckReadyToTeleport();
		if (t != null) {
			StartCoroutine(Teleport(t.rowDelta, t.colDelta));
			return;
		}

		// Return if no arrow key was pressed
		if (!movePieces()) {
			return;
		}

        //acid must be handled before attaching stickables, 
        //because of the case where an acid block is adjacent to a stickable
        HandleAcid();

        List<Stickable> toAdd = new List<Stickable>();
		// Check for new stickables next to root piece
        if (!hitAcid) { //if the root piece is being destroyed by acid, don't attach anything to it
		   toAdd.AddRange(GetStickables(row, col));
        }

		// Check for new stickables next to other pieces
		foreach (Stickable s in stickables) {
			toAdd.AddRange(GetStickables(s.row, s.col));
		}

		stickables.AddRange(toAdd);
		DataLogger.Attach(toAdd.Count);

		// Check if all goals are covered
		if (grid.CheckAllGoals()) {
			audio.PlayOneShot(clearGoal);
			done = true;
			StartCoroutine(AdvanceLevel());
		}
	}

    private void HandleAcid() {
        // First, deal with stickables colliding with the acid
        List<Stickable> toDestory = new List<Stickable>();
        foreach (Stickable s in stickables) {
            if (grid.CheckForAndDestoryAcid(s.row, s.col)) {
                toDestory.Add(s);
            }
        }

        foreach (Stickable s in toDestory) {
            s.DestroyAtEndOfMove();
            stickables.Remove(s);
        }
        
        // Now deal with sticker
        if (grid.CheckForAndDestoryAcid(row, col)) {
			audio.PlayOneShot(acid);
            DestroyAtEndOfMove();// at end of animation, swapWithStickable will be called
        }
    }

	public override void DestroyPiece() {
		// If this is the last block, the player looses
		if (stickables.Count == 0) {
			// Placeholder functionality
			Debug.Log("Game Over, all blocks destroyed");
			lm.Restart();
			return;
		}

		hitAcid = false;
		SwapWithStickable();
		stickables[0].DestroyPiece();
		stickables.RemoveAt(0);
	}

    public void SwapWithStickable() {
		Utils.Assert(stickables.Count > 0);

	    // Swap locations with some stickable, then destroy that stickable
		Stickable s = stickables[0];
		Position tempPos = pos;
		pos = s.pos;
		s.pos = tempPos;

		Vector3 tempTransform = transform.position;
		transform.position = s.transform.position;
		s.transform.position = tempTransform;
    }

    public List<Stickable> AttachedPieces() {
        return stickables;
    }

	private List<Stickable> GetStickables(int r, int c) {
		List<Stickable> toAdd = new List<Stickable>();
		List<Position> positions = grid.GetStickables(r, c);
		for (int i = 0; i < positions.Count; i++) {
			Stickable s;
			stickableMap.TryGetValue(positions[i], out s);
			Utils.Assert(s);

			toAdd.Add(s);
			s.renderer.material = stickerMat;
			stickableMap.Remove(positions[i]);
			grid.SetSquare(s.row, s.col, new Square(SquareType.Player));
		}

		return toAdd;
	}

	private IEnumerator AdvanceLevel() {
		yield return new WaitForSeconds(0.75f);
		lm.AdvanceLevel();
	}
	private IEnumerator Teleport(int rowDelta, int colDelta) {
		
		teleporting = true;
		const float rate = 0.075f;
		
		// Shrink
		Vector3 sc = gameObject.transform.localScale;
		float scale = 1f;
		while (scale > 0) {
			adjustAllScales(sc*scale);
			scale -= rate;
			yield return true;
		}
		
		LiteralTeleport(rowDelta, colDelta);
		
		// Grow
		while (scale < 1f) {
			adjustAllScales(sc*scale);
			scale += rate;
			yield return true;
		}
		adjustAllScales(sc);
		
		teleporting = false;
		
		yield return false;
	}
	
	private void LiteralTeleport(int rowDelta, int colDelta) {
		Vector3 disp = new Vector3(colDelta, rowDelta, 0);
		
		audio.PlayOneShot (teleportSound);
		
		// Move the main block
		gameObject.transform.position += disp;
		ChangePosition(row + rowDelta, col + colDelta);
		
		// Move the other blocks
		foreach (Stickable s in stickables) {
			s.gameObject.transform.position += disp;
			s.ChangePosition(s.row + rowDelta, s.col + colDelta);
		}

		// TODO: Eventually remove this function
		PutPlayersInGrid();

		// If there is a teleporter at the target location, mark it as deactivated so we don't get immediately returned
		Teleporter t = grid.GetTeleporterAt(new Position(row, col));
		if (t != null) {
			t.AppearAt();
		}
	}

	private void adjustAllScales(Vector3 scale) {
		gameObject.transform.localScale = scale;
		foreach (Stickable st in stickables)
			st.transform.localScale = scale;
	}
}
