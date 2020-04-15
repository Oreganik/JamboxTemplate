// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class TrackSegment : MonoBehaviour 
	{
		public Path pathLow;
		public Path pathHigh;

		public float GetDistance ()
		{
			return pathLow.GetDistance();
		}
	}
}
