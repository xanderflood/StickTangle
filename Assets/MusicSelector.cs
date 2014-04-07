using UnityEngine;
using System.Collections;

public class MusicSelector : MonoBehaviour {

	public AudioClip melody1;
	public AudioClip melody2;
	public AudioClip melody3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if ((Application.loadedLevelName == "Level0")||(Application.loadedLevelName == "Level1")||
		    (Application.loadedLevelName == "Level2")||(Application.loadedLevelName == "Level3")||
		    (Application.loadedLevelName == "Level4")||(Application.loadedLevelName == "Level5")||
		    (Application.loadedLevelName == "Level6")||(Application.loadedLevelName == "Level7")){

			audio.clip = melody1;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if ((Application.loadedLevelName == "Level2.1")){
			if (!audio.isPlaying)
				audio.clip = melody2;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if ((Application.loadedLevelName == "Level2.2")||(Application.loadedLevelName == "Level2.3")||
		    (Application.loadedLevelName == "Level2.4")||(Application.loadedLevelName == "Level2.5")){
			audio.clip = melody2;
			if (!audio.isPlaying)
				audio.Play(); 
		}
		
		if ((Application.loadedLevelName == "Level3.1")){
			if (!audio.isPlaying)
				audio.clip = melody3;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (Application.loadedLevelName == "Level3.2"){
			audio.clip = melody3;
			if (!audio.isPlaying)
				audio.Play(); 
		}
	
	}

}
