using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetGlow : MonoBehaviour {
	
	public float rate = 10f;
	public float maxInt = 0.8f;
	public float minInt = 0.6f;

	public MeshRenderer target;

	bool animating = true;

	//public Light light;

	// Use this for initialization
	void Start () {
		StartCoroutine(lightOscillate());
	}

	public void BeginStop () {
		animating = false;
	}
	
	IEnumerator lightOscillate() {

		// Warming up
		float alpha = 0f;
		while (alpha < maxInt) {
			setAlpha(alpha += rate*Time.deltaTime);
			yield return true;
		}

		// Oscillating
		float t = 0;
		while (animating) {
			if (t >= 1)
				t = 0;
				
			setAlpha(0.5f*(maxInt - minInt)*Mathf.Cos(2*Mathf.PI*t) + minInt);
				
			t += rate * Time.deltaTime;
				
			yield return true;
		}
		
		// Cooling down
		while (alpha > 0) {
			setAlpha(alpha -= 7*rate*Time.deltaTime);
			yield return true;
		}

		Destroy(gameObject);
	}

	void setAlpha(float alpha) {
		Color c = target.material.color;
		c.a = alpha;
		target.material.color = c;
		
	}
}
