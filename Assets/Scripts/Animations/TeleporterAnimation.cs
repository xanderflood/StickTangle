using UnityEngine;
using System.Collections;

public class TeleporterAnimation : MonoBehaviour {
	
	public GameObject rippleModel;
	public float rate = 1f;

	// Use this for initialization
	void Start () {

		StartCoroutine(Animate());
	}
	
	// Update is called once per frame
	IEnumerator Animate () {

		while (true) {
			yield return new WaitForSeconds(0.9f);//UnityEngine.Random.Range(70, 150)/100f);

			StartCoroutine(AnimateOneRipple());
		}
	}

	// Update is called once per frame
	IEnumerator AnimateOneRipple () {
		
		GameObject ripple = (GameObject)Instantiate(rippleModel);
		ripple.transform.parent = transform;

		SpriteRenderer sr = ripple.GetComponent<SpriteRenderer>();
		Color c = sr.color;
		c.a = 0.6f;
		sr.color = c;

		ripple.transform.parent = transform;
		Vector3 pos = transform.position;
		pos.z = transform.position.z - .1f;
		ripple.transform.position = pos;


		float sc = 0f;
		Vector3 scale = ripple.transform.localScale*transform.localScale.x;
		while (sc < 1f) {
			sc += rate*Time.deltaTime;
			ripple.transform.localScale = sc*scale;

			if (sc > 0.8f) {
				c = sr.color;
				c.a = 0.6f - (sc - 0.8f)/0.4f;
				sr.color = c;
			}

			yield return true;
		}

		GameObject.Destroy(ripple.gameObject);
	}
}
