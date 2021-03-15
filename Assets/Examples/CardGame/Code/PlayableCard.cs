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
	public class PlayableCard : MonoBehaviour 
	{
		public bool IsMoving
		{
			get { return _isMoving; }
		}

		public Text _info;

		private bool _isMoving;
		private Sequence _card;
		private string _baseText;
		private RectTransform _rectTransform;
		private Timer _timer;
		private Vector2 _startPosition;
		private Vector2 _targetPosition;

		public void MoveToPosition (Vector2 position, float duration)
		{
			_timer.StartNewTimer(duration);
			_startPosition = _rectTransform.anchoredPosition;
			_targetPosition = position;
			_isMoving = true;
			enabled = true;
		}

		public void SetCard (Sequence card)
		{
			_card = card;
			string effects = string.Empty;

			foreach (Effect effect in _card.Effects)
			{
				effects += effect.ToString() + "\n";
			}

			_info.text = string.Format(_baseText, card.LocalizedName.ToUpper(), card.Cost.ToString(), effects);
		}

		protected void Awake ()
		{
			_baseText = _info.text;
			_timer = new Timer(0);
			_rectTransform = GetComponent<RectTransform>();
			enabled = false;
		}

		protected void Update ()
		{
			if (_isMoving)
			{
				_timer.Update(Time.deltaTime);
				_rectTransform.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, _timer.t);
				if (_timer.IsComplete)
				{
					_isMoving = false;
					enabled = false;
				}
			}
		}
	}
}
