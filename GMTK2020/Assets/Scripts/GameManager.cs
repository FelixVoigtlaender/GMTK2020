using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton;
	[Header("GameInfo")]
	public Action onNewTick;
	public Action onStartRain;
	public int tick { get; private set; } = 0;
	public float tickRate { get; private set; } = .5f;
	public bool gameIsOver { get; private set; } = false;
	Building[] buildings;
	int numBuildings = 0;

	[Header("Rainsettings")]
	public int timeToRain = 120;
	float foreshadowTime = 10;
	bool startedRain = false;
	bool enableLightning = true;
	public GameObject gameEndPanel;
	public TMP_Text gameEndText;
	public TMP_Text scoreText;
	public RawImage rainImage;
	// Use this for initialization

	private void Awake()
	{
		singleton = this;
		buildings = FindObjectsOfType<Building>();
		numBuildings = buildings.Length;
		foreach (Building building in buildings)
		{
			building.OnExplosion += buildingExploded;
		}
		rainImage.color = new Color(0, 0, 0, 0);
	}

	void buildingExploded()
	{
		numBuildings--;
		if (numBuildings <= 0)
		{
			EndGame(false);

		}
	}

	private void EndGame(bool isWin)
	{
		if (gameIsOver)
			return;

		gameEndPanel.SetActive(true);
		gameIsOver = true;
		if (isWin)
		{
			print("YOU DID IT!!!!!");
			gameEndText.text = "CONTROL REGAINED";
			gameEndText.color = Color.green;
			scoreText.text = "Enough";
			scoreText.color = Color.green;
			onStartRain?.Invoke();


		}
		else
		{
			print("GAME OVER!");
			gameEndText.text = "OUT OF CONTROL!";
			gameEndText.color = Color.red;
			scoreText.text = (tick * tickRate) + " hours";
			scoreText.color = Color.red;
		}
	}

	IEnumerator StartRain()
	{
		//target is 130
		float starttime = Time.time;
		float alpha = 0;

		while (starttime-Time.time<1000)
		{
			alpha = Mathf.Lerp(0, .4f, (Time.time - starttime) / foreshadowTime);
			if (enableLightning)
			{
				int value = UnityEngine.Random.Range(0, 101);
				if (value < 10)
				{
					rainImage.color = new Color(20, 20, 20, alpha);
					yield return new WaitForSeconds(.2f);

				}
			}

			rainImage.color = new Color(0, 0, 0, alpha);

			yield return new WaitForSeconds(.1f);
		}

	}

	void Start()
	{
		InvokeRepeating("newGameTick", 0, tickRate);
	}

	// Update is called once per frame
	void newGameTick()
	{
		onNewTick();
		tick++;
		if (tick >= tickRate * foreshadowTime && !startedRain)
		{
			StartCoroutine("StartRain");
			startedRain = true;
		}
		if (tick >= tickRate * timeToRain)
		{
			EndGame(true);
		}
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			Time.timeScale = 5;
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			Time.timeScale = 1;
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("gameScene");
	}
}
