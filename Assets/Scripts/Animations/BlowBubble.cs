using UnityEngine;
using System.Collections;

public class BlowBubble : MonoBehaviour {

	public Sprite popped;

	public float rate = 1f;

	// Use this for initialization
	void Start () {
		StartCoroutine(Animate());
	}
	
	// Update is called once per frame
	IEnumerator Animate () {

		// Grow
		float sc = 0f;
		Vector3 orig = gameObject.transform.localScale;
		while (sc < 1f) {
			sc += rate*Time.deltaTime;
			gameObject.transform.localScale = sc*orig;
			yield return true;
		}

		// Pop
		gameObject.GetComponent<SpriteRenderer>().sprite = popped;
		
		while (sc < 1.2f) {
			sc += rate*Time.deltaTime;
			gameObject.transform.localScale = sc*orig;
			yield return true;
		}

		GameObject.Destroy(gameObject);
	}
}
