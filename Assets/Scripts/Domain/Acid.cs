using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			System.Random rand = new System.Random();
			// Select a random wait interval between 0.2 and 1.2 seconds
			float time = (float)((rand.NextDouble() % 1f) + 0.2f);

			// Select a random position
			float x = (float)((rand.NextDouble() % 0.6f) - 0.3f);
			float y = (float)((rand.NextDouble() % 0.6f) - 0.3f);

			// Wait
			yield return new WaitForSeconds(time);

			// Produce a bubble
			GameObject bubble = (GameObject)Instantiate(bubbleModel);
			bubble.transform.parent = transform;
			bubble.transform.position = new Vector3(transform.position.x + x,
			                                        transform.position.y + y, -1.1f);
		}
	}
}
