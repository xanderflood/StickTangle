using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Sticker : Piece {
	
	public AudioClip wallBump;
	public AudioClip magnet;

	public Dictionary<Position, Stickable> stickableMap = new Dictionary<Position, Stickable>();

	private float moveDelay = 0.05f;
	private float lastMoveTime = 0;

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	public bool done = false;

	private LevelManager lm;
	private List<Stickable> stickables = new List<Stickable>();

	private bool teleporting;

	private void Start() {
		if (LevelManager.modeling)
			return;

		lm = Utils.FindComponent<LevelManager>("LevelManager");
	}

    protected override void Awake() {
        base.Awake();
        Color temp = new Color();
        temp.r = 0;
        temp.g = 0;
        temp.b = 0;
        temp.a = .7f;
        this.renderer.material.color = temp;
    }

	private bool isValidSquare(int newR, int newC) {
		if (!grid.InBounds(newR, newC)) {
			return false;
		}

		SquareType type = grid.GetSquare(newR, newC).type;

		return type != SquareType.Block && type != SquareType.Magnet && type != SquareType.Stickable;
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

		if (glowing && stickables.Count > 0) {
			Stickable s = SwapWithStickable();
			if (s.glowing) {
				// TODO: We'll get in an infinite loop
			} else {
				s.StartMagnetGlow();
			}
			StopMagnetGlow();
		}

		bool stuck = false;
		for (int i = stickables.Count - 1; i >= 0; i--) {
			Stickable s = stickables[i];
			if (s.glowing && s.IsStuckToManget(dr, dc)) {
				audio.PlayOneShot(magnet);
				stickableMap.Add(s.pos, s);
				stickables.Remove(s);
				grid.SetSquare(s.pos, new Square(SquareType.Stickable));
				stuck = true;
			}
		}

		if (!isValidMove(dr, dc)) {
			// If we mistakenly unstuck a piece, just add all adjacent stickables again
			// TODO: This is copy and pasted from the Update function
			if (stuck) {
				List<Stickable> toAdd = new List<Stickable>();
				toAdd.AddRange(GetStickables(row, col));
				
				// Check for new stickables next to other pieces
				foreach (Stickable s in stickables) {
					toAdd.AddRange(GetStickables(s.row, s.col));
				}
				stickables.AddRange(toAdd);
			}

			audio.clip = wallBump;

			if (!audio.isPlaying)
				audio.Play();
			return false;
		}
		
		// Start any acid animations
		StartAnimationIfAboutToBeDestroyed(dr, dc);
		foreach (Stickable s in stickables)
			s.StartAnimationIfAboutToBeDestroyed(dr, dc);
		
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
//				audio.PlayOneShot(magnet);
//				stickableMap.Add(s.pos, s);
//				stickables.Remove(s);
//				grid.SetSquare(s.pos, new Square(SquareType.Stickable));
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
		if (LevelManager.modeling)
			return;
		
		// Return if already moving
		if (inMotion || done || teleporting)
			return;

		// 20 millisecond delay after move
		if (Time.time - lastMoveTime > moveDelay) {
			lastMoveTime = Time.time;
		} else {
			return;
		}

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

		CheckForAndAddStickables(false);

		// Check if all goals are covered
		if (grid.CheckAllGoals()) {
			music.clearLevel();
			done = true;
			StartCoroutine(AdvanceLevel());
		}
	}

	private void CheckForAndAddStickables(bool changeColorNow) {

		List<Stickable> toAdd = new List<Stickable>();
		// Check for new stickables next to root piece
		if (!hitAcid) { //if the root piece is being destroyed by acid, don't attach anything to it
			toAdd.AddRange(GetStickables(row, col));
		}
		
		// Check for new stickables next to other pieces
		foreach (Stickable s in stickables) {
			toAdd.AddRange(GetStickables(s.row, s.col));
		}
		newStickables = toAdd;
		
		stickables.AddRange(toAdd);
		DataLogger.Attach(toAdd.Count);

		if (changeColorNow) {
			foreach (Stickable s in newStickables)
				s.renderer.material.color = this.renderer.material.color;

			newStickables.Clear();
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
		Stickable s = SwapWithStickable();
		stickables.Remove(s);
		s.DestroyPiece();
	}

	private Stickable SwapWithStickable() {
		return SwapWithStickable(0);
	}

    private Stickable SwapWithStickable(int index) {
		Utils.Assert(stickables.Count > index);

	    // Swap locations with some stickable, then destroy that stickable
		Stickable s = stickables[index];
		Position tempPos = pos;
		pos = s.pos;
		s.pos = tempPos;

        this.renderer.material = s.renderer.material;
        this.transform.rotation = s.transform.rotation;

		Vector3 tempTransform = transform.position;
		transform.position = s.transform.position;
		s.transform.position = tempTransform;

		// Make sure the magnet glow animation is appropriately tracked
		if (glowing) {
			glowing = false;
			s.glowing = true;
			s.activeGlow = activeGlow;
			activeGlow.transform.position = s.transform.position;
			activeGlow.transform.parent = s.transform;
			activeGlow = null;
		}

		return s;
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
		
		CheckForAndAddStickables(true);
		
		teleporting = false;
		
		yield return false;
	}
	
	private void LiteralTeleport(int rowDelta, int colDelta) {
		Vector3 disp = new Vector3(colDelta, rowDelta, 0);
		
		music.playTeleport();
		
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
  