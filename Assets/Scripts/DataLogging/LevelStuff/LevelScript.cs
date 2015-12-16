using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {

	public string filename; //to be set BEFORE instantiating a new LevelScript
	
	public GameObject player;
	public GameObject goalPrefab;
	public GameObject obstaclePrefab;
	public GameObject stickablePrefab; //loadfromname instead?

	// Use this for initialization
	void Awake () {

		LevelSkeleton ls = new LevelSkeleton (filename);

		GameObject t = (GameObject)Instantiate (player, new Vector2 (ls.start.First, ls.start.Second), Quaternion.identity);
		t.name = player.name;

		foreach (Pair<int,int> pos in ls.obstacles) {
			t = (GameObject)Instantiate(obstaclePrefab, new Vector2(pos.First, pos.Second), Quaternion.identity);
			t.name = obstaclePrefab.name;
		}
		
		foreach (Pair<int,int> pos in ls.stickables) {
			t = (GameObject)Instantiate(stickablePrefab, new Vector2(pos.First, pos.Second), Quaternion.identity);
			t.name = stickablePrefab.name;
		}
		
		foreach (Pair<int,int> pos in ls.goals) {
			t = (GameObject)Instantiate(goalPrefab, new Vector2(pos.First, pos.Second), Quaternion.identity);
			t.name = goalPrefab.name;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadLevel(LevelSkeleton ls) {

		foreach (Pair<int,int> obPos in ls.obstacles) {
			Instantiate(obstaclePrefab, new Vector2(obPos.First, obPos.Second), Quaternion.identity);
		}
	}
}
