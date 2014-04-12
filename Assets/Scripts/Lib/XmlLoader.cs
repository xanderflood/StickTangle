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
				Log.debug("Parsing stage " + stage);
				reader.ReadToFollowing("level");
				do {
					LevelState ls = new LevelState();
					ls.id = id++;
					ls.stage = stage;

					Utils.Assert(reader.MoveToAttribute("id"));
					ls.level = XmlConvert.ToInt32(reader.Value);

					Utils.Assert(reader.ReadToFollowing("bronze"));
					ls.bronzeMoves = XmlConvert.ToInt32(reader.Value);
					Utils.Assert(reader.ReadToFollowing("silver"));
					ls.silverMoves = XmlConvert.ToInt32(reader.Value);
					Utils.Assert(reader.ReadToFollowing("gold"));
					ls.goldMoves = XmlConvert.ToInt32(reader.Value);

					Utils.Assert(reader.ReadToFollowing("narration"));
					ls.narrationText = reader.Value;

					state.Add(ls);
				} while (reader.ReadToNextSibling("level"));
			}
		}

		return state;
	}
}
