using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public GameObject OptionsMenu;
	public GameObject MainMenu;

	private void Awake()
	{
		AudioListener.volume = .5f;
		OpenMenu(MainMenu);
	}
	public void OpenMenu(GameObject menuToOpen)
	{
		OptionsMenu.SetActive(false);
		MainMenu.SetActive(false);
		menuToOpen.SetActive(true);
	}

	public void StartGame()
	{
		SceneManager.LoadScene("gameScene");
	}

	public void ChangeVolume(float newVolume)
	{
		AudioListener.volume = newVolume;
	}
}
