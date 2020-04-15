// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox.Examples
{
	public class StrategyExample : MonoBehaviour 
	{
		public GameObject _tilePrefab;
		public CameraController _cameraController;
		public int _mapSizeX = 32;
		public int _mapSizeY = 32;

		private Pointer _pointer;

		private void HandleDialogClose ()
		{
			_pointer.Activate(reset: true);
		}

		private void OpenDialog ()
		{
			PopupDialog.OpenOneOption("Pop-Up Dialog", "Click the button to continue", "OK", () => { });
			_pointer.Deactivate();
		}

		protected void Awake ()
		{
			Coordinate.SetRange(_mapSizeX, _mapSizeY);

			for (int y = 0; y < _mapSizeY; y++)
			{
				for (int x = 0; x < _mapSizeX; x++)
				{
					StrategyExampleTile tile = Instantiate(_tilePrefab).GetComponent<StrategyExampleTile>();
					tile.SetCoordinate(new Coordinate(x, y));
					tile.transform.parent = transform;
				}
			}

			PopupDialog.OnDialogClose += HandleDialogClose;

			_pointer = GetComponent<Pointer>();
		}

		protected void Start ()
		{
			_cameraController.SetPosition(new Vector3(_mapSizeX / 2f, 0, _mapSizeY / 2f));
		}

		protected void Update ()
		{
			if (PopupDialog.IsOpen)
			{
				return;
			}

			if (Input.GetKeyDown(KeyCode.P))
			{
				OpenDialog();
				return;
			}

			// The pointer is "inert" if the user clicks and holds a collider without an implementation IGrabTarget.
			// For this example, the tiles have an IHoverTarget, but not an IGrabTarget.
			if (_pointer.IsInert)
			{
				_cameraController.Orbit(Input.GetAxis("Mouse X"));
			}

			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) _cameraController.Orbit(1);
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) _cameraController.Orbit(-1);
		}
	}
}
