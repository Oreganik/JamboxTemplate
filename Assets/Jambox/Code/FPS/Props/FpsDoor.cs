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
	public class FpsDoor : MonoBehaviour 
	{
		private enum State { Closed, Opening, Open, Closing }

		public FpsTrigger _trigger;
		public Vector3 _localDirection;
		public float _moveDistance = 1;
		public float _openDuration = 1;
		public MotionCurveType _openCurve = MotionCurveType.Linear;
		public float _closeDuration = 1;
		public MotionCurveType _closeCurve = MotionCurveType.Linear;

		private State _state;
		private Timer _timer;
		private Vector3 _closedPosition;
		private Vector3 _openPosition;

		public void HandleInteract ()
		{
			if (_state == State.Open)
			{
				_timer.StartNewTimer(_closeDuration);
				_state = State.Closing;
				enabled = true;
			}
			else if (_state == State.Closed)
			{
				_timer.StartNewTimer(_openDuration);
				_state = State.Opening;
				enabled = true;
			}
		}

		protected void Awake ()
		{
			_trigger.OnInteract += HandleInteract;
			_closedPosition = transform.position;
			_openPosition = transform.position + transform.TransformVector(_localDirection) * _moveDistance;
			_timer = new Timer(1);
			enabled = false;
		}

		protected void OnDestroy ()
		{
			if (_trigger)
			{
				_trigger.OnInteract -= HandleInteract;
			}
		}

		protected void Update ()
		{
			_timer.Update(Time.deltaTime);
			if (_state == State.Closing)
			{
				transform.position = Vector3.Lerp(_openPosition, _closedPosition, MotionCurve.GetCurveForPercentComplete(_timer.t, _closeCurve));
				if (_timer.IsComplete)
				{
					_state = State.Closed;
					enabled = false;
				}
			}
			else if (_state == State.Opening)
			{
				transform.position = Vector3.Lerp(_closedPosition, _openPosition, MotionCurve.GetCurveForPercentComplete(_timer.t, _openCurve));
				if (_timer.IsComplete)
				{
					_state = State.Open;
					enabled = false;
				}
			}
		}
	}
}
