// ANGRY MOUNTAIN GODS
// Copyright 2018 Ted Brown
// Created for Ludum Dare 43
// December 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	[CreateAssetMenu(fileName = "Palette", menuName = "Jambox/Color Palette", order = 1)]
	public class ColorPalette : ScriptableObject 
	{
		#pragma warning disable 0649
		[SerializeField] private Color[] Colors;
		#pragma warning restore 0649

		public Color GetColor (int id)
		{
			if (id >= 0 && id < Colors.Length)
			{
				return Colors[id];
			}
			return Color.black;
		}
	}
}
