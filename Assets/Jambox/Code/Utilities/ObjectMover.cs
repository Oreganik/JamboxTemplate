using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public class ObjectMover : MonoBehaviour 
	{
		private static List<GameObject> _objects;
		private static Dictionary<GameObject, ObjectMoveData> _moveData;

		public static void AddFinishAction (GameObject targetObject, Action action)
		{
			if (_moveData == null)
			{
				Debug.LogError("Is there not an instance of ContentMover in the scene?");
			}
		}

		// todo: optionally reset position to start
		public static void Cancel (GameObject targetObject)
		{
			if (_objects == null)
			{
				Debug.LogError("Is there not an instance of ContentMover in the scene?");
			}

			_moveData.Remove(targetObject);
			_objects.Remove(targetObject);
		}

		public static ObjectMoveData Rotate (GameObject targetObject, Quaternion end, float duration, bool inWorldSpace = true)
		{
			return Rotate(targetObject, targetObject.transform.rotation, end, duration, inWorldSpace);
		}

		public static ObjectMoveData Rotate (GameObject targetObject, Quaternion start, Quaternion end, float duration, bool inWorldSpace = true)
		{
			ObjectMoveData moveData = GetMoveData(targetObject, duration);
			moveData.Rotate(start, end, inWorldSpace);
			return moveData;
		}

		public static ObjectMoveData Scale (GameObject targetObject, Vector3 end, float duration)
		{
			return Scale(targetObject, targetObject.transform.localScale, end, duration);
		}

		public static ObjectMoveData Scale (GameObject targetObject, Vector3 start, Vector3 end, float duration)
		{
			ObjectMoveData moveData = GetMoveData(targetObject, duration);
			moveData.Scale(start, end);
			return moveData;
		}

		public static ObjectMoveData Translate (GameObject targetObject, Vector3 end, float duration, bool inWorldSpace = true)
		{
			return Translate(targetObject, targetObject.transform.position, end, duration, inWorldSpace);
		}

		public static ObjectMoveData Translate (GameObject targetObject, Vector3 start, Vector3 end, float duration, bool inWorldSpace = true)
		{
			ObjectMoveData moveData = GetMoveData(targetObject, duration);
			moveData.Translate(start, end, inWorldSpace);
			return moveData;
		}

		private static ObjectMoveData GetMoveData (GameObject targetObject, float duration)
		{
			if (_moveData == null)
			{
				Debug.LogError("Is there not an instance of ContentMover in the scene?");
			}

			ObjectMoveData moveData = null;
			if (_moveData.TryGetValue(targetObject, out moveData) == false)
			{
				moveData = new ObjectMoveData(targetObject.transform, duration);
				_moveData.Add(targetObject, moveData);
				_objects.Add(targetObject);
			}
			return moveData;
		}

		protected void Awake ()
		{
			_moveData = new Dictionary<GameObject, ObjectMoveData>();
			_objects = new List<GameObject>();
		}

		protected void Update ()
		{
			int count = _objects.Count;
			ObjectMoveData moveData = null;
			float deltaTime = Time.deltaTime;

			for (int i = count - 1; i >= 0; i--)
			{
				if (_moveData.TryGetValue(_objects[i], out moveData))
				{
					moveData.Process(deltaTime);
					if (moveData.IsComplete)
					{
						moveData.FinishNow();
						_moveData.Remove(_objects[i]);
						_objects.RemoveAt(i);
					}
				}
			}
		}

	}
}
