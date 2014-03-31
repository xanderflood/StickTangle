using UnityEngine;
using System.Collections;

public class TeleportPair : MonoBehaviour {
	private void Start() {
		Utils.Assert(transform.childCount == 2);
		Teleporter first = Utils.GetComponent<Teleporter>(transform.GetChild(0).gameObject);
		Teleporter second = Utils.GetComponent<Teleporter>(transform.GetChild(1).gameObject);

		first.rowDelta = second.row - first.row;
		first.colDelta = second.col - first.col;

		second.rowDelta = first.rowDelta * -1;
		second.colDelta = first.colDelta * -1;
	}
}
