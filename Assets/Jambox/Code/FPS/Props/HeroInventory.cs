// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class HeroInventory
	{
		// item
		// how many do they have
		// how much can they carry -> Affected by stats
		// how is an item different than a stat
		// - item can be dropped
		// - item can affect stats, not vice versa (but they can come together outside of inventory)

		private Dictionary<string, int> _itemCount;
		private Dictionary<string, int> _itemMaxCount;

		public HeroInventory ()
		{
			_itemCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			_itemMaxCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		}

		public int Add (string key, int amount)
		{
			if (amount == 0) Debug.LogWarningFormat("Adding a zero value to key {0}", key);
			if (amount < 0) Debug.LogWarningFormat("Adding a negative value {0} to key {1}. This will subtract.", amount.ToString(), key);
			return Adjust(key, amount);
		}

		public int Adjust (string key, int amount)
		{
			if (amount == 0) Debug.LogWarningFormat("Adjusting key {0} by zero", key);
			int previousValue = 0;
			if (_itemCount.TryGetValue(key, out previousValue))
			{
				return Set(key, previousValue + amount);
			}
			return Set(key, amount);
		}

		public int Get (string key)
		{
			int value = 0;
			// Determine max value. If no max has been set, it's maximum int.
			if (_itemMaxCount.TryGetValue(key, out value))
			{
				return value;
			}
			Debug.LogWarningFormat("No entry found for key {0}", key);
			return 0;
		}

		public int GetMax (string key)
		{
			int max = 0;
			// Determine max value. If no max has been set, it's maximum int.
			if (_itemMaxCount.TryGetValue(key, out max))
			{
				return max;
			}
			return int.MaxValue;
		}

		public bool Has (string key)
		{
			return Get(key) > 0;
		}

		public int Remove (string key, int amount)
		{
			if (amount == 0) Debug.LogWarningFormat("Removing a zero value to key {0}", key);
			if (amount < 0) Debug.LogWarningFormat("Removing a negative value {0} to key {1}. This will add.", amount.ToString(), key);
			return Adjust(key, -amount);
		}

		public int Set (string key, int amount)
		{
			int value = Mathf.Clamp(amount, 0, GetMax(key));
			if (_itemCount.ContainsKey(key))
			{
				_itemCount[key] = value;
			}
			else
			{
				_itemCount.Add(key, value);
			}
			return value;
		}

		public int SetMax (string key, int maxValue)
		{
			int max = Mathf.Clamp(maxValue, 0, int.MaxValue);
			if (_itemMaxCount.ContainsKey(key))
			{
				_itemMaxCount[key] = max;
			}
			else
			{
				_itemMaxCount.Add(key, max);
			}
			return max;
		}

		public void SetToMax (string key)
		{
			Set(key, GetMax(key));
		}
	}
}
