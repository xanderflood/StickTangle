using UnityEngine;
using System.Collections;

public class DataViewScript : MonoBehaviour {

	public GameObject numEntries;
	public GameObject listing;

	int nLevels = 10;

	public int playerID;
	public int levelID;
	public int playID;
	
	private Vector2 scrollViewVector1 = Vector2.zero;
	private Vector2 scrollViewVector2 = Vector2.zero;
	private Vector2 scrollViewVector3 = Vector2.zero;
	void OnGUI () {

		//
		// Clear data button
		//
		if (GUI.Button (new Rect(20,40,80,20), "Clear data")) {
			PlayerPrefs.DeleteAll();
		}

		//
		// Players list
		//
		GUI.Label(new Rect(20, 70, 120, 90), "Players");

		int nPlayers = PlayerPrefs.GetInt("numPlayers");

		scrollViewVector1 = GUI.BeginScrollView (new Rect (25, 95, 120, 400),
		                       scrollViewVector1, new Rect (0, 0, 100, 20*nPlayers));

		string[] playerIds = new string[nPlayers];
		for (int i = 0; i < nPlayers; ++i)
			playerIds[i] = i.ToString();

		playerID = GUI.SelectionGrid(new Rect(0, 0, 100, 20*nPlayers), playerID, playerIds, 1);

		GUI.EndScrollView();

		//
		// Levels list
		//
		GUI.Label(new Rect(165, 70, 305, 90), "Levels");
		
		scrollViewVector2 = GUI.BeginScrollView (new Rect (165, 95, 120, 400),
		                       scrollViewVector2, new Rect (0, 0, 100, 20*nLevels));
		
		string[] levelNums = new string[nLevels];
		for (int i = 0; i < nLevels; ++i)
			levelNums[i] = i.ToString();
		
		levelID = GUI.SelectionGrid(new Rect(0, 0, 100, 20*nLevels), levelID, levelNums, 1);
		int realLevelID = levelID;
		
		GUI.EndScrollView();

		//
		// Plays list
		//
		GUI.Label(new Rect(300, 70, 400, 90), "Plays");
		
		int nPlays = PlayerPrefs.GetInt("numPlays" + playerID + "," + realLevelID);
		
		scrollViewVector3 = GUI.BeginScrollView (new Rect (305, 95, 1100, 400),
		                       scrollViewVector3, new Rect (0, 0, 500, 20*nPlays));
		
		string[] playIds = new string[(nPlays + 1)*7];

		// column headings
		playIds[0] = "Index";
		playIds[1] = "Moves";
		playIds[2] = "Attaches";
		playIds[3] = "Time";
		playIds[4] = "Success";
		playIds[5] = "ResetX";
		playIds[6] = "ResetY";

		// plays
		for (int i = 0; i < nPlays; ++i) {

			string str = PlayerPrefs.GetString("play" + playerID + "," + realLevelID + "," + i);
			Attempt att = new Attempt(str);

			playIds[7*(i+1) + 0] = i.ToString();
			playIds[7*(i+1) + 1] = att.Moves.ToString();
			playIds[7*(i+1) + 2] = att.Attaches.ToString();
			playIds[7*(i+1) + 3] = att.Time.ToString();
			playIds[7*(i+1) + 4] = att.Success.ToString();
			playIds[7*(i+1) + 5] = att.ResetX.ToString();
			playIds[7*(i+1) + 6] = att.ResetY.ToString();
		}
		
		GUI.SelectionGrid(new Rect(0, 0, 500, 20*(nPlays + 1)), playID, playIds, 7);

		//Date/time list
		string[] times = new string[nPlays + 1];
		times[0] = "Time";
		for (int i = 0; i < nPlays; ++i) {
			
			string str = PlayerPrefs.GetString("play" + playerID + "," + realLevelID + "," + i);
			Attempt att = new Attempt(str);
			
			times[i+1] = (att.StartTime < 0 ? "N/A"
			         : new System.DateTime(att.StartTime).ToShortDateString() + " "
			              + new System.DateTime(att.StartTime).ToShortTimeString() + " ");
		}

		GUI.SelectionGrid(new Rect(505, 0, 150, 20*(nPlays + 1)), playID, times, 1);

		GUI.EndScrollView();

	}
}
