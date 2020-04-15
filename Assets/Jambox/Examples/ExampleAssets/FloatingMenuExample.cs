// JAMBOX
// Copyright (c) 2019 Oreganik LLC
// Author: Ted Brown

using UnityEngine;
using UnityEngine.UI;

namespace Jambox.Examples 
{
	public class FloatingMenuExample : MonoBehaviour 
	{
		private enum Buttons
		{
			Hug,
			Kiss,
			Punch,
			Kick
		}

		#pragma warning disable 0649
		[SerializeField] private Canvas _canvas;
		[SerializeField] private GameObject _buttonPrefab;
		[SerializeField] private Text _resultText;
		#pragma warning restore 0649

		private FloatingMenu _dropDownMenu;

		public void HandleButtonClick (int id)
		{
			_resultText.text = ((Buttons)id).ToString();
		}

		private void Awake ()
		{
			if (_canvas == null)
			{
				Debug.LogError("Canvas is null");
				return;
			}

			if (_buttonPrefab == null)
			{
				Debug.LogError("Button prefab is null.");
				return;
			}

			// Create and build the menu
			GameObject menuObject = new GameObject("DropDownMenu", typeof(RectTransform));
			menuObject.transform.SetParent(_canvas.transform);
			_dropDownMenu = menuObject.AddComponent<FloatingMenu>();
			_dropDownMenu.SetButtonPrefab(_buttonPrefab);

			// Add the buttons
			for (int i = 0; i < System.Enum.GetValues(typeof(Buttons)).Length; i++)
			{
				_dropDownMenu.AddButton(((Buttons)i).ToString(), i);
			}

			// Subscribe to the callbacks
			_dropDownMenu.OnMenuButtonClick += HandleButtonClick;

			// Hide the menu
			_dropDownMenu.Hide();

			_resultText.text = string.Empty;
		}

		protected void OnDestroy ()
		{
			if (_dropDownMenu != null)
			{
				_dropDownMenu.OnMenuButtonClick -= HandleButtonClick;
			}
		}

		protected void Update ()
		{
			// Shift + Left Click to show at mouse position
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (Input.GetMouseButtonDown(0))
				{
					_dropDownMenu.Show(Input.mousePosition);
				}
			}

			// Right click to hide
			if (Input.GetMouseButtonDown(1))
			{
				_dropDownMenu.Hide();
			}
		}
	}
}
