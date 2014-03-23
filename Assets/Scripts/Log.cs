using UnityEngine;
using System.Collections;

public class Log {

	private static LogLevel level = LogLevel.Debug;
	private enum LogLevel {
		Error, Warning, Debug, Verbose
	}

	private static void log(LogLevel l, object obj) {
		if (level >= l) {
			Debug.Log(obj);
		}
	}

	public static void error(object obj) {
		log(LogLevel.Error, obj);
	}

	public static void warning(object obj) {
		log(LogLevel.Warning, obj);
	}

	public static void debug(object obj) {
		log(LogLevel.Debug, obj);
	}

	public static void verbose(object obj) {
		log(LogLevel.Verbose, obj);
	}
}
