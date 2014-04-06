using UnityEngine;
using System.Collections.Generic;

// The number of scenes which are NOT game levels

public class LevelSelectScreen : MonoBehaviour {
	
	public const int NUM_EXTRA_SCENES = 2;

	public SelectionGUI gui;
	public List<string> chapterTitles;
	public Texture img;

	public List<int> numsLevels;

	public bool chapterChosen;
	public int chapterSelection;

	// Use this for initialization
	void Start () {

		gui.Titles = chapterTitles;
		gui.Images = Repeat<Texture>(gui.Titles.Count, img);
	}
	
	// Update is called once per frame
	void Update () {
		if (!chapterChosen) {
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				chapterSelection = gui.Selection;
				chapterChosen = true;

				gui.Titles = new List<string>();
				for(int i = 0; i < numsLevels[chapterSelection]; ++i)
					gui.Titles.Add((i + 1).ToString());
				gui.Images = Repeat<Texture>(gui.Titles.Count, img);
				
				gui.Selection = 0;
				gui.Refresh();
			}

		} else {
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				Application.LoadLevel(getLevelID(chapterSelection, gui.Selection));
			}
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				
				gui.Titles = chapterTitles;
				gui.Images = Repeat<Texture>(gui.Titles.Count, img);
				
				chapterChosen = false;
				gui.Selection = chapterSelection;
				gui.Refresh();
			}
		}
	}

	int getLevelID(int ch, int lvl) {

		int ret = 0;
		for (int i = 0; i < ch; ++i) {
			ret += numsLevels[i];
		}

		return ret + lvl + NUM_EXTRA_SCENES;
	}

	static List<T> Repeat<T>(int N, T val) {

		List<T> ret = new List<T>();
		for (int i = 0; i< N; ++i)
			ret.Add(val);

		return ret;
	}
}
