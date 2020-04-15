// JAMBOX
// Copyright (c) 2019 Oreganik LLC
// Author: Ted Brown

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jambox
{
	public class FloatingMenuButton : MonoBehaviour 
	{
		private FloatingMenu _parentMenu;
		private int _id;

		public void HandleClick ()
		{
			_parentMenu.HandleButtonClick(_id);
		}

		public void Initialize(FloatingMenu parentMenu, string name, int id)
		{
			_parentMenu = parentMenu;
			_id = id;
			GetComponent<Button>().onClick.AddListener(HandleClick);
			SetName(name);
		}

		public void SetName (string name)
		{
			TextMeshProUGUI tmprougui = GetComponentInChildren<TextMeshProUGUI>();
			if (tmprougui)
			{
				tmprougui.text = name;
				return;
			}

			Text uitext = GetComponentInChildren<Text>();
			if (uitext)
			{
				uitext.text = name;
				return;
			}

			TextMeshPro tmpro = GetComponentInChildren<TextMeshPro>();
			if (tmpro)
			{
				tmpro.text = name;
				return;
			}
		}
	}
}
