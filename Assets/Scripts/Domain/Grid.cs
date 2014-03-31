using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Grid : MonoBehaviour {

    [System.Serializable]
    public class Teleporter {
        public List<Position> parts;
        public int xDisp;
        public int yDisp;
        private Grid g;

        public bool justAppeared;

        public Teleporter(List<Position> parts, int xDisp, int yDisp) {
            g = Utils.FindComponent<Grid>("Board");
            g.teleporters.Add(this);
            this.parts = parts;
            this.xDisp = xDisp;
            this.yDisp = yDisp;
        }

        public void AppearAt() {
            justAppeared = true;
            g.deactivated = this;
        }

        public bool ReadyToTeleport() {
            if (justAppeared)
                return false;
            return Fits();
        }

        public bool Fits() {
            if (!parts.Contains(new Position(g.playerBlock.row, g.playerBlock.col)))
                return false;

            foreach (Stickable s in g.playerBlock.AttachedPieces())
            {
                if (!parts.Contains(g.CoordToPos(s.gameObject.transform.position)))
                    return false;
            }

            return true;
        }

        public bool Contains(Position pos) {
            return parts.Contains(pos);
        }
    }

	public const int Dim = 11;

	public Sticker playerBlock;

	// Makes all of the position to coordinate computations work out
	private const float magicConst = (Dim - 1) / 2.0f;

	public List<Position> goals = new List<Position>();
    public List<Acid> acidBlocks = new List<Acid>();

	// This is a storage place for a teleporter that has just been
	// used as a target. It'll be reactivated when the user clears
	private Teleporter deactivated = null;

	public List<Teleporter> teleporters;

	public enum SquareType {
		Player, Stickable, Empty, Block, Acid
	}

	public class Square {
		public SquareType type; 

		public Square(SquareType type) {
			this.type = type;
		}
	}

	

	public enum Direction {
		Up, Down, Left, Right
	}

	// Must be in the same order as the Direction enum above
	private int[] dr = {1, -1, 0, 0};
	private int[] dc = {0, 0, -1, 1};
	
	private Square[,] grid;

	void Awake() {
		grid = new Square[Dim, Dim];
		for (int i = 0; i < Dim; i++) {
			for (int j = 0; j < Dim; j++) {
				SquareType type = SquareType.Empty;
				grid[i, j] = new Square(type);
			}
		}
	}

	void Update() {

		if (deactivated == null)
			return;

		// Otherwise, check if the user is touching any part of that teleporter
		foreach (Stickable s in playerBlock.AttachedPieces()) {
			if (deactivated.parts.Contains(CoordToPos(s.gameObject.transform.position)))
				return;
		}

		if (deactivated.parts.Contains(new Position(playerBlock.row, playerBlock.col)))
			return;

		deactivated.justAppeared = false;
		deactivated = null;
	}

	/**
	 * Checks all of the squares in direction dir for a square of type type. If found, returns a the position.
	 * Otherwise, returns null.
	 */
	public Position FindSquareOfType(Direction dir, int row, int col, SquareType type) {
		int stepC = dc[(int) dir];
		int stepR = dr[(int) dir];

		int i;
		for (i = 1; grid[row + stepR * i, col + stepC * i].type != type; i++) {}

		if (i == 1) {
			return null;
		} else {
			i--;
			return new Position(row + stepR * i, col + stepC * i);
		}
	}

	public Position FindSquareOfType(Direction dir, Position pos, SquareType type) {
		return FindSquareOfType(dir, pos.Row, pos.Col, type);
	}
		
	/**
	 * Prints the grid to the console.
	 */
	public void Print() {
		StringBuilder sb = new StringBuilder();
		for (int i = Dim - 1; i >= 0; i--) {
			for (int j = 0; j < Dim; j++) {
				switch(grid[i, j].type) {
				case SquareType.Empty:
					sb.Append("E");
					break;
				case SquareType.Block:
					sb.Append("B");
					break;
				case SquareType.Player:
					sb.Append("P");
					break;
				case SquareType.Stickable:
					sb.Append("S");
					break;
				}
			}
			sb.AppendLine("");
		}

		Debug.Log(sb.ToString());
	}

	public void SetSquare(Position pos, Square square) {
		SetSquare(pos.Row, pos.Col, square);
	}
	
	public void SetSquare(int row, int col, Square square) {
		BoundsCheck(row, col);

		grid[row, col] = square;
	}

	public Square GetSquare(int row, int col) {
		BoundsCheck(row, col);
		
		return grid[row, col];
	}

	public Pair<SquareType, Position> SquareTypeAt(Direction dir, int row, int col) {
		if (!InBounds(row, col)) {
			return null;
		}

		int stepC = dc[(int) dir];
		int stepR = dr[(int) dir];
		int r = row + stepR;
		int c = col + stepC;

		Position pos = new Position(r, c);
		return new Pair<SquareType, Position>(grid[r, c].type, pos);
	}

	public List<Position> GetStickables(int row, int col) {
		List<Position> result = new List<Position>();
		for (int i = 0; i < dr.Length; i++) {
			int stepR = dr[(int) i]; 
			int stepC = dc[(int) i];
		
			if (grid[row + stepR, col + stepC].type == SquareType.Stickable) {
				result.Add(new Position(row + stepR, col + stepC));
			}
		}

		return result;
	}

	public bool CheckReadyToTeleport(out Teleporter tele) {
		foreach (Teleporter t in teleporters) {
			if (t.ReadyToTeleport()) {
				tele = t;
				return true;
			}
		}

		tele = null;
		return false;
	}

	/**
	 * Returns the teleporter whose list of positions contains this one
	 */
	public Teleporter GetTeleporterAt(Position pos) {
		foreach (Teleporter t in teleporters) {
			if (t.Contains(pos))
				return t;
		}

		Utils.Assert(false);
		return null;		 // C# requires me to return a value :(
	}

	public bool IsEmpty(int row, int col) {
		BoundsCheck(row, col);

		SquareType t = grid[row, col].type;
		return t == SquareType.Empty;
	}

	public Vector3 PosToCoord(int row, int col) {
		return PosToCoord(row, col, -1);
	}

	public Vector3 PosToCoord(int row, int col, int layer) {
		//BoundsCheck(row, col); // TODO

		return new Vector3(-1 * magicConst + col, -1 * magicConst + row, layer);
	}

	public Position CoordToPos(Vector3 coord) {
		return CoordToPos(coord, true);
	}

	public Position CoordToPos(Vector3 coord, bool boundsCheck) {
		// The 0.5f is for rounding to the nearest int
		int row = (int) (coord.y + magicConst + 0.5f);
		int col = (int) (coord.x + magicConst + 0.5f);

		if (boundsCheck) BoundsCheck(row, col); // TODO

		return new Position(row, col);
	}

	private void BoundsCheck(int row, int col) {
		if (!InBounds(row, col)) {
			throw new System.ArgumentOutOfRangeException("Out of bounds: (" + row + ", " + col + ")");
		}
	}

	public bool InBounds(Position pos) {
		return InBounds(pos.Row, pos.Col);
	}

	public bool InBounds(int row, int col) {
		return row < Dim - 1 && col < Dim - 1 && row > 0 && col > 0;
	}

	public bool CheckAllGoals() {
		foreach (Position p in goals) {
			if (grid[p.Row, p.Col].type != SquareType.Player) {
				return false;
			}
		}

		return true;
	}
    // checks to see if given location is acid, if so destroys the acid and returns true
    public bool CheckForAndDestoryAcid(int row, int col) {

        foreach (Acid a in acidBlocks)
        {
            Position p = CoordToPos(a.transform.position);
            if (p.Row == row && p.Col == col)
            {
                acidBlocks.Remove(a);
                Destroy(a.gameObject);
                return true;
            }
        }

        return false;
    }
}
