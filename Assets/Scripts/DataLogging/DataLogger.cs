using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

using LevelState = XmlLoader.LevelState;

public static class DataLogger {

	//The -1 is so that zero always remaind open.
	//This way, play on levels that aren't listed
	//in build settings can still be found!

	static bool Initialized = false;
	public static bool Active = true;

	static LevelState ls;
	static string levelName;

	//Maintains a record of each attempt to finish the current level
	static List<Attempt> currentLevel = new List<Attempt>();
	public static int[,] densities = new int[Grid.Dim, Grid.Dim];

	//Maintains a record of the current attempt
	static Attempt currentAttempt = new Attempt();

	static float startTime;

	static int playerID;

	public static void Move(List<Stickable> sts, Sticker sticker, int dr, int dc) {

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

		currentAttempt.Success = false;
		currentAttempt.ResetX = 0;//Utils.FindComponent<Sticker>("Player").col;
		currentAttempt.ResetY = 0;//Utils.FindComponent<Sticker>("Player").row;

		RecordAttempt();
	}

	public static void Win() {

		currentAttempt.Success = true;

		RecordAttempt();
		Save();
	}

	public static void RecordAttempt() {
		currentAttempt.Time = Time.time - startTime;

		currentLevel.Add(currentAttempt);
		currentAttempt = new Attempt();
		
		currentAttempt.StartTime = System.DateTime.Now.Ticks;
		startTime = currentAttempt.Time;
	}

	// Use this for initialization
	public static void Initialize(LevelManager lm) {

		if((ls == null) || (ls.stage != lm.GetLevelState().stage)
		   		 || (ls.level != lm.GetLevelState().level)) {
			currentLevel = new List<Attempt>();
			ls = lm.GetLevelState();
			levelName = ls.stage + "," + ls.level;

			LoadDensities();
		}

		DensityVis dv = Utils.FindComponent<DensityVis> ("Density Visualizer");

		if (Initialized || dv.Active)
			return;
		Initialized = true;
		
		if (!PlayerPrefs.HasKey("numPlayers"))
			PlayerPrefs.SetInt("numPlayers", 0);

		playerID = PlayerPrefs.GetInt("numPlayers");
		PlayerPrefs.SetInt("numPlayers", playerID + 1);

		startTime = Time.time;
		
		currentAttempt.StartTime = System.DateTime.Now.Ticks;
	}

	public static void Save() {
		if (!Active)
			return;

		int i = 0;
		foreach (Attempt att in currentLevel)
			att.Save(levelName, playerID, i++);

		// numPlays{playerID},{stage},{level}
		PlayerPrefs.SetInt("numPlays" + playerID + "," + levelName, i);
		SaveDensities();
	}

	private static void LoadDensities() {
		string key = "dens" + levelName;
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

		string key = "dens" + levelName;
		string val = Grid.Dim.ToString();

		for (int i = 0; i < Grid.Dim; ++i)
			for (int j = 0; j < Grid.Dim; ++j)
				val += "," + densities[i, j];

		PlayerPrefs.SetString(key, val);
	}

}
