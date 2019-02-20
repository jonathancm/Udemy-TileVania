using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] int playerLives = 1;
	[SerializeField] float respawnDelay = 2f;

	// Cached References
	public static GameSession instance = null;

	// State Variables
	int currentScore = 0;
	int currentLives = 0;

	private void Awake()
	{
		SetupSingleton();
	}

	private void SetupSingleton()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(instance != this)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	void Start()
    {
		if(FindObjectOfType<StartGameButton>())
			FindObjectOfType<Player>().SetControlLocking(true);

		currentLives = playerLives;
	}

	public void StartGame()
	{
		FindObjectOfType<Player>().SetControlLocking(false);
	}

	public void ProcessPlayerDeath()
	{
		if(currentLives >= 1)
		{
			TakeLife();
		}
		else
		{
			ResetGameSession();
		}
	}

	private void TakeLife()
	{
		currentLives--;
		if(currentLives < 0)
			currentLives = 0;

		FindObjectOfType<LevelLoader>().ReloadScene(respawnDelay);
	}

	private void ResetGameSession()
	{
		FindObjectOfType<LevelLoader>().LoadMainMenu(respawnDelay);
		Destroy(gameObject);
	}

	IEnumerator DelayRespawn()
	{
		yield return new WaitForSecondsRealtime(respawnDelay);
	}

	public int GetLives()
	{
		return currentLives;
	}

	public void AddToScore(int amount)
	{
		currentScore += amount;
	}

	public int GetScore()
	{
		return currentScore;
	}
}
