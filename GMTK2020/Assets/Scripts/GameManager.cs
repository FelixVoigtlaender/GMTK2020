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
}
