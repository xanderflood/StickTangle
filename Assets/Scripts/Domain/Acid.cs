using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security.Cryptography;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class Acid : MonoBehaviour {

	public GameObject bubbleModel;
    public List<Material> CrayonMats;

    void Awake()
    {
        this.renderer.material = CrayonMats[Random.Range(0, CrayonMats.Count)];
        this.transform.Rotate(0, 0, Random.Range(0, 3) * 90);
        Color temp = new Color();
        temp.r = 0;
        temp.g = .5f;
        temp.b = 0;
        temp.a = 1;
        this.renderer.material.color = temp;
    }

    private void Start() {
		Grid grid = Utils.FindComponent<Grid>("Board");
    	grid.acidBlocks.Add(this);

		StartCoroutine(createBubbles());
    }

	IEnumerator createBubbles() {

		while (true) {
			Debug.Log("bubbling");

			// Select a random wait interval between 1 and 4
			float time = (Random.value % 3f) + 1f;

			// Wait
			yield return new WaitForSeconds(time);

			// Produce a bubble
			Vector3 pos = Random.insideUnitSphere;
			pos.z = -1.1f;

			GameObject bubble = (GameObject)Instantiate(bubbleModel);
			bubble.transform.parent = transform;
			bubble.transform.position = 0.4f*pos + transform.position;
		}
	}
}
