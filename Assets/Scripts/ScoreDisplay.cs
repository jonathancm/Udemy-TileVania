using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
	// Cached References
	GameSession gameSession;
	Text scoreText;

	// Start is called before the first frame update
	void Start()
	{
		scoreText = GetComponent<Text>();
		gameSession = FindObjectOfType<GameSession>();

		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		int score = 0;

		if(!gameSession)
			return;

		score = gameSession.GetScore();
		scoreText.text = score.ToString();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateDisplay();
	}
}
