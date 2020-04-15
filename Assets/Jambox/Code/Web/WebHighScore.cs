using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Jambox
{
	public class WebHighScore : MonoBehaviour 
	{
		public static WebHighScore Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject go = new GameObject("WebHighScore");
					_instance = go.AddComponent<WebHighScore>();
				}
				return _instance;
			}
		}

		private static WebHighScore _instance;

		private string _addScoreUrl = "http://localhost/~ted/jambox/addscore.php";

		public void AddScore (string player, int score)
		{
			WWWForm form = new WWWForm();
			form.AddField("player", player);
			form.AddField("score", score);
//			form.AddField("hashPost", hash);
			StartCoroutine(Post(form));
		}

		private IEnumerator Post (WWWForm form)
		{
			UnityWebRequest www = UnityWebRequest.Post(_addScoreUrl, form);

			yield return www.SendWebRequest();;
 
			if(www.isNetworkError || www.isHttpError) 
			{
				Debug.Log(www.error);
			}
			else 
			{
				Debug.Log("Form upload complete!");
			}
		}
	}
}
