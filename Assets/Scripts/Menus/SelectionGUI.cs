using UnityEngine;
using System.Collections.Generic;

public class SelectionGUI : MonoBehaviour {

	public AudioClip move;
	public AudioClip select;
	public AudioClip up;

	List<string> Titles = new List<string>();
	List<Texture> Images = new List<Texture>();
	
	public LevelSelectDisplayScript disp;

	public Material right;
	public Material left;

	int selection = 0;
	public int Selection {
		get { return selection; }
		set { selection = value; Refresh(); }
	}
	
	public void Refresh() {
		LoadSelection();
	}

	// Use this for initialization
	void Start () {
		XmlLoader.LoadXml("levels");

		Debug.Log (XmlLoader.NumStages);
		for (int i = 0; i < XmlLoader.NumStages; ++i) {
			Texture t = (Texture)Resources.Load("Stage" + (i + 1) + ".png");
			Images.Add(t);
		}
		
		for (int i = 0; i < XmlLoader.NumStages; ++i) {
			Titles.Add("Stage " + (i + 1));
		}

		LoadSelection();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.RightArrow) && selection != Titles.Count - 1) {
			audio.PlayOneShot(move);
			selection += 1;
			LoadSelection();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && selection != 0) {
			audio.PlayOneShot(move);
			selection -= 1;
			LoadSelection();
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
		    audio.PlayOneShot(select);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			audio.PlayOneShot(up);
		}
		arrows();
	}

	void arrows() {
		if (selection == 0) {
			Color c = left.color;
			c.a = .5f;
			left.color = c;
		} else {
			Color c = left.color;
			c.a = 1f;
			left.color = c;
		}

		if (selection == Images.Count) {
			Color c = right.color;
			c.a = .5f;
			right.color = c;
		} else {
			Color c = right.color;
			c.a = 1f;
			right.color = c;
		}

	}

	void LoadSelection() {
		disp.text = Titles[selection];
		disp.img = Images[selection];
	}
}
