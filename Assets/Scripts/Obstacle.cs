using UnityEngine;
using System.Collections;

public enum Direction {
	Up = 0,
	Down = 1,
	Left = 2,
	Right = 3
}

public class Obstacle : MonoBehaviour {

	public float idealX, idealY;
	public bool exited = false;
	public bool entered = false;

	void Start () {
		idealX = transform.position.x;
		idealY = transform.position.y;
	}

	void Update () {

		transform.position = new Vector3 (idealX, idealY, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {

		Stickable stb = other.gameObject.GetComponent<Stickable> ();
		if (!stb || !stb.stuck)
			return;

		if (!(other is BoxCollider2D))
			return;

		Direction dir = getType ((BoxCollider2D)other);

		if (stb.waitingForObstacleAdd[(int)dir])
			return;

		stb.waitingForObstacleAdd[(int)dir] = true;
		stb.owner.waitingForSomething = true;

		StartCoroutine (enterCoroutine (stb, dir));
	}

	IEnumerator enterCoroutine(Stickable stb, Direction dir) {

		while (stb.owner.inMotion) {
			// If exited every gets set true, break immediately
			// so as not to corrupt the values.
			yield return !exited;
		}

		stb.childObs [(int)dir] = this;
		stb.owner.obs [(int)dir] = true;

		stb.waitingForObstacleAdd[(int)dir] = false;
		stb.owner.waitingForSomething = false;
	}

	void OnTriggerExit2D (Collider2D other) {

		Stickable stb = other.gameObject.GetComponent<Stickable> ();
		if (!stb || !stb.stuck)
			return;
		
		if (!(other is BoxCollider2D))
			return;

		Direction dir = getType ((BoxCollider2D)other);

		if (stb.childObs [(int)dir] == this)
			stb.childObs [(int)dir] = null;
		
		if (!stb.owner.waitingForGlobally [(int)dir])
			stb.owner.obs [(int)dir] = false;
	}

	public static Direction getType(BoxCollider2D c) {
		if (c.center.x > .1)
			return Direction.Right;
		if (c.center.x < -.1)
			return Direction.Left;
		if (c.center.y > .1)
			return Direction.Up;
		if (c.center.y < -.1)
			return Direction.Down;
		return 0;
	}

}
