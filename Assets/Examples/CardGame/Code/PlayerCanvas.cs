// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tac
{
	/// <summary>
	/// 
	/// </summary>
	public class PlayerCanvas : MonoBehaviour 
	{
		private const float MOVE_CARDS_DURATION = 0.5f;

		public bool IsBusy
		{
			get { return _isBusy; }
		}

		public GameObject _playableCardPrefab;
		public Text _name;
		public Text _health;
		public Text _deck;
		public RectTransform _cardOrigin;
		public RectTransform _leftHandExtent;
		public RectTransform _rightHandExtent;

		private bool _isBusy;
		private List<PlayableCard> _playableCards;
		private Entity _player;
		private string _healthText;
		private string _deckText;
		private Timer _moveCardsTimer;

		public void HandleDrawCard (Sequence card)
		{
			PlayableCard playableCard = Instantiate(_playableCardPrefab, transform).GetComponent<PlayableCard>();
			playableCard.GetComponent<RectTransform>().anchoredPosition = _cardOrigin.anchoredPosition;
			playableCard.SetCard(card);
			_playableCards.Add(playableCard);
			float count = _playableCards.Count;
			float totalDistance =  Vector2.Distance(_leftHandExtent.anchoredPosition, _rightHandExtent.anchoredPosition);
			float offset = (totalDistance / (count + 1));

			for (int i = 0; i < count; i++)
			{
				_playableCards[i].MoveToPosition(_leftHandExtent.anchoredPosition + Vector2.right * offset * (i + 1), MOVE_CARDS_DURATION);
			}

			_isBusy = true;
		}

		public void SetPlayer (Entity player, int id)
		{
			_player = player;
			_name.text = "PLAYER " + id;
			_health.text = string.Format(_healthText, _player.GetValue(AttributeKey.Health).ToString());
			_deck.text = string.Format(_deckText, _player.Deck.RemainingCards.ToString());

			_player.OnDrawCard += HandleDrawCard;
		}

		protected void Awake ()
		{
			_healthText = _health.text;
			_deckText = _deck.text;
			_playableCards = new List<PlayableCard>();
			_moveCardsTimer = new Timer(MOVE_CARDS_DURATION);
			_moveCardsTimer.FinishNow();
		}

		protected void OnDestroy ()
		{
			if (_player != null)
			{
				_player.OnDrawCard -= HandleDrawCard;
			}
		}

		protected void Update ()
		{
			if (_player == null) return;
			_health.text = string.Format(_healthText, _player.GetValue(AttributeKey.Health).ToString());
			_deck.text = string.Format(_deckText, _player.Deck.RemainingCards.ToString());

			if (_isBusy)
			{
				bool cardsAreMoving = false;
				foreach (PlayableCard card in _playableCards)
				{
					if (card.IsMoving)
					{
						cardsAreMoving = true;
						break;
					}
				}
				_isBusy = cardsAreMoving;
			}
		}
	}
}
