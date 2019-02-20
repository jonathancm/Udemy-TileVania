using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
	// Configurable Parameter
	[SerializeField] float delayInSeconds = 2f;

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		FindObjectOfType<Player>().Warp();
		StartCoroutine(WaitAndLoadNextScene());
		gameObject.layer = LayerMask.NameToLayer("Non Interactables");
	}

	IEnumerator WaitAndLoadNextScene()
	{
		Time.timeScale = 0.2f;
		yield return new WaitForSecondsRealtime(delayInSeconds);
		Time.timeScale = 1f;

		var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex + 1);
	}
}
