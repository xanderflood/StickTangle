﻿using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	public string[] messages;
	public int i;

	public bool mainGoal;
	public GameObject ownerObj;
	public GoalManagerScript owner;

	public GameObject display;

	public bool restart;

	public void Start() {
		if (mainGoal)
			display.GetComponent<TextMesh>().text = messages[0];
		else
			owner = ownerObj.GetComponent<GoalManagerScript> ();
	}

	void Update() {

		if (!mainGoal)
			return;

		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);

		if(Input.GetKeyDown(KeyCode.Space)) {
			if (i < messages.Length-1)
				display.GetComponent<TextMesh>().text = messages[++i];
			else
				display.GetComponent<TextMesh>().text = "";
		}
	}

	public void OnTriggerEnter2D(Collider2D other) {

		Stickable stb = other.gameObject.GetComponent<Stickable> ();
		if (!stb)
			return;

		if (mainGoal)
			StartCoroutine (nextLevelCoroutine(stb.owner));
		else
			owner.addActive(stb.owner);
	}

	public void OnTriggerExit2D(Collider2D other) {

		if (!mainGoal)
			owner.deActive();
	}

	IEnumerator nextLevelCoroutine(Sticker st) {
		int wait = st.currentMove;
		while (st.currentMove == wait)
			yield return true;

		if (restart)
			Application.LoadLevel (0);

		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
