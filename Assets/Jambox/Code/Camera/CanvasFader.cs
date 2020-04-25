// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jambox
{
	public class CanvasFader : MonoBehaviour 
	{
		public enum State
		{
			Clear,
			FadeOut, // fade out to solid
			Solid,
			FadeIn	// fade in to clear
		}

	    public static CanvasFader Instance { get; private set; }

		public bool IsClear
		{
			get { return _state == State.Clear; }
		}

		public bool IsSolid
		{
			get { return _state == State.Solid; }
		}

	    /// <summary>
	    /// True if Screenfader is going from clear to solid or vice-versa
	    /// </summary>
	    public bool IsFading 
		{ 
			get { return _state == State.FadeOut || _state == State.FadeIn; }
		}

		public bool ClearOnAwake = true;

		private Color _beginColor;
		private Color _endColor;
		private Image _image;
		private State _state;
		private Timer _timer;

		public void Clear ()
		{
			//Debug.Log("clear");
			_image.color = Color.clear;
			enabled = false;
			_state = State.Clear;
			_beginColor = Color.clear;
			_endColor = Color.clear;

		}

		public void FadeInFromColor (Color color, float duration)
		{
			//Debug.Log("fade in");
			if (duration <= 0)
			{
				Clear();
				return;
			}

			_beginColor = color;
			_endColor = new Color(color.r, color.g, color.b, 0);
			_timer = new Timer(duration);
			enabled = true;
			_state = State.FadeIn;
		}

		public void FadeOutToColor(Color color, float duration)
		{
			//Debug.Log("fade out");
			if (duration <= 0)
			{
				SetSolid(color);
				return;
			}

			_beginColor = new Color(color.r, color.g, color.b, 0);
			_endColor = color;
			_timer = new Timer(duration);
			enabled = true;
			_state = State.FadeOut;
		}

		public void SetSolid (Color color)
		{
			Debug.Log("set solid " + color);
			_beginColor = color;
			_endColor = color;
			_image.color = color;
			enabled = false;
			_state = State.Solid;
		}

		private void Awake()
		{
			Instance = this;
			_image = GetComponent<Image>();
			_image.raycastTarget = false;
			if (ClearOnAwake)
			{
				Clear();
			}
			else
			{
				this.enabled = false;
			}
		}

		private void Update ()
		{
			_timer.Update(Time.deltaTime);

			_image.color = Color.Lerp(_beginColor, _endColor, _timer.t);

			if (_timer.IsComplete)
			{
				if (_endColor.a == 0)
				{
					Clear();
				}
				else
				{
					SetSolid(_endColor);
				}
			}
		}
	}
}
