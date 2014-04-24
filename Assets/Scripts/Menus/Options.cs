using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {

	public GUIStyle textStyle;
	private GUIStyle toggleStyle;

	private bool colorblindMode;
	private float volume;

	private LevelManager lm;
	private MusicSelector music;

	float virtualWidth = 960.0f; //create gui for this size, use matrix to automaticly scale it
	float virtualHeight = 600.0f;

	bool initialCBMode;

	private void Awake() {
		LevelManager.optionsScreen = true;
	}

	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");
		music = Utils.FindComponent<MusicSelector>("Music");

		colorblindMode = lm.colorblindMode;
		volume = music.GetVolume();

		initialCBMode = colorblindMode;
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
			toggleStyle.alignment = TextAnchor.UpperCenter;
			
			textStyle.alignment = TextAnchor.UpperCenter;
		}

		
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
		                           new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1.0f));

		colorblindMode = GUI.Toggle(new Rect(virtualWidth*0.42f, virtualHeight*0.45f,
		                                     virtualWidth*0.2f, virtualHeight*0.04f),
		                            colorblindMode, "Enable colorblind mode", toggleStyle);
		GUI.Label(new Rect(virtualWidth*0.42f, virtualHeight*0.49f,
		                   virtualWidth*0.2f, virtualHeight*0.04f),
		                   "(Changing this setting will restart your level.)", textStyle);
		volume = GUI.HorizontalSlider(new Rect(virtualWidth*0.47f, virtualHeight*0.58f,
		                                       virtualWidth*0.13f, virtualHeight*0.04f), volume, 0, 1);
		GUI.Label(new Rect(virtualWidth*0.4f, virtualHeight*0.58f,
		                   virtualWidth*0.05f, virtualHeight*0.04f), "Volume", textStyle);

		music.SetVolume(volume);
		lm.colorblindMode = colorblindMode;

		if (GUI.Button(new Rect(virtualWidth*0.47f, virtualHeight*0.63f,
		                        virtualWidth*0.06f, virtualHeight*0.06f), "Back")) {
			LevelManager.optionsScreen = false;
			lm.ReturnFromOptionsMenu(colorblindMode != initialCBMode);

		}
	}
}
