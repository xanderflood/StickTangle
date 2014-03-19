using UnityEngine;
using System.Collections;

public class GoalManagerScript : GoalScript {

	public int numGoals;
	public int numActive;

	public bool nextLevelCorouActive;

	new public void Start() {

		mainGoal = true;
		
		display.GetComponent<TextMesh>().text = messages[0];
	}

	public void addActive(Sticker st) {
		numActive++;
		
		StartCoroutine (nextLevelCoroutine(st));
	}
	
	public void deActive() {

		StartCoroutine (deActivateCoroutine ());
	}
	
	public IEnumerator nextLevelCoroutine(Sticker st) {
		if (nextLevelCorouActive)
			yield return false;
		
		nextLevelCorouActive = true;

		int wait = st.currentMove;
		while (st.currentMove == wait)
			yield return true;
		
		if (numActive == 4*numGoals)
			Application.LoadLevel (Application.loadedLevel + 1);


		nextLevelCorouActive = false;
	}
	
	public IEnumerator deActivateCoroutine() {

		numActive--;

		yield return false;
	}
}
