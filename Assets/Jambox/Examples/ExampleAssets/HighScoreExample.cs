using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jambox.Examples 
{
	public class HighScoreExample : MonoBehaviour 
	{
		public Text _randomScoreText;

		public void PostRandomScore ()
		{
			int id = Random.Range(0, 100);
			int score = Random.Range(1000, 9999);
			WebHighScore.Instance.AddScore("Player " + id, score);
			_randomScoreText.text = "Player " + id + ": " + score;
		}
	}
}
