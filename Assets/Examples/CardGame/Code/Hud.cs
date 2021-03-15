// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// 
	/// </summary>
	public class Hud : MonoBehaviour 
	{
		public static Hud Instance;

		public bool IsBusy
		{
			get
			{
				foreach (PlayerCanvas playerCanvas in _playerCanvases)
				{
					if (playerCanvas.IsBusy) return true;
				}
				return false;
			}
		}

		public PlayerCanvas[] _playerCanvases;

		public void SetPlayer(int id, Entity player)
		{
			_playerCanvases[id].SetPlayer(player, id);
		}

		protected void Awake ()
		{
			Instance = this;
		}
	}
}
