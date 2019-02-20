using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
	// Configurable Parameters
	[Header("Stats")]
	[SerializeField] int scoreValue = 1;

	[Header("Coin Pickups")]
	[Range(0, 1)]
	[SerializeField] float sfxVolume = 0.5f;
	[SerializeField] AudioClip coinPickupSFX = null;

	// Cached References
	GameSession gameSession;

	private void Start()
	{
		gameSession = FindObjectOfType<GameSession>();
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if(!otherCollider.CompareTag("Player"))
			return;

		PlayCoinPickupSFX();

		if(gameSession)
		{
			gameSession.AddToScore(scoreValue);
		}

		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	private void PlayCoinPickupSFX()
	{
		var sfxPlayer = FindObjectOfType<SoundEffectPlayer>();

		if(sfxPlayer)
			sfxPlayer.PlaySoundEffect(coinPickupSFX, sfxVolume);
	}
}
