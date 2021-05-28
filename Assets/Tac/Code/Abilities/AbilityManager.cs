// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// 
	/// </summary>
	public class AbilityManager <T> where T : System.Enum
	{
		private readonly string _classTemplate;

		public AbilityManager (string classTemplate)
		{
			_classTemplate = classTemplate;
		}

		public Ability GetAbilityInstance (T typeName, string parameters)
		{
			string name = typeName.ToString();

			if (name.ToLower().Equals("invalid"))
			{
				Debug.LogError("Can't get ability instance of type 'invalid'");
				return null;
			}

			Type type = Type.GetType(string.Format(_classTemplate, name));

			// If we don't run Validate when the app starts, this might throw a null ref
			Ability ability = null;
			
			try
			{
				ability = (Ability) Activator.CreateInstance(type, parameters);
			} 
			catch
			{
				Debug.LogError(type + " does not derive from class Ability");
				Debug.Log("Was AbilityManager.Validate not run on launch?");
				return null;
			}

			return ability;
		}

		public bool Validate ()
		{
			bool success = true;

			foreach (string name in Enum.GetNames(typeof(T)))
			{
				if (name.ToLower().Equals("invalid"))
				{
					continue;
				}

				Type type = Type.GetType(string.Format(_classTemplate, name));
				if (type == null)
				{
					Debug.LogError(name + " failed at GetType. The class does not exist.");
					success = false;
					continue;
				}

				object obj = null;

				try
				{
					obj = (Ability) Activator.CreateInstance(type);
				}
				catch
				{
					Debug.LogError(type + " does not derive from class Ability, or has a constructor but is missing an empty constructor");
					continue;
				}

				if (obj == null)
				{
					Debug.LogError(type.GetType() + "  " + name + " failed at CreateInstance");
					success = false;
					continue;
				}
/*
				Effect testObj = (Effect) obj;
				if (testObj == null)
				{
					Debug.LogError(name + " failed to cast to type Effect");
					success = false;
					continue;
				}

				testObj.IsValid();
*/
			}
			
			return success;
		}
	}
}
