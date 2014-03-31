using UnityEngine;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour {
	// So we can't reteleport until we've completely moved off the teleport squares
	public bool justAppeared;

	public int row;
	public int col;

	public int rowDelta;
	public int colDelta;

	private Grid g;
	private Sticker playerBlock;
	private List<Position> parts = new List<Position>();

	private void Awake() {
		g = Utils.FindComponent<Grid>("Board");
		g.teleporters.Add(this);

		playerBlock = Utils.FindComponent<Sticker>("Player");

		Position pos = g.CoordToPos(transform.position);
		row = pos.Row;
		col = pos.Col;
	}

	private void Start() {
		// All child objects are assumed to be Teleporter blocks
		foreach (Transform child in transform) {
			Utils.Assert(child.name == "TeleportBlock");
			Piece piece = Utils.GetComponent<Piece>(child.gameObject);
			Position p = new Position(piece.row, piece.col);
			parts.Add(p);
		}
	}

	private void Update() {
		if (justAppeared == false) {
			return;
		}

		// Check if player block is in the teleporter
		if (Contains(new Position(playerBlock.row, playerBlock.col))) {
			return;
		}

		// Otherwise, check if the user is touching any part the teleporter
		foreach (Stickable s in playerBlock.AttachedPieces()) {
			if (Contains(new Position(s.row, s.col))) {
				return;
			}
		}
		
		justAppeared = false;
	}
	
	public void AppearAt() {
		justAppeared = true;
	}
	
	public bool ReadyToTeleport() {
		if (justAppeared) {
			return false;
		}
		return Fits();
	}
	
	/**
	* Check if the entire player and all of the stickables fit on the teleporter squares
	*/
	public bool Fits() {
		if (!parts.Contains(new Position(playerBlock.row, playerBlock.col))) {
			return false;
		}
		foreach (Stickable s in playerBlock.AttachedPieces()) {
			if (!parts.Contains(new Position(s.row, s.col))) {
				return false;
			}
		}
		
		return true;
	}
	
	public bool Contains(Position pos) {
		return parts.Contains(pos);
	}
}
