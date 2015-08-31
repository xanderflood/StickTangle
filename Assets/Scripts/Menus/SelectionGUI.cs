using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LevelState = XmlLoader.LevelState;

public class SelectionGUI : MonoBehaviour {
	
	public bool StageSelected;

	public LevelSelectDisplayScript disp;

	private MusicSelector music;

	public Material right;
	public Material left;
	public Material up;
	public Material down;
	
	public List<GameObject> StageAvatars;

	private int length;
	
	private string[] StageTitles;
	
	private string[][] LevelTitles;
	private LevelState[][] LevelStates;

	private int[] previousSelections;

	private int selection = 0;
	private int savedStage;
	
	void Awake() {
		LevelManager.modeling = true;
	}
	
	private void Start () {
		// Load stage data
		List<LevelState> ls = XmlLoader.LoadXml("levels").First;

		music = Utils.FindComponent<MusicSelector> ("Music");

		previousSelections = new int[XmlLoader.NumStages];
		for (int i = 0; i < XmlLoader.NumStages; ++i)
			previousSelections[i] = 0;

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
		if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button8)) && selection != length - 1) {
			music.playMove();
			selection += 1;
		}
		if ((Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.Joystick1Button7)) && selection != 0) {
			music.playMove();
			selection -= 1;
		}

		if ((Input.GetKeyDown(KeyCode.DownArrow)) || (Input.GetKeyDown(KeyCode.Joystick1Button6))) {
			music.playSelect();

			if (StageSelected) {
				music.playSchoolBell();

				GameObject.Destroy(Utils.FindObject("Level"));
				Utils.FindObject("Border").GetComponent<Renderer>().enabled = false;
				LevelManager.modeling = false;
				string levelName = LevelStates[savedStage][selection].name;
				Utils.FindComponent<LevelManager>("LevelManager").SetIndex(levelName);
				Application.LoadLevel(levelName);
			} else {
				StageSelected = true;
				disp.Current.SetActive(false);
				savedStage = selection;
				selection = previousSelections[savedStage];
			}
		}
		if ((Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.Joystick1Button5))) && StageSelected) {
			music.playBack();
			StageSelected = false;
			previousSelections[savedStage] = selection;
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

	private IEnumerator playBell() {
		music.playSchoolBell();
		yield return new WaitForSeconds(1.5f);
	}
}
