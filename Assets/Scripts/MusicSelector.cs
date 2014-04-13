using UnityEngine;
using System.Collections;

public class MusicSelector : MonoBehaviour {

	public AudioClip melody1;
	public AudioClip melody2;
	public AudioClip melody3;
	public AudioClip melody4;

	private void Update() {
		string name = Application.loadedLevelName;

		if (name == "Level1.1" || name == "Level1.2" || name == "Level1.3" || name == "Level1.4" ||
		    name == "Level1.5" || name == "Level1.6" || name == "Level1.7" || name == "Level1.8") {
			audio.clip = melody1;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (name == "Level2.1"){
			if (!audio.isPlaying)
				audio.clip = melody2;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (name == "Level2.2" || name == "Level2.3" || name == "Level2.4" || name == "Level2.5") {
			audio.clip = melody2;
			if (!audio.isPlaying)
				audio.Play(); 
		}
		
		if (name == "Level3.1") {
			if (!audio.isPlaying)
				audio.clip = melody3;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (name == "Level3.2" || name == "Level3.3") {
			audio.clip = melody3;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (name == "Level4.1") {
			if (!audio.isPlaying)
				audio.clip = melody4;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (name == "Level4.2") {
			audio.clip = melody4;
			if (!audio.isPlaying)
				audio.Play(); 
		}
	}
}
