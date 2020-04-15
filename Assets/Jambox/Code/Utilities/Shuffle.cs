// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	public class Shuffle : MonoBehaviour 
	{
		// variant of the Fisher-Yates-Shuffle which does biased randomly choose the swapped element and is known as Sattolo's algorithm
		// https://stackoverflow.com/questions/15003825/shuffles-random-numbers-with-no-repetition-in-javascript-php
		public static void ShuffleArray (int[] array) 
		{
			for (var i = array.Length - 1; i > 0; i--) 
			{
				int j = (int) Mathf.Floor(Random.Range(0, 1f) * i);
				var temp = array[i];
				array[i] = array[j];
				array[j] = temp;
		    }
		}
	}
}
