using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSkeleton {

	public Pair<int,int> start;
	public List<Pair<int, int>> goals;
	public List<Pair<int, int>> obstacles;
	public List<Pair<int, int>> stickables;

	public LevelSkeleton(string file) {
		
		
		TextAsset xmlFile = Resources.Load<TextAsset>(file);
		string xmlText = xmlFile.text;
		
		stickables = new List<Pair<int, int>> ();
		obstacles = new List<Pair<int, int>> ();
		goals = new List<Pair<int, int>> ();
		
		using (XmlReader reader = XmlReader.Create(new StringReader(xmlText))) {
			
			reader.ReadToFollowing("level");
			while (reader.Read()) {
				bool done = false;
				switch (reader.NodeType) {
				case XmlNodeType.Element:
					if (reader.Name == "obstacles") {
						obstacles = loadPositions(reader);
					} else if (reader.Name == "goals") {
						goals = loadPositions(reader);
					} else if (reader.Name == "stickables") {
						stickables = loadPositions(reader);
					} else if (reader.Name == "player") {
						start = readPos(reader);
					} else {
						Debug.Log(reader.Name);
						Utils.Assert(false);
					}
					break;

				case XmlNodeType.EndElement:
					if (reader.Name == "scenes") {
						done = true;
					}
					break;
				}
				if (done == true) {
					break;
				}
			}
		}
	}

	private List<Pair<int,int>> loadPositions(XmlReader xr) {

		string name = xr.Name;
		List<Pair<int,int>> retval = new List<Pair<int,int>> ();

		while (xr.Read()) {
			bool done = false;
			switch (xr.NodeType) {
			case XmlNodeType.Element:
				if (xr.Name == "line") {
					xr.ReadToFollowing("direction");
					string dir = xr.ReadElementContentAsString();
					
					xr.ReadToFollowing("x");
					int x = xr.ReadElementContentAsInt();
					xr.ReadToFollowing("y");
					int y = xr.ReadElementContentAsInt();
					xr.ReadToFollowing("length");
					int length = xr.ReadElementContentAsInt();

					for (int i = 0; i < length; i++) {
						retval.Add (new Pair<int, int>(x, y));

						if (dir == "h")
							x++;
						else
							y++;
					}

				} else if (xr.Name == "point") {

					retval.Add (readPos(xr));
				} else {
					Debug.Log(xr.Name);
					Utils.Assert(false);
				}
				break;
			case XmlNodeType.EndElement:
				if (xr.Name == name) {
					done = true;
				}
				break;
			}
			if (done == true) {
				break;
			}
		}

		return retval;
	}

	Pair<int,int> readPos(XmlReader xr) {
		xr.ReadToFollowing("x");
		int x = xr.ReadElementContentAsInt();
		xr.ReadToFollowing("y");
		int y = xr.ReadElementContentAsInt();
		
		return new Pair<int, int>(x, y);
	}

}
