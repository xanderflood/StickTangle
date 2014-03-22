using UnityEngine;
using System.Collections;

public class MoveableObject : MonoBehaviour {
	private const float speed = 0.1f;

	private int row, col;
	private int layer = -3;

	public IEnumerator move(int dr, int dc) {
		row += dr;
		col -= dc;
		Vector3 to = new Vector3(row, col, layer);
		Vector3 velocity = speed * (to - transform.position).normalized;
		while (transform.position != to) {
			transform.position += velocity;
			yield return null;
		}
		transform.position = to;
	}
}
