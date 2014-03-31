using UnityEngine;
using System.Collections;

public class TeleportPair : MonoBehaviour {
	private void Start() {
		Utils.Assert(transform.childCount == 2);
		Teleporter first = Utils.GetComponent<Teleporter>(transform.GetChild(0).gameObject);
		Teleporter second = Utils.GetComponent<Teleporter>(transform.GetChild(1).gameObject);

		first.rowOther = second.row;
		first.colOther = second.col;

		second.rowOther = first.row;
		second.colOther = first.col;
	}
}
