using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
	// Cached References
	AudioSource audioSource = null;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySoundEffect(AudioClip soundEffect, float volume)
	{
		if(!audioSource)
			return;

		if(!soundEffect)
			return;

		audioSource.PlayOneShot(soundEffect, volume);
	}
}
