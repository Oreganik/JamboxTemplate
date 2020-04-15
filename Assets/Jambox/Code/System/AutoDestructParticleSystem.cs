///////////////////////////////////////////////////////////////////////////////
//
//	AutoDestructParticleSystem.cs
//	Part of the JamBox Plugin for Unity
//
//	Copyright (c) 2015, Oreganik LLC
//	All rights reserved.
//
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace Jambox
{
	public class AutoDestructParticleSystem : MonoBehaviour 
	{
		void LateUpdate () 
		{
			if (GetComponent<ParticleSystem>().IsAlive()) 
			{
				if (GetComponent<ParticleSystem>().isStopped) Destroy (gameObject);
			}
			else
			{
				if (GetComponent<AudioSource>())
				{
					if (GetComponent<AudioSource>().isPlaying == false)
					{
						Destroy (gameObject);
					}
				}
				else
				{
					Destroy (gameObject);
				}
			}
		}
	}
}
