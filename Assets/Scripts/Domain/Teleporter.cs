using UnityEngine;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour {
	// So we can't reteleport until we've completely moved off the teleport squares
	public bool justAppeared;
	
	public int rowOther;
	public int colOther;

	private Grid g;
	private List<Position> parts = new List<Position>();
	
	private void Start() {
		// All child objects are assumed to be Teleporter blocks
		foreach (Transform child in transform) {
			Piece piece = Utils.GetComponent<Piece>(child.gameObject);
			Position p = new Position(piece.row, piece.col);
			parts.Add(p);
		}

		g = Utils.FindComponent<Grid>("Board");
		g.teleporters.Add(this);
	}
	
	public void AppearAt() {
		justAppeared = true;
		g.deactivated = this;
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
		if (!parts.Contains(new Position(g.playerBlock.row, g.playerBlock.col))) {
			return false;
		}
		
		foreach (Stickable s in g.playerBlock.AttachedPieces()) {
			if (!parts.Contains(g.CoordToPos(s.gameObject.transform.position))) {
				return false;
			}
		}
		
		return true;
	}
	
	public bool Contains(Position pos) {
		return parts.Contains(pos);
	}
}
