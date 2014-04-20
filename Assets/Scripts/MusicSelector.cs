using UnityEngine;
using System.Collections;

public class MusicSelector : MonoBehaviour {


	public AudioClip melody0;
	public AudioClip melody1;
	public AudioClip melody2;
	public AudioClip melody3;
	public AudioClip melody4;

	public AudioClip blop;
	public AudioClip clear;
	public AudioClip teleport;
	public AudioClip acid;

	private LevelManager lm;

	private void Start() {
		lm = Utils.FindComponent<LevelManager>("LevelManager");	
	}

	private void Update() {
		if (LevelManager.modeling)
			return;

		string name = Application.loadedLevelName;

		if (name == "LevelSelect") {
			audio.clip = melody0;
			if (!audio.isPlaying)
				audio.Play();
		}

		if ((name == "Level1.1")||(name == "Level1.2")){
			if (!audio.isPlaying)
				audio.clip = melody1;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (lm.CurrentLevelInRange("1.2", "1.8")) {
			audio.clip = melody1;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (name == "Level2.1") {
			if (!audio.isPlaying)
				audio.clip = melody2;
			if (!audio.isPlaying)
				audio.Play(); 
		}

		if (lm.CurrentLevelInRange("2.2", "2.5")) {
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

		if (lm.CurrentLevelInRange("3.2", "3.4")) {
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
}
