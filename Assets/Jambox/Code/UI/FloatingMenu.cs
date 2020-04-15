// JAMBOX
// Copyright (c) 2019 Oreganik LLC
// Author: Ted Brown

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class FloatingMenu : MonoBehaviour 
	{
		public Action<int> OnMenuButtonClick;

		public bool IsOpen
		{
			get { return gameObject.activeInHierarchy; }
		}

		[Tooltip("Not required if set by another class")]
		// Buttons must be anchored in the top left
		[SerializeField] private GameObject _buttonPrefab;

		private Canvas _canvas;
		private Dictionary<int, FloatingMenuButton> _buttons;
		private RectTransform _rectTransform;
		private Vector2 _buttonSize;

		public void AddButton (string name, int value)
		{
			if (_buttonPrefab == null)
			{
				Debug.LogError("Button prefab is null on " + gameObject.name, gameObject);
				return;
			}

			// Expand the menu size. Used to keep the menu on screen.
			_rectTransform.sizeDelta = new Vector2(_buttonSize.x, _buttonSize.y * (_buttons.Count + 1));

			RectTransform newButtonRectTransform = Instantiate(_buttonPrefab, _rectTransform).GetComponent<RectTransform>();
			newButtonRectTransform.localPosition = new Vector3(0, -_buttonSize.y * _buttons.Count, 0);

			FloatingMenuButton button = newButtonRectTransform.GetComponent<FloatingMenuButton>();
			button.Initialize(this, name, value);

			_buttons.Add(value, button);

			// TODO: Adjust screen position to ensure options aren't left off-screen
		}

		public void ClearButtons ()
		{
			List<int> keys = new List<int>(_buttons.Keys);
			foreach (int key in keys)
			{
				Destroy(_buttons[key].gameObject);
				_buttons.Remove(key);
			}
			_rectTransform.sizeDelta = Vector2.zero;
		}

		public void HandleButtonClick (int value)
		{
			if (OnMenuButtonClick != null)
			{
				OnMenuButtonClick(value);
			}
		}

		public void Hide ()
		{
			gameObject.SetActive(false);
		}

		public void SetButtonPrefab (GameObject prefab)
		{
			_buttonPrefab = prefab;
			_buttonSize = _buttonPrefab.GetComponent<RectTransform>().sizeDelta;
		}

		public void SetName (int id, string name)
		{
			FloatingMenuButton button = null;
			if (_buttons.TryGetValue(id, out button))
			{
				button.SetName(name);
			}
		}

		public void Show (Vector2 screenPosition) // typically receives input from mousePosition
		{
			MoveToPosition(screenPosition);
			gameObject.SetActive(true);
		}

		private void MoveToPosition (Vector2 screenPosition)
		{
			// Screen Space - Overlay
			if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				// When a RectTransform.position is set to a screen position, the canvas scale factor is automatically taken into account.
				// Unfortunately, this totally messes with any calculations that try to keep the menu and its buttons within the bounds of the screen.
				// (huge thanks to https://forum.unity.com/threads/overlay-canvas-and-world-space-coordinates.291108/)

				// If you divide screen position by canvas.scaleFactor, you will get a value equal to what RectTransform.position will be.
				// But we're more interested in the menu size, so we'll modify that.
				Vector2 menuScreenSize = _rectTransform.sizeDelta * _canvas.scaleFactor;

				if (screenPosition.x + menuScreenSize.x > Screen.width)
				{
					screenPosition.x = Screen.width - menuScreenSize.x;
				}

				if (screenPosition.y - menuScreenSize.y < 0)
				{
					screenPosition.y = menuScreenSize.y;
				}

				_rectTransform.position = screenPosition;
			}
			// Screen Space - Camera
			else if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				// https://forum.unity.com/threads/mouse-position-for-screen-space-camera.294458/
				Vector3 pos = screenPosition;
				pos.z = 100; // I'm not sure where this actual value comes from, but it gives the expected results. (tbrown)
				_rectTransform.position = _canvas.worldCamera.ScreenToWorldPoint(pos);
			}
		}

		protected void Awake ()
		{
			// First: get a reference to this canvas
			_canvas = gameObject.GetComponentInParent<Canvas>();

			// Set the anchor in the top left
			_rectTransform = GetComponent<RectTransform>();
			_rectTransform.anchorMin = new Vector2(0, 1);
			_rectTransform.anchorMax = new Vector2(0, 1);
			_rectTransform.pivot = new Vector2(0, 1);

			if (_buttonPrefab != null)
			{
				SetButtonPrefab(_buttonPrefab);
			}

			_buttons = new Dictionary<int, FloatingMenuButton>();
			Hide();
		}
	}
}
