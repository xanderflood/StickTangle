using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetGlow : MonoBehaviour {
	
	public float rate = 10f;
	public float maxInt = 0.8f;
	public float minInt = 0.6f;

	public MeshRenderer target;

	bool oscillating = true;

	// Use this for initialization
	void Start () {
		StartCoroutine(lightOscillate());
	}

	public void BeginStop () {
		oscillating = false;
	}
	
	IEnumerator lightOscillate() {

		// Warming up
		float alpha = 0f;
		while (alpha < 0.5f*(maxInt + minInt) && oscillating) {
			setAlpha(alpha += 0.7f*rate*Time.deltaTime);
			yield return true;
		}

		// Oscillating
		float t = .75f;
		while (oscillating) {
			if (t >= 1)
				t = 0;
				
			setAlpha((0.5f*(maxInt - minInt)*Mathf.Cos(2*Mathf.PI*t) + 0.5f*(maxInt + minInt))
			         *gameObject.transform.parent.gameObject.renderer.material.color.a);
				
			t += rate * Time.deltaTime;
				
			yield return true;
		}

		// Turning off
		while (alpha > 0) {
			setAlpha(alpha -= 4.0f*rate*Time.deltaTime);
			yield return true;
		}

		GameObject.Destroy(gameObject);
	}

	void setAlpha(float alpha) {
		Color c = target.material.color;
		c.a = alpha;
		target.material.color = c;
	}
}
