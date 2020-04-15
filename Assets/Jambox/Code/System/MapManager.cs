// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

// Based on code explained here:
// https://www.alanzucconi.com/2016/03/30/loading-bar-in-unity/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
	public MapManager Instance { get; private set; }

	private Coroutine _sceneLoader;
	private Dictionary<string, bool> _allowLoadToComplete;
	private Dictionary<string, float> _loadProgress;
	private Dictionary<string, bool> _makeActiveOnLoad;

	public void FinishLoad (string sceneName)
	{
		_allowLoadToComplete[sceneName] = true;
	}

	public float GetProgress (string sceneName)
	{
		float progress = 0;
		if (_loadProgress.TryGetValue(sceneName, out progress))
		{
			return progress;
		}
		Debug.LogWarning("Scene " + sceneName + " has not been loaded, is not being loaded");
		// TODO: Check if it's in the queue
		return -1;
	}

	public bool IsReadyForActivation (string sceneName)
	{
		float progress = 0;
		if (_loadProgress.TryGetValue(sceneName, out progress))
		{
			return progress >= 0.9f;
		}
		return false;
	}

	public void Load (string sceneName, bool activateImmediately = true, bool makeActiveScene = true)
	{
		if (_sceneLoader != null)
		{
			// TODO: add to queue
			Debug.Log("Async load operation in progress. Adding scene to queue");
			return;
		}

		// TODO: check if the scene is already loaded
		_loadProgress.Add(sceneName, 0);
		_allowLoadToComplete.Add(sceneName, activateImmediately);
		_sceneLoader = StartCoroutine(iLoadScene(sceneName));
	}

	public void Unload (string sceneName)
	{
		StartCoroutine(iUnload(sceneName));
	}

	protected void Awake ()
	{
		Instance = this;
		_allowLoadToComplete = new Dictionary<string, bool>();
		_loadProgress = new Dictionary<string, float>();
		_makeActiveOnLoad = new Dictionary<string, bool>();
	}

	private IEnumerator iLoadScene (string sceneName)
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		ao.allowSceneActivation = _allowLoadToComplete[sceneName];

		while (!ao.isDone)
		{
			// [0, 0.9] > [0, 1]
			_loadProgress[sceneName] = Mathf.Clamp01(ao.progress / 0.9f);

			// Loading completed
			if (ao.progress == 0.9f)
			{
				// if you want to delay things, wait for input here
				{
					ao.allowSceneActivation = _allowLoadToComplete[sceneName];
				}
			}

			yield return null;
		}

		yield return null;

		if (_makeActiveOnLoad[sceneName])
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
		}
	}

	private IEnumerator iUnload (string sceneName)
	{
		// is another scene active? that's important...
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName(SYSTEM_SCENE_NAME));

		AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);

		while (ao.isDone == false)
		{
			yield return null;
		}
	}
}
