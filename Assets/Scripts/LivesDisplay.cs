using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
	// Cached References
	Text livesText;
	GameSession gameSession;

    // Start is called before the first frame update
    void Start()
	{
		livesText = GetComponent<Text>();
		gameSession = FindObjectOfType<GameSession>();

		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		int lives = 0;

		if(!gameSession)
			return;

		lives = gameSession.GetLives();
		livesText.text = lives.ToString();
	}

	// Update is called once per frame
	void Update()
    {
		UpdateDisplay();
	}
}
