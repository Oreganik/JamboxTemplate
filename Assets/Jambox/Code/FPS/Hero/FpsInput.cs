// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
	/// <summary>
	/// 
	/// </summary>
	public enum FpsInput
	{
		Invalid,

		// Move
		Crouch,
		Jump,
		Run,
		Sneak,

		// Do Stuff
		Aim,
		Interact,
		Reload,
		Shoot,

		// UI
		HeroMenu,
		InventoryLeft,
		InventoryRight,
		Screenshot,
		SystemMenu,

		// Menus
		Cancel,
		Confirm,
		MenuUp,
		MenuDown,
		MenuLeft,
		MenuRight,
		MenuNext,
		MenuPrevious,
	}
}
