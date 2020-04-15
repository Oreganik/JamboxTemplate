using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jambox
{
	// When used as part of a prefab, this allows debug text to quickly be printed to screen.
	public class BasicTextCanvas : MonoBehaviour 
	{
		public enum ScreenPosition
		{
			None,
			BottomCenter,
			BottomLeft,
			BottomRight,
			MiddleCenter,
			MiddleLeft,
			MiddleRight,
			TopCenter,
			TopLeft,
			TopRight
		}

		public static BasicTextCanvas Instance
		{
			get { return _instance; }
		}

		private static BasicTextCanvas _instance;

		#pragma warning disable 0649
		[SerializeField] private Text _bottomCenter;
		[SerializeField] private Text _bottomLeft;
		[SerializeField] private Text _bottomRight;
		[SerializeField] private Text _middleCenter;
		[SerializeField] private Text _middleLeft;
		[SerializeField] private Text _middleRight;
		[SerializeField] private Text _topCenter;
		[SerializeField] private Text _topLeft;
		[SerializeField] private Text _topRight;
		#pragma warning restore 0649

		private Dictionary<ScreenPosition, Text> _screenText;

		public void SetColor (ScreenPosition screenPosition, Color value)
		{
			Text textField = GetTextField(screenPosition);
			if (textField != null)
			{
				textField.color = value;
			}
		}

		public void SetSize (ScreenPosition screenPosition, int value)
		{
			Text textField = GetTextField(screenPosition);
			if (textField != null)
			{
				textField.fontSize = value;
			}
		}

		public void SetText (ScreenPosition screenPosition, string value)
		{
			Text textField = GetTextField(screenPosition);
			if (textField != null)
			{
				textField.text = value;
			}
		}

		public void SetVisible (ScreenPosition screenPosition, bool isVisible, bool forceAlpha = false)
		{
			Text textField = GetTextField(screenPosition);
			if (textField != null)
			{
				textField.gameObject.SetActive(isVisible);
			}
		}

		private Text GetTextField (ScreenPosition screenPosition)
		{
			Text textField = null;
			if (_screenText.TryGetValue(screenPosition, out textField))
			{
				return textField;
			}
			else
			{
				Debug.LogError("Could not find Text field for ScreenPosition." + screenPosition);
				return null;
			}
		}

		private void Awake ()
		{
			_instance = this;

			_screenText = new Dictionary<ScreenPosition, Text>()
			{
				{ ScreenPosition.BottomCenter, _bottomCenter },
				{ ScreenPosition.BottomLeft, _bottomLeft },
				{ ScreenPosition.BottomRight, _bottomRight },
				{ ScreenPosition.MiddleCenter, _middleCenter },
				{ ScreenPosition.MiddleLeft, _middleLeft },
				{ ScreenPosition.MiddleRight, _middleRight },
				{ ScreenPosition.TopCenter, _topCenter },
				{ ScreenPosition.TopLeft, _topLeft },
				{ ScreenPosition.TopRight, _topRight }
			};

			foreach (KeyValuePair<ScreenPosition, Text> pair in _screenText)
			{
				pair.Value.text = string.Empty;
				pair.Value.gameObject.SetActive(false);
			}
		}

	}
}
