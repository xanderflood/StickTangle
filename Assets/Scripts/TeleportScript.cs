using UnityEngine;
using System.Collections.Generic;

public class TeleportScript : MonoBehaviour {

	public int xJump;
	public int yJump;

	void Start() {

		// All child objects are assumed to be Teleporter blocks
		List<Position> pos = new List<Position>();
		foreach (Transform child in gameObject.transform) {

			Position p = new Position(child.gameObject.GetComponent<Piece>().row,
			                          child.gameObject.GetComponent<Piece>().col);

			pos.Add(p);
		}

		// This line DOES do something, since a
		// Teleporter adds itself to a list in
		// the Grid class
		new Grid.Teleporter(pos, xJump, yJump);
	}
}
