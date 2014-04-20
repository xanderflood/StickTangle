using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;

public class XmlLoader {
	public class LevelState {
		public int id;
		public int stage;
		public int level;
		public int bronzeMoves;
		public int silverMoves;
		public int goldMoves;
		public List<string> narrationText1 = new List<string>();
		public List<string> narrationText2 = new List<string>();
		public string name;

		public override string ToString() {
			return string.Format("id: {0}, stage: {1}, level: {2}, name: {3}", id, stage, level, name); 
		}
	}

	static int numStages;
	public static int NumStages { get { return numStages; } }
	static List<int> numLevels = new List<int>();
	public static List<int> NumLevels { get { return numLevels; } }
	public static Pair<List<LevelState>, List<string>> LoadXml(string filename) {
		TextAsset xmlFile = Resources.Load<TextAsset>(filename);
		string xmlText = xmlFile.text;

		List<string> scenes = new List<string>();
		List<LevelState> state = new List<LevelState>();
		int id = 1;
		int stage = -1;
		using (XmlReader reader = XmlReader.Create(new StringReader(xmlText))) {
			reader.ReadToFollowing("sticktangle");
			while (reader.Read()) {
				bool done = false;
				switch (reader.NodeType) {
				case XmlNodeType.Element:
					if (reader.Name == "scene") {
						scenes.Add(reader.ReadElementContentAsString());
					} else if (reader.Name == "scenes") {
						continue;
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
			while (reader.ReadToFollowing("stage")) {
				Utils.Assert(reader.MoveToFirstAttribute());
				stage = XmlConvert.ToInt32(reader.Value);

				reader.ReadToFollowing("level");
				numLevels.Add(0);
				do {
					++numLevels[numLevels.Count - 1];
					LevelState ls = new LevelState();
					ls.id = id++;
					ls.stage = stage;

					Utils.Assert(reader.MoveToAttribute("id"));
					ls.level = XmlConvert.ToInt32(reader.Value);

					ls.name = "Level" + ls.stage + "." + ls.level;
					
					Utils.Assert(reader.ReadToFollowing("bronze"));
					ls.bronzeMoves = reader.ReadElementContentAsInt();
					Utils.Assert(reader.ReadToFollowing("silver"));
					ls.silverMoves = reader.ReadElementContentAsInt();
					Utils.Assert(reader.ReadToFollowing("gold"));
					ls.goldMoves = reader.ReadElementContentAsInt();

					reader.ReadEndElement(); // Read </stars> (</gold>?) tag

					bool done = false;
					while (reader.Read()) {
						switch (reader.NodeType) {
						case XmlNodeType.Element:
							if (reader.Name == "narration1") {
								ls.narrationText1.Add(reader.ReadElementContentAsString());
							} else if (reader.Name == "narration2") {
								ls.narrationText2.Add(reader.ReadElementContentAsString());
							} else {
								Debug.Log(reader.Name);
								Utils.Assert(false);
							}
							break;
						case XmlNodeType.EndElement:
							if (reader.Name == "level") {
								done = true;
							}
							break;
						}
						if (done == true) {
							break;
						}
					}

					state.Add(ls);
				} while (reader.ReadToNextSibling("level"));
			}
			
			numStages = stage;
		}

		return new Pair<List<LevelState>, List<string>>(state, scenes);
	}
}
