using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LevelState = XmlLoader.LevelState;

public class SelectionGUI : MonoBehaviour {

	public AudioClip move;
	public AudioClip select;
	public AudioClip back;
	public bool StageSelected;

	public LevelSelectDisplayScript disp;

	public MusicSelector music;

	public Material right;
	public Material left;
	public Material up;
	public Material down;
	
	public List<GameObject> StageAvatars;

	private int length;
	
	private string[] StageTitles;
	
	private string[][] LevelTitles;
	private LevelState[][] LevelStates;

	private int selection = 0;
	private int savedStage;
	
	void Awake() {
		LevelManager.modeling = true;
	}
	
	private void Start () {

		// Load stage data
		List<LevelState> ls = XmlLoader.LoadXml("levels").First;

		StageTitles = new string[XmlLoader.NumStages];
		LevelTitles = new string[XmlLoader.NumStages][];
		LevelStates = new LevelState[XmlLoader.NumStages][];
		for (int i = 0; i < XmlLoader.NumStages; ++i) {
			StageTitles[i] = "Stage " + (i + 1);
			LevelTitles[i] = new string[XmlLoader.NumLevels[i]];
			LevelStates[i] = new LevelState[XmlLoader.NumLevels[i]];
		}

		// Load level data
		foreach (LevelState l in ls) {
			LevelStates[l.stage - 1][l.level - 1] = l;
			LevelTitles[l.stage - 1][l.level - 1] = l.name;
		}

		// Setup
		LoadSelection();
	}
	
	void Update () {
		// Invalid inputs get a bump
		if (Input.GetKeyDown(KeyCode.UpArrow) && !StageSelected ||
		    Input.GetKeyDown(KeyCode.RightArrow) && selection == length - 1 ||
		    Input.GetKeyDown(KeyCode.LeftArrow) && selection == 0) {
			return;
		}

		// Here are all the other inputs
		if (Input.GetKeyDown(KeyCode.RightArrow) && selection != length - 1) {
			audio.PlayOneShot(move);
			selection += 1;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && selection != 0) {
			audio.PlayOneShot(move);
			selection -= 1;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
		    audio.PlayOneShot(select);

			if (StageSelected) {
				music.playSchoolBell();
				LevelManager.modeling = false;
				string levelName = LevelStates[savedStage][selection].name;
				Utils.FindComponent<LevelManager>("LevelManager").SetIndex(levelName);
				Application.LoadLevel(levelName);
			} else {
				StageSelected = true;
				disp.Current.SetActive(false);
				savedStage = selection;
				selection = 0;
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) && StageSelected) {
			audio.PlayOneShot(back);
			StageSelected = false;
			selection = savedStage;
		}

		// Update arrow materials accordingl
		arrows();

		// Finally, load the appropriate data
		LoadSelection();
	}

	void arrows() {

		Color c;
		if (selection == 0) {
			c = left.color;
			c.a = .1f;
			left.color = c;
		} else {
			c = left.color;
			c.a = 1f;
			left.color = c;
		}

		if (selection == length - 1) {
			c = right.color;
			c.a = .1f;
			right.color = c;
		} else {
			c = right.color;
			c.a = 1f;
			right.color = c;
		}

		if (StageSelected) {

			c = down.color;
			c.a = 0f;
			down.color = c;
			c = up.color;
			c.a = 1f;
			up.color = c;
		} else {

			c = down.color;
			c.a = 1f;
			down.color = c;
			c = up.color;
			c.a = 0f;
			up.color = c;
		}
	}

	void LoadSelection() {
		if (!StageSelected) {
			disp.mode = false;
			disp.text = StageTitles[selection];
			disp.img = StageAvatars[selection];

			disp.stageNum = selection + 1;

			length = StageTitles.Length;
		} else {
			disp.mode = true;
			disp.text = LevelTitles[savedStage][selection];
			length = XmlLoader.NumLevels[savedStage];
			
			disp.stageNum = selection + 1;

			disp.level = LevelStates[savedStage][selection];
		}
	}
	
}
