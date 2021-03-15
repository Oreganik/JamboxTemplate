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
	/// Runs a game between two players
	/// </summary>
	public class Session : MonoBehaviour 
	{
		public static Session Instance;

		public Entity ActivePlayer
		{
			get { return _players[_activePlayerId]; }
		}

		public Entity OtherPlayer
		{
			get { return _players[_otherPlayerId]; }
		}

		public TextAsset _cardData;
		public TextAsset _entityData;
		public TextAsset[] _deckFiles;

		private Entity[] _players;
		private int _activePlayerId;
		private int _otherPlayerId;

		public void GoToNextPlayer ()
		{
			// increase id by 1 and then mod by player length.
			// this ensures the id resets to zero after the last player has played.
			_activePlayerId = ++_activePlayerId % _players.Length;
			_otherPlayerId = ++_otherPlayerId % _players.Length;
		}

		public bool Initialize ()
		{
			bool error = false;

			CardManager.Initialize(_cardData);

			if (CardManager.ErrorOnLoad)
			{
				Debug.LogError("CardManager: Error On Load");
				error = true;
			}

			EntityManager.Initialize(_entityData);

			if (EntityManager.ErrorOnLoad)
			{
				Debug.LogError("EntityManager: Error On Load");
				error = true;
			}
			
			_players = new Entity[]
			{
				EntityManager.Spawn("player", null),
				EntityManager.Spawn("player", null)
			};

			for (int i = 0; i < _players.Length; i++)
			{
				if (_players[i].Deck.LoadFromTextAsset(_deckFiles[i]) == false)
				{
					Debug.LogError("Deck: Error On Load");
					error = true;
				}
			}
			
			for (int i = 0; i < _players.Length; i++)
			{
				Hud.Instance.SetPlayer(i, _players[i]);
			}

			return (error == false);
		}

		protected void Awake ()
		{
			Instance = this;
			_activePlayerId = 0;
			_otherPlayerId = 1;
		}
	}
}
