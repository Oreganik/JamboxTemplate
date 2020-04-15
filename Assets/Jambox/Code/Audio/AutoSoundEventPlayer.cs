using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// Loads all sound files in Resources/EventSounds. 
	/// When an EventType is broadcast, if this find an audioclip exactly matching the name (case ignored)s, it is played in stereo.
	/// </summary>

	// TODO: 
	// - Support multiple sounds per event with underscore numbers

	public class AutoSoundEventPlayer : MonoBehaviour 
	{
		// The full path is Resources/RESOURCE_FOLDER
		const string RESOURCE_FOLDER = "EventSounds";

		private AudioClip[] _audioClips;
		private List<GenericSoundPlayer> _soundPlayers;

		private GenericSoundPlayer GetSoundPlayer ()
		{
			GenericSoundPlayer soundPlayer = null;

			int count = _soundPlayers.Count;

			for (int i = count - 1; i >= 0; i--)
			{
				if (_soundPlayers[i] == null)
				{
					_soundPlayers.RemoveAt(i);
				}
				else if (_soundPlayers[i].IsPlaying == false)
				{
					soundPlayer = _soundPlayers[i];
					break;
				}
			}

			if (soundPlayer == null)
			{
				GameObject newPlayerObject = new GameObject("SoundPlayer");
				newPlayerObject.transform.parent = transform;
				soundPlayer = newPlayerObject.AddComponent<GenericSoundPlayer>();
			}

			return soundPlayer;
		}

		/// <summary>
		/// Handles the event.
		/// </summary>
		/// <param name="eventType">Event type.</param>
		/// <param name="data">A Vector3 of the world position of the event. If ignored, will be Vector3 zero.</param>
		private void HandleEvent (EventType eventType, object data = null)
		{
			string eventString = eventType.ToString().ToLower();
			AudioClip eventClip = null;

			foreach (AudioClip clip in _audioClips)
			{
				if (clip.name.ToLower().Equals(eventString))
				{
					eventClip = clip;
					break;
				}
			}

			// If this is null, we did not find a match.
			if (eventClip == null)
			{
				return;
			}

			Vector3 worldPosition = data == null ? Vector3.zero : (Vector3) data;
			float panStereo = GenericSoundPlayer.GetPanStereo(Camera.main, worldPosition);

			GenericSoundPlayer soundPlayer = GetSoundPlayer();
			soundPlayer.PlayStereo(eventClip, panStereo);
		}

		private void Awake ()
		{
			Object[] soundFiles = Resources.LoadAll(RESOURCE_FOLDER, typeof(AudioClip));

			_audioClips = new AudioClip[soundFiles.Length];

			for (int i = 0; i < soundFiles.Length; i++)
			{
				_audioClips[i] = (AudioClip) soundFiles[i];
			}

			_soundPlayers = new List<GenericSoundPlayer>();
		}

		private void OnDestroy ()
		{
			if (Events.Instance)
			{
				Events.Instance.OnEventTriggered -= HandleEvent;
			}
		}

		private void Start ()
		{
			Events.Instance.OnEventTriggered += HandleEvent;
		}
	}
}