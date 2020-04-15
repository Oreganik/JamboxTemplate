using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class Sounds : MonoBehaviour 
	{
		public static Sounds Instance;

		#pragma warning disable 0649
		[Tooltip("Holds references to all of your SoundPlayer prefabs")]
		[SerializeField] SoundPlayer[] _soundPlayerPrefabs;
		#pragma warning restore 0649

		private Dictionary<EventType, GameObject> _cachedPrefabs;
		private Dictionary<EventType, List<SoundPlayer>> _soundPool;

		private SoundPlayer GetSoundPlayer (EventType eventType)
		{
			List<SoundPlayer> pool = new List<SoundPlayer>();
			SoundPlayer soundPlayer = null;

			// See if an inactive instance exists
			if (_soundPool.TryGetValue(eventType, out pool))
			{
				foreach (SoundPlayer player in pool)
				{
					if (player.IsPlaying == false)
					{
						soundPlayer = player;
						break;
					}
				}
			}

			// If it's still null, add a new one to the pool
			if (soundPlayer == null)
			{
				GameObject prefab = null;
				if (_cachedPrefabs.TryGetValue(eventType, out prefab))
				{
					GameObject newObject = Instantiate(prefab, transform);
					soundPlayer = newObject.GetComponent<SoundPlayer>();
					pool.Add(soundPlayer);
				}
				else
				{
					Debug.LogWarning("No prefab found for EventType." + eventType.ToString());
				}
			}

			return soundPlayer;
		}

		private void Awake ()
		{
			Instance = this;

			_soundPool = new Dictionary<EventType, List<SoundPlayer>>();

			// Build the cached dictionary of prefab references

			_cachedPrefabs = new Dictionary<EventType, GameObject>();

			for (int i = 0; i < System.Enum.GetValues(typeof(EventType)).Length; i++)
			{
				EventType eventType = (EventType) i;
				bool foundEventType = false;

				// Find the prefab associated with this event.
				foreach (SoundPlayer soundPlayer in _soundPlayerPrefabs)
				{
					if (soundPlayer.EventType == eventType)
					{
						if (foundEventType)
						{
							Debug.LogError("Ignoring duplicate entry for EventType." + eventType.ToString() + ": " + soundPlayer.gameObject.name);
						}
						else
						{
							_cachedPrefabs.Add(eventType, soundPlayer.gameObject);
							foundEventType = true;
						}
					}
				}
			}
		}
	}
}
