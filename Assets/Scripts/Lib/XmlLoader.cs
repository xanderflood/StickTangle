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
		public string narrationText;
		public string name;
	}

	public static List<LevelState> LoadXml(string filename) {
		StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + filename);
		string xmlText = sr.ReadToEnd();

		List<LevelState> state = new List<LevelState>();
		int id = 1;
		int stage;
		using (XmlReader reader = XmlReader.Create(new StringReader(xmlText))) {
			while (reader.ReadToFollowing("stage")) {
				Utils.Assert(reader.MoveToFirstAttribute());
				stage = XmlConvert.ToInt32(reader.Value);
				reader.ReadToFollowing("level");
				do {
					LevelState ls = new LevelState();
					ls.id = id++;
					ls.stage = stage;

					Utils.Assert(reader.MoveToAttribute("id"));
					ls.level = XmlConvert.ToInt32(reader.Value);

					Utils.Assert(reader.ReadToFollowing("bronze"));
					ls.bronzeMoves = reader.ReadElementContentAsInt();
					Utils.Assert(reader.ReadToFollowing("silver"));
					ls.silverMoves = reader.ReadElementContentAsInt();
					Utils.Assert(reader.ReadToFollowing("gold"));
					ls.goldMoves = reader.ReadElementContentAsInt();

					Utils.Assert(reader.ReadToFollowing("narration"));
					ls.narrationText = reader.ReadElementContentAsString();

					ls.name = "Level" + ls.stage + "." + ls.level;

					state.Add(ls);
				} while (reader.ReadToNextSibling("level"));
			}
		}

		return state;
	}
}
