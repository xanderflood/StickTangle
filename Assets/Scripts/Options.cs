using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {

	public GUIStyle textStyle;
	private GUIStyle toggleStyle;

	private bool colorblindMode = false;
	private float volume = 0.2f;

	private LevelManager lm;
	private MusicSelector music;

	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
		music = Utils.FindComponent<MusicSelector>("Music");
	}

	private void OnGUI() {
		if (toggleStyle == null) {
			toggleStyle = new GUIStyle(GUI.skin.toggle);
			toggleStyle.normal.textColor = Color.black;
			toggleStyle.hover.textColor = Color.grey;
			toggleStyle.active.textColor = Color.grey;
			toggleStyle.onNormal.textColor = Color.black;
			toggleStyle.onHover.textColor = Color.grey;
			toggleStyle.onActive.textColor = Color.grey;
		}

		colorblindMode = GUI.Toggle(new Rect(30, 30, 170, 20), colorblindMode, "Enable colorblind mode", toggleStyle);
		volume = GUI.HorizontalSlider(new Rect(30, 60, 100, 20), volume, 0, 1);
		GUI.Label(new Rect(140, 60, 50, 20), "Volume", textStyle);

		music.SetVolume(volume);
		lm.colorblindMode = colorblindMode;

		if (GUI.Button(new Rect(30, 90, 50, 20), "Back")) {
			lm.ReturnFromOptionsMenu();
		}
	}
}
