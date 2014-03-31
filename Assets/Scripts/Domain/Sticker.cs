using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Sticker : Piece {

	public Material stickerMat;

	public Dictionary<Position, Stickable> stickableMap = new Dictionary<Position, Stickable>();

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	private bool done = false;

	private LevelManager lm;
	private List<Stickable> stickables = new List<Stickable>();
	
	private new void Start() {
		base.Start();
		lm = Utils.GetComponent<LevelManager>(Camera.main.gameObject);

		grid.playerBlock = this;
	}

	private bool isValidSquare(int newR, int newC) {
		if (!grid.InBounds(newR, newC)) {
			return false;
		}

		SquareType type = grid.GetSquare(newR, newC).type;

		return type != SquareType.Block;
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
			return false;
		}

		StartCoroutine(move(dr, dc));

		foreach (Stickable s in stickables) {
			StartCoroutine(s.move(dr, dc));
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
			Teleport(t.rowOther, t.colOther);
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

		// Check if all goals are covered
		if (grid.CheckAllGoals()) {
			done = true;
			StartCoroutine(AdvanceLevel());
		}
	}

    void HandleAcid() {
        //first, deal with stickables colliding with the acid
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
        
        //now deal with sticker
        if (grid.CheckForAndDestoryAcid(row, col)) {
            DestroyAtEndOfMove();// at end of animation, swapWithStickable will be called
        }
    }

    //should only be called at the end of handleMove, if the Sticker hit an Acid Block
    public void swapWithStickable() {
            //if this is the last block, the player looses
            if (stickables.Count == 0) {
                //placeholder functionality
                Debug.Log("Game Over, all blocks destroyed");
                Application.LoadLevel(Application.loadedLevel); //reload the level,
                return;
            }

            //swap locations with some stickable, then destroy that stickable
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
		yield return new WaitForSeconds(0.5f);
		lm.AdvanceLevel();
	}

	private void Teleport(int rowOther, int colOther) {
		int dr = rowOther - row;
		int dc = colOther - col;
		Vector3 disp = new Vector3(dr, dc, 0);

		// Move the main block
		gameObject.transform.position += disp;
		ChangePosition(row + dr, col + dc);

		// Move the other blocks
		foreach (Stickable s in stickables) {
			s.gameObject.transform.position += disp;
			s.ChangePosition(s.row + dr, s.col + dc);
		}

		// Mark the target teleporter, so we don't get immediately returned
		try {
			grid.GetTeleporterAt(new Position(row, col)).AppearAt();
		} catch { }
	}
}
