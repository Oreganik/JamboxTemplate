// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>A customizable behavior for common SFX triggers that require randomization or variance</summary>
	// For example, if you have an explosion sound effect that is heard often, you could randomly play one sound from a list,
	// and then further modify it with a randomized pitch.
	public class SoundEffect : MonoBehaviour 
	{
		public bool IsPlaying 
		{
			get { return _audioSource.clip != null && _audioSource.time < _audioSource.clip.length; }
		}

		#pragma warning disable 0649
		[SerializeField] private AudioClip[] _clips;
		[SerializeField] private FloatRange _pitchRange = new FloatRange(0.8f, 1.2f);
		[SerializeField] private bool _randomNoRepeat = true;
		[Tooltip("Only works for 2D sounds")]
		[SerializeField] private bool _stereoPanWithCameraPosition = true;
		[Tooltip("Useful when attaching this to particle effects")]
		[SerializeField] private bool _autoPlayOnEnable;
		#pragma warning restore 0649

		private static Camera s_mainCamera;

		private AudioSource _audioSource;
		private float _defaultVolume;
		private List<AudioClip> _unplayedClips;

		public void Play (float volume = -1)
		{
			PlayAtPosition(transform.position, volume);
		}

		public void PlayAtPosition (Vector3 position, float volume = -1)
		{
			transform.position = position;

			// If it's a 2D sound and we are panning stereo based on position... well... do it!
			if (_audioSource.spatialBlend == 0 && _stereoPanWithCameraPosition)
			{
				if (s_mainCamera == null)
				{
					s_mainCamera = Camera.main;
				}
				Vector3 viewportPoint = s_mainCamera.WorldToViewportPoint(transform.position);
				_audioSource.panStereo = (Mathf.Clamp01(viewportPoint.x) * 2) - 1; // returns a value of -1 to 1
			}

			// If it's random with no repeat, do a little song and dance.
			// BTW this is super dirty and will generate a lot of garbage collection.
			// Just a fair warning if you try to put this in production code!
			if (_randomNoRepeat && _clips.Length > 1)
			{
				if (_unplayedClips.Count == 0)
				{
					_unplayedClips.AddRange(_clips);
					_unplayedClips.RemoveAll(x => x == null);
				}

				// Remove the last played clip from the list, if applicable. This prevents a repeat when refreshing the list.
				if (_audioSource.clip != null)
				{
					_unplayedClips.Remove(_audioSource.clip);
				}

				// Set the new clip and remove it from the unplayed list.
				_audioSource.clip = _unplayedClips[Random.Range(0, _unplayedClips.Count)];
				_unplayedClips.Remove(_audioSource.clip);
			}
			else
			{
				_audioSource.clip = _clips[Random.Range(0, _clips.Length)];
			}

			_audioSource.volume = volume < 0 ? _defaultVolume : volume;
		}

		protected void Awake ()
		{
			if (_clips.Length == 0)
			{
				Debug.LogError(gameObject.name + " has zero sound clips and has been destroyed.");
				Destroy(gameObject);
				return;
			}

			foreach (AudioClip clip in _clips)
			{
				if (clip == null)
				{
					Debug.LogError(gameObject.name + " is missing an audio clip!");
				}
			}

			_audioSource = GetComponent<AudioSource>();

			if (_audioSource == null)
			{
				Debug.LogError(gameObject.name + " has no audio source and has been destroyed.");
				Destroy(gameObject);
				return;
			}

			_defaultVolume = _audioSource.volume;
			_unplayedClips = new List<AudioClip>(_clips);
			_unplayedClips.RemoveAll(x => x == null);
		}

		protected void OnEnable ()
		{
			if (_autoPlayOnEnable)
			{
				Play();
			}
		}
	}
}
