using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int nGranniesFallen = 0;
	public int nGranniesSpawned = 0;
	public int nGranniesToSpawn = 5;
	public int grannySpawningTime = 10;
	public Transform grannyPrefab;
	private float timescale;

	bool inPauseScreen = false;
	bool inMainMenuScreen = false;
	bool inWinScreen = false;
	bool inLoseScreen = false;
	bool playing = false;

	GameObject pauseScreen;
	GameObject mainMenuScreen;
	GameObject winScreen;
	GameObject loseScreen;
	List<GameObject> screens = new List<GameObject>();

	// Use this for initialization
	void Start () {
		pauseScreen = GameObject.FindGameObjectWithTag ("PauseScreen");
		mainMenuScreen = GameObject.FindGameObjectWithTag ("MainMenuScreen");
		winScreen = GameObject.FindGameObjectWithTag ("WinScreen");
		loseScreen = GameObject.FindGameObjectWithTag ("LoseScreen");

		screens.Add (pauseScreen);
		screens.Add (mainMenuScreen);
		screens.Add (winScreen);
		screens.Add (loseScreen);

		ClearScreens ();
		OnMainMenu (null);
	}

	void SpawnGranny() {
		nGranniesSpawned++;
		Instantiate (grannyPrefab, new Vector3((float) (0 + nGranniesSpawned), 4.13f, -4f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {

		if (playing) {
			if (Input.GetKeyDown (KeyCode.P))
				OnPauseGame (null);
			
		} else if (inMainMenuScreen) {
			if (Input.GetKeyDown (KeyCode.Space))
				OnGameStart (null);
		
		} else if (inPauseScreen) {
			if (Input.GetKeyDown (KeyCode.P))
				OnUnpauseGame (null);
			else if (Input.GetKeyDown (KeyCode.R))
				OnRestartGame (null);
			else if (Input.GetKeyDown (KeyCode.Q))
				OnQuitGame (null);
			
		} else if (inWinScreen) {
			if (Input.GetKeyDown (KeyCode.R))
				OnRestartGame (null);
			else if (Input.GetKeyDown (KeyCode.Q))
				OnQuitGame (null);

		} else if (inLoseScreen) {
			if (Input.GetKeyDown (KeyCode.R))
				OnRestartGame (null);
			else if (Input.GetKeyDown (KeyCode.Q))
				OnQuitGame (null);

		}

	}

	public void ClearScreens() {
		foreach (GameObject screen in screens) {
			screen.SetActive (false);
		}
	}

	public void OnMainMenu(object msg) {
		StopWorld ();
		inMainMenuScreen = true;
		mainMenuScreen.SetActive (true);
	}

	public void OnGameStart(object msg) {
		inMainMenuScreen = false;
		ClearScreens ();
		UnstopWorld ();
		playing = true;
		for (int i = 0; i < nGranniesToSpawn; i++) {
			Invoke ("SpawnGranny", i + 2);
		}
	}

	public void OnPauseGame(object msg) {
		StopWorld ();
		inPauseScreen = true;
		playing = false;
		pauseScreen.SetActive (true);
	}

	public void OnUnpauseGame(object msg) {
		inPauseScreen = false;
		playing = true;
		pauseScreen.SetActive (false);
		UnstopWorld ();
	}

	public void StopWorld() {
		Time.timeScale = 0;
	}

	public void UnstopWorld() {
		Time.timeScale = 1;
	}

	public void OnRestartGame(object msg) {
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void OnQuitGame(object msg) {
		Application.Quit ();
	}

	public void OnGameLost(object msg) {
		loseScreen.SetActive (true);
		playing = false;
		StopWorld ();
		inLoseScreen = true;
	}

	public void OnGameWon(object msg) {
		winScreen.SetActive (true);
		playing = false;
		inWinScreen = true;
		StopWorld ();
	}

	public void OnGrannyFallen (object msg) {
		nGranniesFallen++;
		Debug.Log (nGranniesFallen);
		if (nGranniesFallen >= nGranniesToSpawn) {
			OnGameWon (null);
		}
	}
}
