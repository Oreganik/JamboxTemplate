// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using UnityEngine;

namespace Jambox
{
	public interface IHoverTarget
	{
		Transform GetTransform();
		void HandleHover(Pointer pointer);
		void HandleUnhover(Pointer pointer);
	}
}
