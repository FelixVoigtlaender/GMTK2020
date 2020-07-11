using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton;
	public Action onNewTick;
	public int tick = 0;
	
	// Use this for initialization

	private void Awake()
	{
		singleton = this;
	}
	void Start()
	{
		InvokeRepeating("newGameTick", 0, .5f);
	}

	// Update is called once per frame
	void newGameTick()
	{
		onNewTick();
		tick++;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			Time.timeScale = 5;
		}
		else
		{
			Time.timeScale = 1;
		}
	}
}
