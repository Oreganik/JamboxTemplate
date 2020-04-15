// JAMBOX
// General purpose game code for Unity
// Author: Ted Brown
// Contains code from http://catlikecoding.com/unity/tutorials/curves-and-splines/

using UnityEngine;

namespace Jambox
{
	// When PathFollower is attached to a GameObject, it will follow a given path from start to end, 
	// over a given time, at a rate that is determined by the motion curve, travel time, and path distance.
	public class PathFollower : MonoBehaviour
	{
		public bool HasReachedDestination
		{
			get { return (_distanceTraveled >= _pathDistance); }
		}

		public AnimationCurve motionCurve = AnimationCurve.Linear(0, 0, 1, 1);

		private bool _followDirection = true;
		private float _pathDistance;
		private float _distanceTraveled;
		private float _timer;
		private Path _targetPath;

		public void GoToEnd ()
		{
			_distanceTraveled = _pathDistance;
			//timer = travelTime;
			ProcessPathPosition(1);
		}

		public void GoToStart (float pushAhead = 0)
		{
			_timer = pushAhead;
			ProcessPathPosition(0);
		}

		public void Process (float speed)
		{
			if (_targetPath)
			{
				if (HasReachedDestination)
				{
					this.enabled = false;
				}
				else
				{
					_distanceTraveled += speed * Time.deltaTime;
					ProcessPathPosition(_distanceTraveled / _pathDistance);
					/*
					float travelTime = pathDistance / speed;
					timer = Mathf.Clamp(timer + Time.deltaTime, 0, travelTime);
					float percentComplete = motionCurve.Evaluate(timer / travelTime);
					ProcessPathPosition(percentComplete);
					*/
				}
			}
		}

		public void SetPath (Path path)
		{
			_targetPath = path;
			_timer = 0;
			_pathDistance = path.GetDistance();
			_distanceTraveled = 0;
			ProcessPathPosition(0);
		}

		private void ProcessPathPosition (float percentComplete)
		{
			transform.position = _targetPath.GetPointByLinearDistanceTraveled(percentComplete * _targetPath.pathDistance);

			if (_followDirection == true)
			{
				transform.rotation = Quaternion.LookRotation(_targetPath.GetDirectionByLinearDistanceTraveled(percentComplete * _targetPath.pathDistance));
			}
		}
	}
}
