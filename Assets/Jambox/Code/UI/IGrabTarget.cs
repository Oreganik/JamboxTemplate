// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using UnityEngine;

namespace Jambox
{
	public interface IGrabTarget 
	{
		Transform GetTransform();
		void HandleGrab (Pointer pointer);
		void HandleRelease (Pointer pointer);
	}
}
