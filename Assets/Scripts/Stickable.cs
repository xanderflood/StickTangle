using UnityEngine;
using System.Collections.Generic;

public class Stickable : MonoBehaviour {

	public Sticker owner;
	
	public Material stuckMat;
	public GameObject quad;

	public Obstacle[] childObs = new Obstacle[4];

	public bool stuck;

	public bool adding;
	public bool[] waitingForObstacleAdd;
	public bool[] waitingForObstacleRemove;

	public List<Stickable> neighbors;

	public void OnTriggerStay2D(Collider2D other) {
		//Debug.Log ("good!");
	}

	public void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.GetComponent<Stickable>()) {
			//Debug.Log ("good!");
		}

		if (!stuck && !adding) {

			Sticker st = other.gameObject.GetComponent<Sticker> ();
			if (st) {
				adding = true;
				st.StickTo (this);
				return;
			}

			Stickable stb = other.gameObject.GetComponent<Stickable> ();
			if (stb) {
				adding = true;
				stb.owner.StickTo (this);
				return;
			}
		}
	}
	
	public void stick() {
		quad.renderer.material = stuckMat;

		Collider2D[] colls = GetComponents<Collider2D> ();
		colls [0].isTrigger = false;
		colls [1].isTrigger = false;
		colls [2].isTrigger = false;
		colls [3].isTrigger = false;
		stuck = true;
		adding = false;
	}
}
