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
	/// Stats are numeric qualities like Health and Armor.
	/// Most entities (like enemies and the hero) have stats.
	/// This component manages access to FpsStat objects.
	/// Enums are converted to strings to keep things flexible for prototyping.
	/// </summary>
	public class FpsStats : MonoBehaviour 
	{
		[Serializable]
		/// <summary>Used to add stat characteristics to this object.</summary>
		public class BaseStat
		{
			public string Id;
			public int Base;
			public int Min;
			[Tooltip("A max of zero or less means it is unlimited")]
			public int Max;
		}

		[Tooltip("Automatically adds these stats. Stats can also be added by other scripts.")]
		public List<BaseStat> _baseStats;

		// This is a list so it can be quickly checked in the inspector during the prototyping phase.
		// Convert this to a dictionary for an actual project.
		private List<FpsStat> _stats;

		public bool Add (FpsStat stat)
		{
			if (Has(stat.Id))
			{
				Debug.LogErrorFormat("Object {0} already has a stat with ID {1}", gameObject.name, stat.Id);
				return false;
			}
			_stats.Add(stat);
			//Debug.LogFormat("{0}: Add {1}", gameObject.name, stat.ToString());
			return true;
		}

		public void Adjust (Enum id, int amount)
		{
			Adjust(id.ToString(), amount);
		}

		public void Adjust (string id, int amount)
		{
			FpsStat stat = GetStat(id);
			if (stat == null)
			{
				Debug.LogErrorFormat("Object {0} does not have a stat with ID {1}", gameObject.name, id);
				return;
			}
			stat.Value += amount;
		}

		public int Get (Enum id)
		{
			return Get(id.ToString());
		}

		public int Get (string id)
		{
			foreach (FpsStat stat in _stats)
			{
				if (stat.Id.Equals(id))
				{
					return stat.Value;
				}
			}
			Debug.LogErrorFormat("Object {0} does not have a stat with ID '{1}'", gameObject.name, id);
			return -1;
		}

		public FpsStat GetStat (Enum id)
		{
			return GetStat(id.ToString());
		}

		public FpsStat GetStat (string id)
		{
			foreach (FpsStat stat in _stats)
			{
				if (stat.Id.Equals(id))
				{
					return stat;
				}
			}
			Debug.LogErrorFormat("Object {0} does not have a stat with ID '{1}'", gameObject.name, id);
			return null;
		}

		public bool Has (Enum id)
		{
			return Has(id.ToString());
		}

		public bool Has (string id)
		{
			foreach (FpsStat stat in _stats)
			{
				if (stat.Id.Equals(id))
				{
					return true;
				}
			}
			return false;
		}

		public void Reset (Enum id)
		{
			Reset(id.ToString());
		}

		public void Reset (string id)
		{
			FpsStat stat = GetStat(id);
			if (stat == null)
			{
				Debug.LogErrorFormat("Object {0} does not have a stat with ID '{1}'", gameObject.name, id);
				return;
			}
			stat.Reset();
		}

		protected void Awake ()
		{
			_stats = new List<FpsStat>();
			foreach (BaseStat baseStat in _baseStats)
			{
				_stats.Add(new FpsStat(baseStat.Id, baseStat.Base, baseStat.Max, baseStat.Min));
			}
		}
	}
}
