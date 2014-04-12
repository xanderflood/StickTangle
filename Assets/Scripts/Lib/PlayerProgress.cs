using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelRecord {

	public LevelRecord(int idealMoves, int idealPieces) {
		Init(idealMoves, idealPieces, -1, -1, 0);
	}

	public LevelRecord(int idealMoves, int idealPieces, int pieces,
	                     int moves, int stars) {
		Init(idealMoves, idealPieces, pieces, moves, stars);
	}

	private void Init(int idealMoves, int idealPieces, int pieces,
	                  int moves,  int stars) {
		Moves = moves;
		IdealMoves = idealMoves;
		Pieces = pieces;
		IdealPieces = idealPieces;
		Stars = stars;
	}

	public int Moves,
		IdealMoves,
		Pieces,
		IdealPieces,
		Stars;

	public override string ToString() {
		return string.Format("LevelRecord({0}, {1}, {2}, {3}, {4})",
		                     Moves, IdealMoves, Pieces, IdealPieces, Stars);
	}
}

public static class PlayerProgress {

	public static int CurrentLevel;
	
	private static Dictionary<int, LevelRecord> _AllPlayerProgress
		= new Dictionary<int, LevelRecord>();

	public static Dictionary<int, LevelRecord> AllPlayerProgress {
		get {

			if (!Initialized)
				Initialize();
			return _AllPlayerProgress;
		}
	}

	private static bool Initialized = false;

	private static void Initialize() {
		if (Initialized)
			return;

		// These are numbered from 1 in order to jive with the scene indexing
		// I would put this into an array, but I eventually want to load it
		// from somewhere persistent anyways, so that we can save player progress
		_AllPlayerProgress.Add (1, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (2, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (3, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (4, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (5, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (6, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (7, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (8, new LevelRecord(0, 0));
		_AllPlayerProgress.Add (9, new LevelRecord(0, 0));

		Initialized = true;
	}

}
