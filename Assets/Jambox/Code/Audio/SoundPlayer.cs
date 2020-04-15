using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour 
	{
		public bool IsPlaying 
		{
			get { return _audioSource.clip != null && _audioSource.time < _audioSource.clip.length; }
		}

		public EventType EventType
		{
			get { return _eventType; }
		}

		#pragma warning disable 0649
		[SerializeField] private AudioClip[] _audioClips;
		[SerializeField] private EventType _eventType;
		#pragma warning restore 0649

		// TODO: Range type (low, high) for pitch variation

		private AudioSource _audioSource;

		public void Play ()
		{
			_audioSource.Play();
		}

		private void Awake ()
		{
			_audioSource = GetComponent<AudioSource>();
		}
	}
}
