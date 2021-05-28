// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox.Experiments
{
	/// <summary>
	/// Trying to figure out how to validate a series of classes based on an enum
	/// </summary>
	public class RuntimeTypeValidation : MonoBehaviour 
	{
		enum TestType { Works, WrongParentClass, NotImplemented }

		class Test
		{
			public void Hey () { Debug.Log(this.GetType().Name + " has been validated"); }
		}

		class WorksTest : Test
		{}

		class WrongParentClassTest : Test
		{}

		public static bool Validate ()
		{
			bool success = true;
			const string template = "Jambox.Experiments.RuntimeTypeValidation+{0}Test";
			for (int i = 0; i < System.Enum.GetValues(typeof(TestType)).Length; i++)
			{
				TestType t = (TestType) i;

				Type type = Type.GetType(string.Format(template, t.ToString()));
				if (type == null)
				{
					Debug.LogError(t.ToString() + " failed at GetType. The class does not exist.");
					success = false;
					continue;
				}

				object obj = null;

				try
				{
					obj = (Test) Activator.CreateInstance(type);
				}
				catch
				{
					Debug.LogError(t.ToString() + " -> " + type + " does not derive from class Test");
					continue;
				}

				if (obj == null)
				{
					Debug.LogError(type.GetType() + "  " + t.ToString() + " failed at CreateInstance");
					success = false;
					continue;
				}

				Test testObj = (Test) obj;
				if (testObj == null)
				{
					Debug.LogError(t.ToString() + " failed to cast to type Test");
					success = false;
					continue;
				}

				testObj.Hey();
			}
			
			return success;
		}
	}
}
