// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using UnityEngine;

namespace Jambox
{
	public interface IClickTarget
	{
		Transform GetTransform();
		void HandleClick(Pointer pointer);
	}
}
