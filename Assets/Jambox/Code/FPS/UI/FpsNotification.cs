// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class FpsNotification : MonoBehaviour 
	{
		public TMP_Text _text;
		public float _moveDuration = 0.5f;
		public MotionCurveType _moveCurve;
		public float _visibleDuration = 4;
		public Vector2 _offscreenDirection;

		private bool _finishedMoving;
		private RectTransform _rectTransform;
		private Timer _timer;
		private Vector2 _hiddenPosition;
		private Vector2 _visiblePosition;

		public void Hide ()
		{
			_rectTransform.anchoredPosition = _hiddenPosition;
			gameObject.SetActive(false);
		}

		public void Show (string text)
		{
			gameObject.SetActive(true);
			_finishedMoving = false;
			_rectTransform.anchoredPosition = _hiddenPosition;
			_text.text = text;
			_timer.StartNewTimer(_moveDuration);
		}

		protected void Awake ()
		{
			_timer = new Timer(_moveDuration);
			_rectTransform = GetComponent<RectTransform>();
			_visiblePosition = _rectTransform.anchoredPosition;
			_hiddenPosition = _rectTransform.anchoredPosition + _offscreenDirection * _rectTransform.sizeDelta.x;
		}

		protected void Update ()
		{
			_timer.Update(Time.deltaTime);
			if (_finishedMoving)
			{
				if (_timer.IsComplete)
				{
					Hide();
				}
			}
			else
			{
				_rectTransform.anchoredPosition = Vector2.Lerp(_hiddenPosition, _visiblePosition, MotionCurve.GetCurveForPercentComplete(_timer.t, _moveCurve));
				if (_timer.IsComplete)
				{
					_timer.StartNewTimer(_visibleDuration);
					_finishedMoving = true;
				}
			}
		}
	}
}
