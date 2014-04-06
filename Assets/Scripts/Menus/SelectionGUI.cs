using UnityEngine;
using System.Collections.Generic;

public class SelectionGUI : MonoBehaviour {

	List<string> _Titles;
	List<Texture> _Images;
	public List<string> Titles {
		get { return _Titles; }
		set { _Titles = value; }
	}
	public List<Texture> Images {
		get { return _Images; }
		set { _Images = value; }
	}
	
	public GUITexture display;
	public TextMesh title;

	public GUITexture right;
	public GUITexture left;

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
		LoadSelection();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.RightArrow) && selection != _Titles.Count - 1) {
			selection += 1;
			LoadSelection();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && selection != 0) {
			selection -= 1;
			LoadSelection();
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

		if (selection == _Images.Count) {
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
		title.text = _Titles[selection];
		display.texture = _Images[selection];
	}
}
