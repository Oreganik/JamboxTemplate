// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class Track : MonoBehaviour 
	{
		public float TotalDistance
		{
			get { return _totalDistance; }
		}

		private float _totalDistance;
		private TrackSegment[] _trackSegments;

		public TrackSegment GetNearestTrackSegment (Vector3 position)
		{
			TrackSegment nearestSegment = null;
			float nearestDistance = float.MaxValue;

			foreach (TrackSegment trackSegment in _trackSegments)
			{
				float d = Vector3.Distance(position, trackSegment.pathLow.transform.position);
				if (d < nearestDistance)
				{
					nearestSegment = trackSegment;
					nearestDistance = d;
				}
			}

			return nearestSegment;
		}

		private void Awake ()
		{
			_trackSegments = GetComponentsInChildren<TrackSegment>();
			foreach (TrackSegment trackSegment in _trackSegments)
			{
				_totalDistance += trackSegment.GetDistance();
			}
		}
	}
}
