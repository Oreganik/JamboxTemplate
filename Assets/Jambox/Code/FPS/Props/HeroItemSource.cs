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
	/// Data file that links an id with a prefab.
	/// </summary>
	[CreateAssetMenu(fileName = "NewHeroItemSource", menuName = "Jambox/Hero Item Source", order = 1)]
	public class HeroItemSource : ScriptableObject 
	{
		public string Category
		{
			get { return _category.ToLower(); }
		}

		public string Id
		{
			get { return _id.ToLower(); }
		}

		public string Name
		{
			get { return _name; }
		}

		public GameObject Prefab
		{
			get { return _prefab; }
		}

		public GameObject EditorPrefab
		{
			get { return _editorPrefab; }
		}

		#pragma warning disable 0649
		[SerializeField] private string _category;
		[Tooltip("A unique identifier for this source. Once placed in a level, probably should not be changed.")]
		[SerializeField] private string _id;
		[Tooltip("An accessible name for this. Can be changed as necessary.")]
		[SerializeField] private string _name;
		[Tooltip("Used in game")]
		[SerializeField] private GameObject _prefab;
		[Tooltip("Used in editor")]
		[SerializeField] private GameObject _editorPrefab;
		#pragma warning restore 0649
	}
}
