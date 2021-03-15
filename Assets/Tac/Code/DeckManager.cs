// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// 
	/// </summary>
	public class DeckManager : MonoBehaviour 
	{
		public TextAsset[] _deckFiles;

		private Dictionary<string, List<string>> _playerDeckIds;
		private Dictionary<string, Deck> _decks;

		protected void Awake ()
		{
			_playerDeckIds = new Dictionary<string, List<string>>();
			_decks = new Dictionary<string, Deck>();
		}
	}
}
