using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	// Configurable Parameters
	[Header("Audio Clips")]
	[SerializeField] AudioClip introMusic = null;
	[SerializeField] AudioClip gameMusic = null;

	[Header("Settings")]
	[SerializeField] float audioVolume = 0.25f;

	// Cached References
	public static MusicPlayer instance = null;
	AudioSource audioSource1 = null;
	AudioSource audioSource2 = null;

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

	// Start is called before the first frame update
	void Start()
    {
		audioSource1 = InitializeAudioSource(false, introMusic);
		audioSource2 = InitializeAudioSource(true, gameMusic);

		PlayGameMusic();
	}

	private AudioSource InitializeAudioSource(bool isLoop, AudioClip clip)
	{
		AudioSource audioSource;

		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.volume = audioVolume;
		audioSource.loop = isLoop;
		audioSource.clip = clip;

		return audioSource;
	}

	private void PlayGameMusic()
	{
		// Play Intro Music
		audioSource1.Play();

		// Play Game Music
		audioSource2.PlayDelayed(audioSource1.clip.length);
	}
}

