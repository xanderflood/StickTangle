using UnityEngine;
using System.Collections;

public class MusicSelector : MonoBehaviour {

	public AudioClip melody0;
	public AudioClip melody1;
	public AudioClip melody2;
	public AudioClip melody3;
	public AudioClip melody4;
	public AudioClip button_select;
	public AudioClip button_move;
	public AudioClip button_back;

	public AudioClip blop;
	public AudioClip clear;
	public AudioClip teleport;
	public AudioClip acid;
	public AudioClip schoolBell;
	public AudioSource wallBump;
	public AudioSource magnet;

	public bool isPlaying = false;

	private LevelManager lm;

	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");	
	}

	private void Update() {
		string name = Application.loadedLevelName;

		if (name == "LevelSelect" || name == "MainMenu") {
			if (!audio.isPlaying)
				audio.clip = melody0;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (lm.CurrentLevelInRange("1.1", "1.8")) {
			if (!audio.isPlaying)
				audio.clip = melody1;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (lm.CurrentLevelInRange("2.1", "2.5")) {
			if (!audio.isPlaying)
				audio.clip = melody2;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (lm.CurrentLevelInRange("3.1", "3.6")) {
			if (!audio.isPlaying)
				audio.clip = melody3;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (lm.CurrentLevelInRange("4.1", "4.4")) {
			if (!audio.isPlaying)
				audio.clip = melody4;
			if (!audio.isPlaying)
				audio.Play();
		}
	}

	public void playBlop(){
		audio.PlayOneShot(blop);
	}
	public void clearLevel(){
		audio.PlayOneShot(clear);
	}
	public void playTeleport(){
		audio.PlayOneShot(teleport);
	}
	public void playAcid(){
		audio.PlayOneShot(acid);
	}
	public void playSchoolBell(){
		audio.PlayOneShot (schoolBell);
	}
	public void playSelect(){
		audio.PlayOneShot (button_select);
	}
	public void playMove(){
		audio.PlayOneShot (button_move);
	}
	public void playBack(){
		audio.PlayOneShot (button_back);
	}
	public void SetVolume(float volume) {
		audio.volume = volume;
	}

	public void playBump(){
		if (!wallBump.isPlaying)
			wallBump.Play();
	}

	public void playMagnet(){
		if (!magnet.isPlaying)
			magnet.Play();
	}

	public float GetVolume() {
		return audio.volume;
	}


}
