using UnityEngine;
using System.Collections;

public class MagnetGlow : MonoBehaviour {
	
	public float rate = 0.1f;
	public float maxInt = 2.5f;
	public float minInt = 1.5f;

	public Light target;

	//public Light light;

	// Use this for initialization
	void Start () {
		StartCoroutine(lightOscillate());
	}
	

	IEnumerator lightOscillate() {

		while (true) {

			float t = 0;
			while (t < 1) {

				target.intensity = -(maxInt - minInt)*Mathf.Cos(2*Mathf.PI*t) + minInt;
				t += rate * Time.deltaTime;
				yield return true;
			}
		}
	}
}
