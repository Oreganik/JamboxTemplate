// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// Forces objects to snap to a specified grid after being moved or rotated.
	/// </summary>
	public class SnapToGrid : MonoBehaviour 
	{
		public bool SnapPosition = true;
		public float PositionValue = 1;
		public bool ForceZeroPosY = true;
		public bool SnapRotation = true;
		public float RotationValue = 90;
	}
}
