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

	public Dictionary<Position, Stickable> stickableMap = new Dictionary<Position, Stickable>();

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	private bool done = false;

	private LevelManager lm;
	private List<Stickable> stickables = new List<Stickable>();
	
	private void Start() {
		lm = Utils.GetComponent<LevelManager>(Camera.main.gameObject);
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

		if (!isValidMove(dr, dc)) {
			audio.clip = wallBump;

			if (!audio.isPlaying)
				audio.Play();
			return false;
		}

		StartCoroutine(Move(dr, dc));

		foreach (Stickable s in stickables) {

			StartCoroutine(s.Move(dr, dc));
		}

		return true;
	}
	
	private void Update() {
		// Return if already moving
		if (inMotion || done)
			return;

		// Check for valid teleporters
		Teleporter t = grid.CheckReadyToTeleport();
		if (t != null) {
			Teleport(t.rowDelta, t.colDelta);
		}

		// Return if no arrow key was pressed
		if (!movePieces()) {
			return;
		}

		// Check for new stickables next to root piece

		List<Stickable> toAdd = GetStickables(row, col);

		// Check for new stickables next to other pieces
		foreach (Stickable s in stickables) {
			toAdd.AddRange(GetStickables(s.row, s.col));
		}

		stickables.AddRange(toAdd);


        HandleAcid();

		HandleMagnets();

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
            DestroyAtEndOfMove();// at end of animation, swapWithStickable will be called
        }
    }

	private void HandleMagnets() {
		// TODO
	}

	protected override void DestroyPiece() {
		hitAcid = false;
		SwapWithStickable();
	}

    public void SwapWithStickable() {
	    // If this is the last block, the player looses
	    if (stickables.Count == 0) {
	        // Placeholder functionality
	        Debug.Log("Game Over, all blocks destroyed");
			lm.Restart();
	        return;
	    }

	    // Swap locations with some stickable, then destroy that stickable
		grid.SetSquare(row, col, new Square(SquareType.Empty));
	    row = stickables[0].row;
	    col = stickables[0].col;
	    transform.position = stickables[0].transform.position;
	    Destroy(stickables[0].gameObject);
	    stickables.RemoveAt(0);
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
			grid.SetSquare(row, col, new Grid.Square(Grid.SquareType.Player));

		}

		return toAdd;
	}

	private IEnumerator AdvanceLevel() {
		yield return new WaitForSeconds(0.75f);
		lm.AdvanceLevel();
	}
	
	private void Teleport(int rowDelta, int colDelta) {
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

		// If there is a teleporter at the target location, mark it as deactivated so we don't get immediately returned
		Teleporter t = grid.GetTeleporterAt(new Position(row, col));
		if (t != null) {
			t.AppearAt();
		}
	}
	
}
