﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour 
{	
	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;

	public float spawnWait;
	public float startWait;
	public float waveWait;

	public int numberOfWaves = 10;

	const string kHealth = "Health";
	const string kScore = "Score";
	
	public Text GameLabel;
	public Text HealthText;
	public Text ScoreText;
	public Text InfoText;

	int health = 100;
	int score = 0;

	public int level = 1;
	
	bool levelIsRunning = true;
	bool spawningWaves = true;

	double timeToWait = 5.0F;

	MusicController musicController;

	void Start ()
	{
//		Vector3 Size = Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width, 0, 0));	
//		spawnValues.x = Mathf.Floor (Size.x);

//		StartCoroutine (SpawnWaves ());

		health = 100;
		score = 0;

		musicController = (MusicController) GetComponent (typeof (MusicController));

		PlayerPrefs.SetInt (kHealth, health);
		PlayerPrefs.SetInt (kScore, score);

		levelIsRunning = true;

		GameLabel.text = "";

		if (level == 1)
		{
			InfoText.text = "Warning! Asteroids ahead";

			StartCoroutine ("SpawnWaves");
		}
		else if (level == 2)
		{
			InfoText.text = "Look Out! Enemies are coming";

			StartCoroutine ("SpawnWaves");
		}
		else if (level == 3)
		{
			InfoText.text = "Prepare for battle!";

			StartSpawningWaves ();
		} 
		else
		{
			InfoText.text = "Get Ready!";

			StartCoroutine ("SpawnWaves");
		}
	}

	void Update ()
	{
		if ((Time.time > timeToWait) && levelIsRunning)
		{
			InfoText.text = "";
		}

		health = PlayerPrefs.GetInt (kHealth);
		score = PlayerPrefs.GetInt (kScore);

		HealthText.text = "Health: " + health;
		ScoreText.text = "Score: " + score;

		if (health == 0 && levelIsRunning) 
		{
			GameLabel.text = "Game Over";
			levelIsRunning = false;

			StopCoroutine ("SpawnWaves");

			StartCoroutine ("FinishLevel");
		}

		if (! spawningWaves && levelIsRunning)
		{
			GameLabel.text = "Well Done";
			levelIsRunning = false;

			int highScore = PlayerPrefs.GetInt ("Level" + level + "Score");

			if (score > highScore)
			{
				PlayerPrefs.SetInt ("Level" + level + "Score", score);
				InfoText.text = "You've set a new record";
			}

			StartCoroutine ("FinishLevel");
		}
	}

	public bool LevelIsRunning ()
	{
		return this.levelIsRunning;
	}
	
	IEnumerator SpawnWaves ()
	{
		spawningWaves = true;

		yield return new WaitForSeconds (startWait);

		for (int wave = 0; wave < numberOfWaves; wave++)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

				//Quaternion spawnRotation = new Quaternion(0,180,0,0);
				Instantiate (hazard, spawnPosition, hazard.transform.rotation);

				yield return new WaitForSeconds (spawnWait);
			}

			yield return new WaitForSeconds (waveWait);
		}

		spawningWaves = false;
	}

	void StartSpawningWaves ()
	{
		spawningWaves = true;
	}

	public void EndSpawningWaves ()
	{
		spawningWaves = false;
	}

	public void FadeOutLevelMusic ()
	{
		float fadeOutTime = 4.0F;
		musicController.FadeOutMusic (fadeOutTime);
	}

	public void PlayMusic ()
	{
		GetComponent<AudioSource> ().Play ();
	}

	public void PauseMusic ()
	{
		GetComponent<AudioSource> ().Pause ();
	}

	IEnumerator FinishLevel ()
	{
		this.FadeOutLevelMusic ();
		yield return new WaitForSeconds (5.0F);

		Application.LoadLevel ("MainMenu");
	}
}