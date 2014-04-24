using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Sticker : MonoBehaviour {
	
	public AudioClip wallBump;
	public AudioClip magnet;

	public Dictionary<Position, Stickable> stickableMap = new Dictionary<Position, Stickable>();

	//public GameObject stickableModel;

	public bool isPLaying = false;
	private float moveDelay = 0.05f;
	private float lastMoveTime = 0;

	// When all the goals are covered, we set done to true which disables movement, allowing us time to transition
	// to the next level
	public bool done = false;

	public bool inMotion;

	Grid grid;

	private LevelManager lm;
	private List<Stickable> stickables = new List<Stickable>();
	public List<Stickable> Stickables { get { return stickables; } }

	private bool teleporting;
	private MusicSelector music;

	private void Start() {
		if (LevelManager.modeling)
			return;

		lm = Utils.FindComponent<LevelManager>("LevelManager");
		DataLogger.Initialize(lm);
		grid = Utils.FindComponent<Grid>("Board");

		music = Utils.FindComponent<MusicSelector>("Music");
	}

    protected void Awake() {
        Color temp = new Color();
        temp.r = 0;
        temp.g = 0;
        temp.b = 0;
        temp.a = .7f;

		if (LevelManager.modeling)
			return;

		Stickable s = Utils.FindComponent<Stickable>("BaseStickable");
		stickables.Add(s);
    }

	private bool isValidSquare(int newR, int newC) {
		if (!grid.InBounds(newR, newC)) {
			return false;
		}

		SquareType type = grid.GetSquare(newR, newC).type;

		return type != SquareType.Block && type != SquareType.Magnet && type != SquareType.Stickable;
	}

	private bool isValidMoveForPieces(int dr, int dc, List<Stickable> ss, List<Stickable> leftBehind) {

		int newR, newC;
		foreach (Stickable s in ss) {
			newR = s.row + dr;
			newC = s.col + dc;

			if (!isValidSquare(newR, newC))
				return false;

			foreach (Stickable obs in leftBehind)
				if (newR == obs.row && newC == obs.col)
					return false;
		}

		return true;
	}

	// Returns true if pieces were actually moved
	public bool movePieces() {
		int dr = 0;
		int dc = 0;
		if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.Joystick1Button5))) {
			dr = 1;
		} else if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.Joystick1Button6))) {
			dr = -1;
		} else if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.Joystick1Button8))) {
			dc = 1;
		} else if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.Joystick1Button7))){
			dc = -1;
		} else {
			return false;
		}
		
		List<Stickable> notLeftBehind = new List<Stickable>();
		List<Stickable> leftBehind = new List<Stickable>();
		foreach (Stickable s in stickables) {
			if (!s.IsStuckToManget(dr, dc))
				notLeftBehind.Add(s);
			else
				leftBehind.Add(s);
		}
		
		if (notLeftBehind.Count == 0)
			return false;

		if (!isValidMoveForPieces(dr, dc, notLeftBehind, leftBehind)) {
			audio.clip = wallBump;
			
			if (!audio.isPlaying)
				audio.Play();
			return false;
		}

		// Only play the magnet sound if the move is valid!
		if (notLeftBehind.Count < stickables.Count){
			if (isPLaying == false)
				StartCoroutine(playMagnet());
		}
		foreach (Stickable s in leftBehind) {
			bool willBeAddedBack = false;
			foreach (Stickable s2 in notLeftBehind) {
				if (Grid.isAdjacent(s.row, s.col, s2.row + dr, s2.col + dc)) {
					willBeAddedBack = true;
					break;
				}
			}

			if (!willBeAddedBack)
				s.Unstick(false);
		}

		// Check who needs to be dissolved, and start their animations
		List<Stickable> notAcided = new List<Stickable>();
		foreach (Stickable s in notLeftBehind)
			if (!s.StartAnimationIfAboutToBeDestroyed(dr, dc))
				notAcided.Add(s);
			else 
				music.playAcid();

		// Make sure we keep adequate track of the glow
		foreach (Stickable s in notLeftBehind) {
			if (grid.IsNextToMagnet(s.row + dr, s.col + dc))
				s.StartMagnetGlow();
			else
				s.StopMagnetGlow();
		}

		// Start actual movement
		DataLogger.Move(stickables, this, dr, dc);
		StartCoroutine(moveThesePieces(dr, dc, notLeftBehind, notAcided));
		
		// Update positions
		foreach (Stickable s in notLeftBehind)
			s.ChangePosition(s.row + dr, s.col + dc);

		// Disown other pieces
		foreach (Stickable s in leftBehind) {
			stickableMap.Add(s.pos, s);
			grid.SetSquare(s.pos, new Square(SquareType.Stickable));
		}

		return true;
	}

	IEnumerator moveThesePieces(int dr, int dc, List<Stickable> toBeMoved, List<Stickable> notAcided) {

		// By the time tie coroutine is called, ALL
		// necessary animations (glow, acid) have been
		// dealt with, and any stickables that haven't

		// Add them as children
		foreach (Stickable s in toBeMoved)
			if (s != null)
				s.transform.parent = transform;

		// Move
		inMotion = true;

		Vector3 to = gameObject.transform.position + new Vector3(dc, dr, 0);
		
		Vector3 velocity = Piece.speed * (to - transform.position).normalized;
		float distanceTravelled = 0;
		while (distanceTravelled < 1f) {
			transform.position += velocity;
			distanceTravelled += velocity.magnitude;
			yield return null;
		}
		transform.position = to;
		
		inMotion = false;

		// TODO: Do I need to destroy things after acid, or does acid animation handle that?

		// Remove them as children
		foreach (Stickable s in toBeMoved)
			s.transform.parent = null;

		// New list!
		stickables = notAcided;

		// Restart if everyone died
		if (stickables.Count == 0)
			lm.Restart();

		// Attach any new Stickables
		CheckForAndAddStickables(true, false);

	}

	// TODO: Eventually remove this function
	private void PutPlayersInGrid() {

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
		if (grid != null) {
			Teleporter t = grid.CheckReadyToTeleport();
			if (t != null) {
				StartCoroutine(Teleport(t.rowDelta, t.colDelta));
				return;
			}
		}

		// Return if no arrow key was pressed
		if (!movePieces()) {
			return;
		}

        // acid must be handled before attaching stickables,
        // because of the case where an acid block is adjacent to a stickable

		// Check if all goals are covered
		if (grid.CheckAllGoals()) {

			music.clearLevel();
			done = true;
			StartCoroutine(AdvanceLevel());
		}
	}

	private void CheckForAndAddStickables(bool changeColorNow, bool checkRoot) {

		List<Stickable> toAdd = new List<Stickable>();
		
		// Check for new stickables next to other pieces
		foreach (Stickable s in stickables) {
			toAdd.AddRange(GetStickables(s.row, s.col));
		}
		
		stickables.AddRange(toAdd);
		DataLogger.Attach(toAdd.Count);

		if (changeColorNow) {
			foreach (Stickable s in toAdd)
				s.Stick(false);

			toAdd.Clear();
		}
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
		
		CheckForAndAddStickables(true, true);
		
		teleporting = false;
		
		yield return false;
	}
	
	private void LiteralTeleport(int rowDelta, int colDelta) {
		Vector3 disp = new Vector3(colDelta, rowDelta, 0);
		
		music.playTeleport();
		
		// Move the main block
		gameObject.transform.position += disp;
		//ChangePosition(row + rowDelta, col + colDelta);
		
		// Move the other blocks
		foreach (Stickable s in stickables) {
			s.gameObject.transform.position += disp;
			s.ChangePosition(s.row + rowDelta, s.col + colDelta);
		}

		// TODO: Eventually remove this function
		PutPlayersInGrid();

		// If there is a teleporter at the target location, mark it as deactivated so we don't get immediately returned
		Teleporter t = grid.GetTeleporterAt(new Position(stickables[0].row, stickables[0].col));
		if (t != null) {
			t.AppearAt();
		}
	}

	private void adjustAllScales(Vector3 scale) {
		gameObject.transform.localScale = scale;
		foreach (Stickable st in stickables)
			st.transform.localScale = scale;
	}

	private IEnumerator playMagnet() {
		isPLaying = true;
		audio.PlayOneShot (magnet);
		yield return new WaitForSeconds(1.00f);
		isPLaying = false;
	}
}
  