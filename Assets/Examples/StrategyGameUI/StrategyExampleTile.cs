// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox.Examples
{
	public class StrategyExampleTile : MonoBehaviour, IHoverTarget
	{
		public Color _darkColor;
		public Color _lightColor;

		private Color _baseColor;
		private Color _hoverColor;
		private Coordinate _coordinate;

		private static MaterialPropertyBlock s_propertyBlock;
		private Renderer _renderer;

		#region IHoverTarget
		public Transform GetTransform ()
		{
			return transform;
		}

		public void HandleHover (Pointer pointer)
		{
			SetColor(_hoverColor);
		}

		public void HandleUnhover (Pointer pointer)
		{
			SetColor(_baseColor);
		}
		#endregion // IHoverTarget

		// Using 0,0,0 as the origin, offset x,0,z based on coordinate
		public void SetCoordinate (Coordinate coordinate)
		{
			_coordinate = coordinate;
			transform.position = coordinate.WorldPosition;

			// Set "dark" or "light" based on whether or not the coordinate id is even or odd
			bool evenRow = coordinate.Y % 2 == 0;
			bool isDark = false;
			if (coordinate.Id % 2 == 0)
			{
				isDark = evenRow;
			}
			else
			{
				isDark = !evenRow;
			}

			if (isDark)
			{
				_baseColor = _darkColor;
				_hoverColor = Color.cyan;
				_hoverColor.r *= 0.7f;
				_hoverColor.g *= 0.7f;
				_hoverColor.b *= 0.7f;
			}
			else
			{
				_baseColor = _lightColor;
				_hoverColor = Color.cyan;
			}

			SetColor(_baseColor);
		}

		private void SetColor (Color color)
		{
			_renderer.GetPropertyBlock(s_propertyBlock);
			s_propertyBlock.SetColor("_Color", color);
			_renderer.SetPropertyBlock(s_propertyBlock);
		}

		protected void Awake ()
		{
			if (s_propertyBlock == null)
			{
				s_propertyBlock = new MaterialPropertyBlock();
			}

			_renderer = GetComponentInChildren<Renderer>();
		}
	}
}
