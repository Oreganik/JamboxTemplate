// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// Loads all sound files in Resources/Audio.
	/// Each can be called by name. 
	/// </summary>
	public class SimpleSounds : MonoBehaviour 
	{
		private const string RESOURCE_PATH = "Audio";

		private static Camera s_mainCamera;
		private static SimpleSounds s_instance;

		private Dictionary<string, AudioClip> _audioClipDictionary;
		private List<AudioSource> _activeAudioSources;
		private Queue<AudioSource> _audioSourcePool;

		/// <summary>Adjust the stereo panning of an object to match its screen position on the x axis</summary>
		public static void PlayWithCameraPan (AudioSource source, Vector3 worldPosition, Camera camera = null)
		{
			// If a camera is not passed, use the main camera
			if (camera == null)
			{
				if (s_mainCamera == null)
				{
					s_mainCamera = Camera.main;
				}
				camera = s_mainCamera;
			}

			// The bottom-left of the camera is (0,0); the top-right is (1,1). The z position is in world units from the camera.
			Vector3 viewportPoint = camera.WorldToViewportPoint(worldPosition);
			float x = Mathf.Clamp01(viewportPoint.x);
			source.panStereo = -1 + (x*2);
		}

		/// <summary>Play a sound in stereo</summary>
		public static AudioSource Play (string clipName, float volume = 1)
		{
			AudioSource audioSource = s_instance.GetAudioSource(clipName, volume);
			if (audioSource != null)
			{
				audioSource.panStereo = 0; // center
				audioSource.spatialBlend = 0;
				audioSource.Play();
			}
			return audioSource;
		}

		/// <summary>Play a sound at a world position</summary>
		public static AudioSource Play (string clipName, Vector3 position, float volume = 1)
		{
			AudioSource audioSource = s_instance.GetAudioSource(clipName, volume);
			if (audioSource != null)
			{
				audioSource.transform.position = position;
				audioSource.spatialBlend = 1;
				audioSource.Play();
			}
			return audioSource;
		}

		private AudioSource GetAudioSource (string clipName, float volume = 1)
		{
			AudioClip audioClip = null;

			if (_audioClipDictionary.TryGetValue(clipName.ToLower(), out audioClip) == false)
			{
				Debug.LogWarning("Could not find audio clip [" + clipName + "]");
				return null;
			}

			AudioSource audioSource = null;

			if (_audioSourcePool.Count == 0)
			{
				GameObject audioObject = new GameObject("AudioSource");
				audioObject.transform.parent = transform;
				audioSource = audioObject.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
				audioSource.loop = false;
			}
			else
			{
				audioSource = _audioSourcePool.Dequeue();
				audioSource.gameObject.SetActive(true);
			}

			audioSource.clip = audioClip;
			audioSource.volume = volume;
			_activeAudioSources.Add(audioSource);

			return audioSource;
		}

		private void ReturnAudioSource (AudioSource audioSource)
		{
			_audioSourcePool.Enqueue(audioSource);
			audioSource.gameObject.SetActive(false);
		}

		private void Awake ()
		{
			if (s_instance)
			{
				Debug.Log("Multiple instances of SimpleSounds detected. Destroying instance on " + gameObject.name);
				Destroy(this);
				return;
			}

			s_instance = this;
			Object[] soundFiles = Resources.LoadAll(RESOURCE_PATH, typeof(AudioClip));
			_audioClipDictionary = new Dictionary<string, AudioClip>();

			for (int i = 0; i < soundFiles.Length; i++)
			{
				AudioClip clip = (AudioClip) soundFiles[i];
				_audioClipDictionary.Add(clip.name.ToLower(), clip);
			}

			_audioSourcePool = new Queue<AudioSource>();
			_activeAudioSources = new List<AudioSource>();
		}

		private void LateUpdate ()
		{
			int count = _activeAudioSources.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				if (_activeAudioSources[i].clip == null ||
					_activeAudioSources[i].isPlaying == false ||
					_activeAudioSources[i].time >= _activeAudioSources[i].clip.length)
				{						
					ReturnAudioSource(_activeAudioSources[i]);
					_activeAudioSources.RemoveAt(i);
				}
			}
		}
	}
}
