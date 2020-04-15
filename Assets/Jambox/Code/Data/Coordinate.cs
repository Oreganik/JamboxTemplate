// Jambox
// Copyright (c) 2019 Oreganik LLC
// Author: Ted Brown

using UnityEngine;

namespace Jambox
{
	[System.Serializable]
	public class Coordinate
	{
		public static int MaxX { get { return s_maxX; } }
		public static int MaxY { get { return s_maxY; } }

		public int Id { get { return _x + (_y * MaxY); } }
		public int X { get { return _x; } }
		public int Y { get { return _y; } }

		public Vector3 WorldPosition
		{
			get { return new Vector3(_x, 0, _y); }
		}

		private static int s_maxX, s_maxY;

		[SerializeField] private int _x, _y;

		public Coordinate(int x, int y)
		{
			if (s_maxX <= 0 || s_maxY <= 0)
			{
				Debug.LogWarning("Warning: Coordinate range has not been set.");
			}
			_x = x;
			_y = y;
		}

		public static void SetRange (int maxX, int maxY)
		{
			s_maxX = maxX;
			s_maxY = maxY;
		}

		public string ToString (string format = "")
		{
			return "(" + _x.ToString(format) + "," + _y.ToString(format) + ")";
		}
	}
}
