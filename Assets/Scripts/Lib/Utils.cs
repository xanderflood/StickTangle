#define DEBUG

using UnityEngine;
using System;
using System.Diagnostics;

public class Utils {
	[Conditional("DEBUG")]
	public static void Assert(bool condition) {
		if (!condition) throw new Exception();
	}

	public static GameObject FindObject(string objectName) {
		GameObject obj = GameObject.Find(objectName);
		Utils.Assert(obj);
		return obj;
	}

	public static T FindComponent<T>(string objectName) where T : Component {
		T component = FindObject(objectName).GetComponent<T>();
		Utils.Assert(component != null);
		return component;
	}

	public static T GetComponent<T>(GameObject obj) where T : Component {
		T component = obj.GetComponent<T>();
		Utils.Assert(component != null);
		return component;
	}
}
