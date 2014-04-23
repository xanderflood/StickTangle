using UnityEngine;
using System.Collections;

[System.Serializable]
public class DissolveOneSquare : MonoBehaviour {

	public Color target;
	public bool dissolving;
	private bool animating;

	public static float rate = 0.1f;
	
	// Update is called once per frame
	void Update () {
		if (!animating & dissolving)
			StartCoroutine(DissolveCoroutine());
	}

	IEnumerator DissolveCoroutine() {
		animating = true;

		// A bunch of System.Random's all get initialized at around the same
		// time, and the default seed is timestamp. Without some nonsense
		// depending on position, no actual randomness will take place.
		// 13 is prime, so there's no preiodicity
		float randWait = ((100000 * transform.position.x * transform.position.x
		                   + 2000 * transform.position.y + System.DateTime.Now.Second) % 13)*(0.1f/11.0f);

		yield return new WaitForSeconds(randWait);

		float val = 0f;
		Color start = gameObject.GetComponent<MeshRenderer>().material.color;

		Color c;
		while (val <= 1) {
			c = Color.Lerp(target, start, Mathf.Max(1 - 3*val, 0f));
			c.a = 1 - val;

			if (val > .2 && val < .5) {
				Vector3 ls = transform.localScale;
				ls.x *= 0.9f;
				ls.y *= 0.9f;
				transform.localScale = ls;

				//Quaternion q = transform.rotation;
				//q.z += (randWait - 0.05f) * rate * 4f;
				//transform.rotation = q;
			}

			gameObject.GetComponent<MeshRenderer>().material.color = c;

			val += rate;
			yield return true;
		}

		Destroy(this.gameObject);
	}

}
