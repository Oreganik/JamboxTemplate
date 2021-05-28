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
	public class HeroItemManager : MonoBehaviour 
	{
		public static HeroItemManager Instance;

		[SerializeField] private HeroItemSource[] _sources;

		public GameObject GetPrefab (string category, string id)
		{
			category = category.ToLower();
			id = id.ToLower();
			foreach (HeroItemSource source in _sources)
			{
				if (source.Category.Equals(category) == false) continue;
				if (source.Id.Equals(id))
				{
					return source.Prefab;
				}
			}
			return null;
		}

		protected void Awake ()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Multuple instances of HeroItemManager. Destroying new one.");
				Destroy(gameObject);
				return;
			}
			Instance = this;
		}
	}
}
