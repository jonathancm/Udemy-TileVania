using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public void LoadMainMenu(float delayInSeconds)
	{
		StartCoroutine(WaitAndLoad(0, delayInSeconds));
	}

	public void LoadNextScene(int index, float delayInSeconds)
	{
		int currentScene = 0;

		currentScene = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(WaitAndLoad(currentScene + 1, delayInSeconds));
	}

	public void ReloadScene(float delayInSeconds)
	{
		int currentScene = 0;

		currentScene = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(WaitAndLoad(currentScene, delayInSeconds));
	}

	IEnumerator WaitAndLoad(int index, float delayInSeconds)
	{
		yield return new WaitForSecondsRealtime(delayInSeconds);
		SceneManager.LoadScene(index);
	}
}
