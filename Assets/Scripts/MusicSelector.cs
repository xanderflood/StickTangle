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
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().clip = melody0;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play(); 
		}

		if (lm.CurrentLevelInRange("1.1", "1.8")) {
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().clip = melody1;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play(); 
		}

		if (lm.CurrentLevelInRange("2.1", "2.5")) {
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().clip = melody2;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play(); 
		}

		if (lm.CurrentLevelInRange("3.1", "3.6")) {
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().clip = melody3;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play(); 
		}

		if (lm.CurrentLevelInRange("4.1", "4.4")) {
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().clip = melody4;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}
	}

	public void playBlop(){
		GetComponent<AudioSource>().PlayOneShot(blop);
	}
	public void clearLevel(){
		GetComponent<AudioSource>().PlayOneShot(clear);
	}
	public void playTeleport(){
		GetComponent<AudioSource>().PlayOneShot(teleport);
	}
	public void playAcid(){
		GetComponent<AudioSource>().PlayOneShot(acid);
	}
	public void playSchoolBell(){
		GetComponent<AudioSource>().PlayOneShot (schoolBell);
	}
	public void playSelect(){
		GetComponent<AudioSource>().PlayOneShot (button_select);
	}
	public void playMove(){
		GetComponent<AudioSource>().PlayOneShot (button_move);
	}
	public void playBack(){
		GetComponent<AudioSource>().PlayOneShot (button_back);
	}
	public void SetVolume(float volume) {
		GetComponent<AudioSource>().volume = volume;
		magnet.volume = volume;
		wallBump.volume = volume;
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
		return GetComponent<AudioSource>().volume;
	}
}
