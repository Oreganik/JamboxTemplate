using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public static TimeManager Instance;

	public bool isPaused { get; private set; }
	bool isFrozen;
	float baseTimeScale;
	float deltaTimeScale;
	float fadeUpTimeScaleTimer;
	float fadeUpTimeScaleDuration;
	public float gameTimeScale { get; private set; }

	void Awake ()
	{
		Instance = this;
		gameTimeScale = 1;
	}

	void Update ()
	{
		fadeUpTimeScaleTimer += Time.deltaTime;
		if (fadeUpTimeScaleTimer > fadeUpTimeScaleDuration)
		{
			SetGameTimeScale(1);
			this.enabled = false;
		}
		else
		{
			SetGameTimeScale(baseTimeScale + deltaTimeScale * (fadeUpTimeScaleTimer / fadeUpTimeScaleDuration));
		}
	}

	public void SetPaused (bool value)
	{
		if (isPaused == value) return;
		
		isPaused = value;

		if (isPaused)
		{
			// Pause animations, sounds, etc.
		}
		else
		{
			// Resume animations, sounds, etc.
		}
	}

	public void SetGameTimeScale (float value)
	{
		gameTimeScale = value;
		Time.timeScale = value;
		// Set scale of sounds
	}
	
	public float DeltaGameTime ()
	{
		if (isPaused) return 0;
		else return Time.deltaTime;
	}
	
	public void SlowTimeScale (float targetTimeScale, float fadeUpTimeScaleDuration)
	{
		this.enabled = true;
		SetGameTimeScale(targetTimeScale);
		baseTimeScale = targetTimeScale;
		deltaTimeScale = 1 - baseTimeScale;
		this.fadeUpTimeScaleDuration = fadeUpTimeScaleDuration;
		fadeUpTimeScaleTimer = 0;
	}
}
