using System;
using UnityEngine;

public class Attempt {
	public int Moves;
	public int Attaches;
	public float Time;
	public long StartTime;
	public bool Success;
	public int ResetX;
	public int ResetY;

	public Attempt() { }

	public Attempt(string str) {
		string[] ss = str.Split(',');

		Moves = Convert.ToInt32(ss[0]);
		Attaches = Convert.ToInt32(ss[1]);
		Time = Convert.ToSingle(ss[2]);
		Success = Convert.ToBoolean(ss[3]);
		ResetX = Convert.ToInt32(ss[4]);
		ResetY = Convert.ToInt32(ss[5]);

		if (ss.Length > 6)
			StartTime = Convert.ToInt64(ss[6]);
		else
			StartTime = -1;
	}
	
	public override string ToString() {
		return Moves + "," + Attaches + "," + Time + ","+ Success + "," + ResetX + "," + ResetY + "," + StartTime;
	}
}

