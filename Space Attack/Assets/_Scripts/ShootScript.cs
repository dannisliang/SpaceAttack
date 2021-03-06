﻿using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour 
{
	public GameController gameController;

	public GameObject shot;
	public Transform shotSpawn;

	public float fireRate = 0.5f;
	private float nextFire;
	
	void Update ()
	{
		if ((Time.time > nextFire) && gameController.LevelIsRunning())
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

			int audioEnabled = PlayerPrefs.GetInt("AudioEnabled");

			if (audioEnabled == 1)
			{
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
