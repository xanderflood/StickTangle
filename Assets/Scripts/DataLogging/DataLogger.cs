using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

public static class DataLogger {

	static bool Initialized = false;
	public static bool Active = true;

	//Maintains a record of each attempt to finish the current level
	static List<Attempt> currentLevel = new List<Attempt>();
	public static int[,] densities;

	//Maintains a record of the current attempt
	static Attempt currentAttempt = new Attempt();

	static float startTime;

	static int playerID;

	public static void Move(List<Stickable> sts, int dr, int dc) {

		// Record that there *was* a move
		++currentAttempt.Moves;

		// Record all positions now occupied
		foreach (Stickable st in sts)
			densities[st.row + dr, st.col + dc] += 1;
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

		densities = new int[Grid.Dim, Grid.Dim];
	}

	// Use this for initialization
	public static void Initialize() {

		currentLevel = new List<Attempt>();
		LoadDensities();
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

		if (!Active)
			return;

		int i = 0;
		foreach (Attempt att in currentLevel)
			att.Save (playerID, i++);

		// numPlays{playerID},{level}
		PlayerPrefs.SetInt("numPlays" + playerID + "," + Application.loadedLevel, i);
		SaveDensities();
	}

	private static void LoadDensities() {

		// zeros
		densities = new int[Grid.Dim, Grid.Dim];

		string key = "dens" + Application.loadedLevel;
		if (!PlayerPrefs.HasKey(key))
			return;

		string[] parts = PlayerPrefs.GetString(key).Split(',');
		int dim = System.Convert.ToInt32(parts[0]);

		if (Grid.Dim != dim)
			return;

		for (int i = 0; i < dim; ++i)
			for (int j = 0; j < dim; ++j)
				densities[i, j] = System.Convert.ToInt32(parts[1 + i*dim + j]);
	}

	private static void SaveDensities() {

		string key = "dens" + Application.loadedLevel;
		string val = Grid.Dim.ToString();

		if (!PlayerPrefs.HasKey(key))
			return;
		
		string[] parts = PlayerPrefs.GetString(key).Split(',');
		int dim = System.Convert.ToInt32(parts[0]);
		
		if (Grid.Dim != dim)
			return;

		for (int i = 0; i < dim; ++i)
			for (int j = 0; j < dim; ++j)
				val += "," + densities[i, j];

		PlayerPrefs.SetString(key, val);
	}

}
