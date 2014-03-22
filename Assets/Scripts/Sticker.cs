using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sticker : Stickable {

	private const float speed = 0.1f;
	private const int layer = -3;

	public int row, col;
	public bool inMotion;

	public bool[] obs;

	public bool[] waitingForGlobally;

	public int currentMove = 0;

	public bool addNew;
	public List<Stickable> toAdd;

	// This guarantees that the Sticker won't accept keyboard input
	// until after all adds from the previous cycle are registered.
	public bool _waitingForSomething;
	public bool waitingForSomething {
		get {
			return _waitingForSomething;
		}
		set {
			_waitingForSomething = value;
			if (value) moreFrames = moreFramesNUM;
		}
	}
	const int moreFramesNUM = 1;
	public int moreFrames;

	void Start() {
		owner = this;
		stuck = true;
		row = (int) Mathf.Round(transform.position.x);  
		col = (int) Mathf.Round(transform.position.y);

		obs = new bool[4];
		waitingForGlobally = new bool[4];
	}
	
	void Update () {
		if (inMotion || waitingForSomething)
			return;

		if (moreFrames != 0) {
			--moreFrames;
			return;
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
			col += 1;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			col -= 1;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			row += 1;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			row -= 1;
		}
		StartCoroutine(move(new Vector3(row, col, layer)));
	}

	public void StickTo(Stickable st) {
		st.owner = this;
		toAdd.Add (st);
	}

	IEnumerator moveCoroutine() {
		inMotion = true;

		Vector3 disp = new Vector3(row, col, 0) - transform.position;
		float goal = disp.magnitude;
		disp = disp * (speed / goal);

		Vector3 soFar = new Vector3 ();

		while (soFar.magnitude < goal) {
			transform.position += disp;
			soFar += disp;
			yield return true;
		}

		transform.position = new Vector3 (row, col, 0);

		foreach (Stickable stb in toAdd) {
			stb.transform.parent = this.transform;
			stb.owner = this;
			stb.stick ();
		}

		toAdd.Clear ();

		inMotion = false;

		++currentMove;
	}

	private IEnumerator move(Vector3 to) {
		inMotion = true;
		
		Vector3 velocity = speed * (to - transform.position).normalized;
		while (transform.position != to) {
			transform.position += velocity;
			yield return null;
		}
		transform.position = to;
		
		inMotion = false;
	}
}
