// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jambox
{
	public class SceneLoaderAsync
	{
		private string _sceneName;
		private float Progress;

		public SceneLoaderAsync (string sceneName)
		{
			_sceneName = sceneName;
			
		}

		private IEnumerator LoadScene (string sceneName)
		{
			Progress = 0;

			yield return null;

			AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			ao.allowSceneActivation = false;

			while (!ao.isDone)
			{
				// [0, 0.9] > [0, 1]
				Progress = Mathf.Clamp01(ao.progress / 0.9f);

				// Loading completed
				if (ao.progress == 0.9f)
				{
					// if you want to delay things, wait for input here
					{
						ao.allowSceneActivation = true;
					}
				}

				yield return null;
			}

			yield return null;

			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
		}
	}
}