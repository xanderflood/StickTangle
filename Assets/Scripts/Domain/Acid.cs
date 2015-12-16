using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security.Cryptography;

using SquareType = Grid.SquareType;

public class Acid : MonoBehaviour {

	public GameObject bubbleModel;
    public List<Material> CrayonMats;
	public Material CBMat;

	int row;
	int col;

    void Awake()
    {
		if (Utils.FindComponent<LevelManager>("LevelManager").colorblindMode) {
			GetComponent<Renderer>().material = CBMat;
		} else {
	        this.GetComponent<Renderer>().material = CrayonMats[Random.Range(0, CrayonMats.Count)];
	        this.transform.Rotate(0, 0, Random.Range(0, 3) * 90);
		}

        Color temp = new Color();
        temp.r = 0;
        temp.g = .5f;
        temp.b = 0;
        temp.a = 1;
        this.GetComponent<Renderer>().material.color = temp;
    }

    private void Start() {

		if (!LevelManager.modeling) {
			Grid grid = Utils.FindComponent<Grid>("Board");
			Position pos = grid.CoordToPos(transform.position);
			grid.SetSquare(pos, SquareType.Acid);
    		grid.acidBlocks.Add(this);
			
			row = pos.Row;
			col = pos.Col;
			name = "Acid," + row + "," + col;
		}

		StartCoroutine(createBubbles());

    }

	IEnumerator createBubbles() {

		while (true) {

			// Select a random wait interval between 1 and 4
			float time = (Random.value % 1f) + 0.5f;

			// Wait
			yield return new WaitForSeconds(time);

			// Produce a bubble
			Vector3 pos = Random.insideUnitSphere*gameObject.transform.localScale.x;
			pos.z = -1.1f;

			GameObject bubble = (GameObject)Instantiate(bubbleModel);
			bubble.transform.parent = transform;
			bubble.transform.position = 0.4f*pos + transform.position;
			bubble.transform.localScale *= Mathf.Sqrt(gameObject.transform.localScale.x);
		}
	}
}
