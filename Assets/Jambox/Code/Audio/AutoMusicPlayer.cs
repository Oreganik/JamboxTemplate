// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	// Add this to a game object with an AudioSource
	public class AutoMusicPlayer : MonoBehaviour
	{
		private enum State { Stop, FadeIn, Play, FadeOut }

		public bool IsPlaying { get { return _state != State.Stop; } }
		public bool IsStopped { get { return _state == State.Stop; }}

		#pragma warning disable 0649
		[Range(0, 10)]
		[SerializeField] private float _fadeInDuration;
		[Range(0, 10)]
		[SerializeField] private float _defaultFadeOutDuration;
		#pragma warning restore 0649
		
		private AudioSource _audioSource;
		private float _peakVolume;
		private State _state;
		private Timer _timer;

		public void FadeOut (float duration = 0)
		{
			if (duration <= 0)
			{
				duration = _defaultFadeOutDuration;
			}

			_timer.StartNewTimer(duration);
			_state = State.FadeOut;
		}

		public void Stop ()
		{
			_audioSource.Stop();
			_state = State.Stop;
		}

		protected void Awake ()
		{
			_audioSource = GetComponent<AudioSource>();

			if (_audioSource == null || _audioSource.clip == null)
			{
				Debug.LogError(gameObject.name + " has no audio source OR audio source has no clip", gameObject);
				enabled = false;
				return;
			}

			_peakVolume = _audioSource.volume;

			// If fade in duration is zero, volume will be set to 1 on the first update frame
			_timer = new Timer(_fadeInDuration);
			_state = State.FadeIn;
			_audioSource.volume = 0;
			_audioSource.Play();
		}

		protected void Update ()
		{
			if (_state == State.FadeIn)
			{
				_timer.Update(Time.deltaTime);
				_audioSource.volume = _peakVolume * _timer.t;
				if (_timer.IsComplete)
				{
					_state = State.Play;
				}
			}
			else if (_state == State.Play)
			{
				if (_audioSource.isPlaying && _audioSource.loop == false && _audioSource.time >= _audioSource.clip.length)
				{
					Stop();
				}
			}
			else if (_state == State.FadeOut)
			{
				_timer.Update(Time.deltaTime);
				_audioSource.volume = _peakVolume * (1 - _timer.t);
				if (_timer.IsComplete)
				{
					Stop();
				}
			}
		}

	}
}
