// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using UnityEngine;

namespace Jambox
{
	public class ScreenFader : MonoBehaviour
	{
		public enum State
		{
			Clear,
			FadeOut, // fade out to solid
			Solid,
			FadeIn	// fade in to clear
		}

	    public static ScreenFader Instance { get; private set; }

		public bool IsCanvasVisible
		{
			get { return _canvasVisible; }
		}

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

		private bool _canvasVisible = false;
		private Color _beginColor;
		private Color _endColor;
		private Material _fadeMaterial;
		private State _state;
		private Timer _timer;

		public void Clear ()
		{
			enabled = false;
			_state = State.Clear;
		}

		public void FadeInFromColor (Color color, float duration)
		{
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
			_fadeMaterial.color = color;
			enabled = true;
			_state = State.Solid;
		}

		public void SetCanvasHidden ()
		{
			_canvasVisible = false;
		}

		public void SetCanvasVisible ()
		{
			_canvasVisible = true;
		}

		private void Awake()
		{
			Instance = this;
			_fadeMaterial = new Material(Shader.Find("Sprites/Diffuse"));
			this.enabled = false;
		}

		private void Update ()
		{
			_timer.Update(Time.deltaTime);

			_fadeMaterial.color = Color.Lerp(_beginColor, _endColor, _timer.t);

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

		private void OnDestroy()
		{
			if (_fadeMaterial != null)
			{
				Destroy(_fadeMaterial);
			}
		}

	    // Renders the fade overlay when attached to a camera object.
	    // Script must be enabled for this to work.
	    private IEnumerator OnPostRender()
		{
			if (_canvasVisible == false)
			{
				yield return new WaitForEndOfFrame();
			}

			_fadeMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Color(_fadeMaterial.color);
			GL.Begin(GL.QUADS);
			GL.Vertex3(0f, 0f, -12f);
			GL.Vertex3(0f, 1f, -12f);
			GL.Vertex3(1f, 1f, -12f);
			GL.Vertex3(1f, 0f, -12f);
			GL.End();
			GL.PopMatrix();
		}
	}
}
