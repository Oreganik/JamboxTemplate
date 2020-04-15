using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	// TODO:
	// Fade time on stop
	public class GenericSoundPlayer : MonoBehaviour 
	{
		public bool IsPlaying 
		{
			get { return _audioSource.clip != null && _audioSource.time < _audioSource.clip.length; }
		}

		private AudioSource _audioSource;

		public static float GetPanStereo (Camera camera, Vector3 worldPosition)
		{
			// The bottom-left of the camera is (0,0); the top-right is (1,1). The z position is in world units from the camera.
			Vector3 viewportPoint = camera.WorldToViewportPoint(worldPosition);
			float x = Mathf.Clamp01(viewportPoint.x);
			return -1 + (x*2);
		}

		public void Play (AudioClip clip, float volume = 1, float pitch = 1)
		{
			PlayStereo(clip, 0, volume, pitch);
		}

		/// <summary>
		/// Plays a clip in stereo 2D
		/// </summary>
		/// <param name="clip"></param>
		/// <param name="panStereo">-1 (left) to 1 (right)</param>
		/// <param name="volume">Defaults to 1</param>
		public void PlayStereo (AudioClip clip, float panStereo = 0, float volume = 1, float pitch = 1)
		{
			_audioSource.panStereo = panStereo;
			_audioSource.spatialBlend = 0;
			PlayClip(clip, volume, pitch);
		}

		public void PlayWorld (AudioClip clip, Vector3 position, float volume = 1, float pitch = 1)
		{
			transform.position = position;
			_audioSource.spatialBlend = 1;
			PlayClip(clip, volume, pitch);
		}

		public void Stop ()
		{
			_audioSource.Stop();
			_audioSource.clip = null;
			enabled = false;
		}

		private void PlayClip (AudioClip clip, float volume = 1, float pitch = 1)
		{
			_audioSource.pitch = pitch;
			_audioSource.volume = volume;
			_audioSource.clip = clip;
			_audioSource.Play();
			_audioSource.clip = clip;
			enabled = true;
		}

		private void Awake ()
		{
			_audioSource = gameObject.AddComponent<AudioSource>();
			_audioSource.loop = false;
			_audioSource.playOnAwake = false;
		}

		private void Update ()
		{
			if (IsPlaying == false)
			{
				Stop();
			}
		}
	}
}
