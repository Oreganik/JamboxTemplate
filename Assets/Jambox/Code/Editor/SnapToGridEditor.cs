// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2021 Ted Brown

using UnityEditor;
using UnityEngine;

namespace Jambox
{
	[InitializeOnLoad]
	[CustomEditor(typeof(SnapToGrid), true)]
	[CanEditMultipleObjects]
	public class SnapToGridEditor : Editor 
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		
			SnapToGrid actor = target as SnapToGrid;
			if (actor.SnapPosition)
			{
				Vector3 position = RoundVector3(actor.transform.position, actor.PositionValue);
				if (actor.ForceZeroPosY) position.y = 0;
				actor.transform.position = position;
			}

			if (actor.SnapRotation)
			{
				Vector3 rotation = RoundVector3(actor.transform.rotation.eulerAngles, actor.RotationValue);
				actor.transform.rotation = Quaternion.Euler(rotation);
			}
		}
		
		private Vector3 RoundVector3 (Vector3 v, float snapValue)
		{
			return new Vector3
			(
				snapValue * Mathf.Round(v.x / snapValue),
				snapValue * Mathf.Round(v.y / snapValue),
				snapValue * Mathf.Round(v.z / snapValue)
			);
		}
	}
}
