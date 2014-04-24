using UnityEngine;
using System.Collections;

public class MainMenuAnimation : MonoBehaviour {

	public GameObject RectWord;
	public GameObject StickyWord;
	public GameObject Holder;
	public Acid acid;

	public float rate;

	public GUIStyle textStyle;

	private MusicSelector music;

	bool gui = false;

	void Awake () {
		LevelManager.modeling = true;
	}

	void Start () {
		music = Utils.FindComponent<MusicSelector> ("Music");
		StartCoroutine(InitialWait());
	}
	
	void OnGUI() {
		
		GUI.Label(new Rect(0.4f * Screen.width, 0.62f * Screen.height,
		                   0.1f * Screen.width, 0.4f * Screen.height),
		          		   "Press down to begin", textStyle);
		
		GUI.Label(new Rect(0.38f * Screen.width, 0.52f * Screen.height,
		                   0.1f * Screen.width, 0.4f * Screen.height),
		          		   "Press up for level select", textStyle);

	}
	
	void Update() {

		if (!gui)
			return;
		
		if (Input.GetKey(KeyCode.DownArrow)) {
			music.playSchoolBell();
			LevelManager.modeling = false;
			Utils.FindComponent<LevelManager>("LevelManager").SetIndex("Level1.1");
			Application.LoadLevel("Level1.1");
		}
		if (Input.GetKey (KeyCode.UpArrow)){
			music.playSelect ();
			Application.LoadLevel("LevelSelect");
		}
	}

	IEnumerator InitialWait() {
		yield return new WaitForSeconds(1f);
		StartCoroutine(MoveRect());
		StartCoroutine(MoveSticky());
	}

	IEnumerator MoveRect() {

		Vector3 tmp;

		bool glowing = false;
		//First, move downwards to the magnet
		while(RectWord.transform.position.y > -4) {
			tmp = RectWord.transform.position;
			tmp.y -= rate*Time.deltaTime;
			RectWord.transform.position = tmp;

			if (tmp.y < -3 && !glowing) {
				for (int i = 0; i < 3; ++i)
					RectWord.GetComponent<WordPiece>().Letters[i].StartMagnetGlow();

				glowing = true;
			}

			yield return true;
		}

		//transform.position = new Vector3(-4, 0, -1);

		// Leave those behind and start fading them out, with the magnets
		RectWord.GetComponent<WordPiece>().SeverFirstLetter().transform.parent.parent = Holder.transform;
		RectWord.GetComponent<WordPiece>().SeverFirstLetter().transform.parent.parent = Holder.transform;
		RectWord.GetComponent<WordPiece>().SeverFirstLetter().transform.parent.parent = Holder.transform;

		StartCoroutine(fadeChildren(Holder, 1f));

		// Change the letter 't' to a capital
		RectWord.GetComponent<WordPiece>().Letters[0].letter = "T";

		// Move back, but one space further up
		Vector3 disp = new Vector3 (-3f, 5f, 0f);
		float total = disp.magnitude;
		Vector3 dir = disp/disp.magnitude;
		Vector3 start = RectWord.transform.position;

		float traversed = 0f;
		while (traversed < total) {
			Vector3 step = dir*rate*Time.deltaTime;

			RectWord.transform.position += step;

			traversed += step.magnitude;

			yield return true;
		}
		RectWord.transform.position = start + disp;

		// Initialize the actual GUI
		gui = true;
		StartCoroutine(GuiFade());
	}

	IEnumerator GuiFade() {

        Color c = textStyle.normal.textColor;
		c.a = 0;
		while (c.a < 1f)	{
			c.a += rate*Time.deltaTime;
            textStyle.normal.textColor = c;
			yield return true;
		}
	}

	IEnumerator MoveSticky() {
		
		Vector3 tmp;
		bool dissolving = false;

		//First, move towards the acid and burn up the y
		while(StickyWord.transform.position.x < -4) {
			tmp = StickyWord.transform.position;
			tmp.x += rate*Time.deltaTime;
			StickyWord.transform.position = tmp;
			
			if (tmp.x > -6 && !dissolving) {

				StickyWord.GetComponent<WordPiece>().Letters[5].
					StartAcidAnimation(0, 1, acid);
				StartCoroutine(FadeLetter(StickyWord.GetComponent<WordPiece>().
				                          Letters[5], rate));
				
				dissolving = true;
			}
			
			yield return true;
		}

		yield return true;
	}

	IEnumerator FadeLetter(LetterPiece lp, float r) {

		while (lp.fontColor.a > 0) {
			lp.fontColor.a -= 10*r*Time.deltaTime;
			yield return true;
		}

		StickyWord.GetComponent<WordPiece>().DeleteLastLetter();
	}

	IEnumerator fadeChildren(GameObject go, float rate) {

		int total = go.transform.childCount;

		for (int i = 0; i < total; ++i) {
			Transform sub = go.transform.GetChild(i).FindChild("Block");
			if (sub != null)
				GameObject.Destroy(sub.FindChild("Quad").gameObject);
		}

		for (int i = 0; i < total; ++i) {
			
			GameObject cur = go.transform.GetChild(i).gameObject;

			if (cur.name == "PieceWithLetter(Clone)")
				StartCoroutine(FadeLetter(cur.transform.FindChild("Block").
				                          GetComponent<LetterPiece>(),rate/3));
			else
				StartCoroutine(DissolveAnimation.fadeObject(cur, rate));
		}

		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < total; ++i) {
			
			GameObject cur = go.transform.GetChild(i).gameObject;
			
			if (cur.name == "PieceWithLetter(Clone)")
				GameObject.Destroy(cur);
		}

	}
}
