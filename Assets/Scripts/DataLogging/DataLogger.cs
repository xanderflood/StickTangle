using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

public static class DataLogger {

	static bool Initialized = false;

	//Maintains a record of each attempt to finish the current level

	static List<Attempt> currentLevel = new List<Attempt>();
	//Maintains a record of the current attempt
	static Attempt currentAttempt = new Attempt();

	static float startTime;

	static int playerID;

	public static void Move() {
		++currentAttempt.Moves;
	}

	public static void Attach(int n) {
		currentAttempt.Attaches += n;
	}

	public static void Restart() {
		currentAttempt.Time = Time.time - startTime;

		currentAttempt.Success = false;
		currentLevel.Add(currentAttempt);
		currentAttempt = new Attempt();
		
		currentAttempt.ResetX = Utils.FindComponent<Sticker>("Player").col;
		currentAttempt.ResetY = Utils.FindComponent<Sticker>("Player").row;
		
		currentAttempt.StartTime = System.DateTime.Now.Ticks;
		startTime = currentAttempt.Time;
	}

	public static void Win() {
		currentAttempt.Time = Time.time - startTime;

		currentAttempt.Success = true;
		currentLevel.Add(currentAttempt);
		currentAttempt = new Attempt();

		currentAttempt.StartTime = System.DateTime.Now.Ticks;
		startTime = currentAttempt.Time;
	}

	// Use this for initialization
	public static void Initialize () {
		if (Initialized)
			return;
		
		Initialized = true;
		
		if (!PlayerPrefs.HasKey("numPlayers"))
			PlayerPrefs.SetInt("numPlayers", 0);
		
		playerID = PlayerPrefs.GetInt("numPlayers");
		PlayerPrefs.SetInt("numPlayers", playerID + 1);

		startTime = Time.time;
		
		currentAttempt.StartTime = System.DateTime.Now.Ticks;
		currentAttempt.Time = Time.time - startTime;
	}

	public static void Save() {

		int i = 0;
		foreach (Attempt att in currentLevel) {

			// play{playerID},{level},{play}
			string key = "play" + playerID + "," + Application.loadedLevel + "," + i;
			string record = att.ToString();
			PlayerPrefs.SetString(key, record);

			++i;
		}

		// numPlays{playerID},{level}
		PlayerPrefs.SetInt("numPlays" + playerID + "," + Application.loadedLevel, i);

		currentLevel = new List<Attempt>();
	}

}
