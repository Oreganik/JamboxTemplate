// JAMBOX
// General purpose game code for Unity
// Copyright (c) 2020 Ted Brown

using UnityEngine;
using System.Collections;

// SaveData is a wrapper over PlayerPrefs that allows:
// 1. Default values
// 2. Boolean support (where zero = false and non-zero = true)
// 3. Long int support (casting to and from a string)
// 4. Profile support (using a prefix to demarcate which local profile is being used)

// It also gives us the option of:
// 1. Using a game-controlled file to store data, as opposed to PlayerPrefs
// 2. Encrypting data to prevent hacking

namespace Jambox
{
	public class SaveData  
	{
		#region SYSTEM
		public static bool HasKey (string key)
		{
			return PlayerPrefs.HasKey(key);
		}
		
		public static bool DeleteKey (string key)
		{
			if (HasKey(key))
			{
				PlayerPrefs.DeleteKey(key);
				return true;
			}
			return false;
		}
		
		public static void DeleteSaveDataArray (string key)
		{
			int arrayLength = GetInt(key + "_ArrayLength");
			if (arrayLength > 0)
			{
				for (int i = 0; i < arrayLength; i++)
				{
					DeleteKey(key + "_" + i.ToString());
				}
			}
		}
		#endregion
		
		#region BOOL
		public static bool GetBool (string key)
		{
			if (PlayerPrefs.GetInt(key) == 0)
				return false;
			else
				return true;
		}
		
		public static bool[] GetBoolArray (string key)
		{
			int arrayLength = GetInt(key + "_ArrayLength");
			if (arrayLength == 0)
			{
				return new bool[0];
			}
			else
			{
				bool[] values = new bool[arrayLength];
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = GetBool(key + "_" + i.ToString());
				}
				return values;
			}
		}
		
		public static void SetBool (string key, bool isTrue)
		{
			if (isTrue) PlayerPrefs.SetInt (key, 1);
			else PlayerPrefs.SetInt (key, 0);
		}
		
		public static void SetBoolArray (string key, bool[] values)
		{
			// Clear previous array in case we are writing a shorter one
			DeleteSaveDataArray(key);
			
			for (int i = 0; i < values.Length; i++)
			{
				SetBool (key + "_" + i.ToString(), values[i]);
			}
			SetInt (key + "_ArrayLength", values.Length);
		}
		#endregion
		
		#region INT
		public static int GetInt (string key)
		{
			return PlayerPrefs.GetInt(key);
		}
		
		public static int[] GetIntArray (string key)
		{
			int arrayLength = GetInt(key + "_ArrayLength");
			if (arrayLength == 0)
			{
				return new int[0];
			}
			else
			{
				int[] values = new int[arrayLength];
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = GetInt(key + "_" + i.ToString());
				}
				return values;
			}
		}
		
		public static void SetInt (string key, int value)
		{
			PlayerPrefs.SetInt (key, value);
		}
		
		public static void SetIntArray (string key, int[] values)
		{
			// Clear previous array in case we are writing a shorter one
			DeleteSaveDataArray(key);
			
			// Set new values
			for (int i = 0; i < values.Length; i++)
			{
				SetInt (key + "_" + i.ToString(), values[i]);
			}
			SetInt (key + "_ArrayLength", values.Length);
		}
		#endregion
		
		#region FLOAT
		public static float GetFloat (string key)
		{
			return PlayerPrefs.GetFloat(key);
		}
		
		public static float[] GetFloatArray (string key)
		{
			int arrayLength = GetInt(key + "_ArrayLength");
			if (arrayLength == 0)
			{
				return new float[0];
			}
			else
			{
				float[] values = new float[arrayLength];
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = GetFloat(key + "_" + i.ToString());
				}
				return values;
			}
		}
		
		public static void SetFloat (string key, float value)
		{
			PlayerPrefs.SetFloat (key, value);
		}
		
		public static void SetFloatArray (string key, float[] values)
		{
			// Clear previous array in case we are writing a shorter one
			DeleteSaveDataArray(key);
			
			// It would be a good idea to clear out all previous array key/value pairs
			for (int i = 0; i < values.Length; i++)
			{
				SetFloat (key + "_" + i.ToString(), values[i]);
			}
			SetInt (key + "_ArrayLength", values.Length);
		}
		#endregion
		
		#region LONG
		public static long GetLong (string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				// This is only guaranteed to work if the value was set by SetLong
				return long.Parse((PlayerPrefs.GetString(key)));
			}
			else
			{
				return 0;
			}
		}
		
		public static long[] GetLongArray (string key)
		{
			int arrayLength = GetInt(key + "_ArrayLength");
			if (arrayLength == 0)
			{
				return new long[0];
			}
			else
			{
				long[] values = new long[arrayLength];
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = GetLong(key + "_" + i.ToString());
				}
				return values;
			}
		}
		
		public static void SetLong (string key, long value)
		{
			SetString (key, value.ToString());
		}
		
		public static void SetLongArray (string key, long[] values)
		{
			// Clear previous array in case we are writing a shorter one
			DeleteSaveDataArray(key);
			
			for (int i = 0; i < values.Length; i++)
			{
				SetLong (key + "_" + i.ToString(), values[i]);
			}
			SetInt (key + "_ArrayLength", values.Length);
		}
		#endregion
		
		#region STRING
		public static string GetString (string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetString(key);
			}
			else
			{
				return System.String.Empty;
			}
		}
		
		public static string[] GetStringArray (string key)
		{
			int arrayLength = GetInt(key + "_ArrayLength");
			if (arrayLength == 0)
			{
				return new string[0];
			}
			else
			{
				string[] values = new string[arrayLength];
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = GetString(key + "_" + i.ToString());
				}
				return values;
			}
		}
		
		public static void SetString (string key, string value)
		{
			PlayerPrefs.SetString (key, value);
		}
		
		public static void SetStringArray (string key, string[] values)
		{
			// Clear previous array in case we are writing a shorter one
			DeleteSaveDataArray(key);
			
			// It would be a good idea to clear out all previous array key/value pairs
			for (int i = 0; i < values.Length; i++)
			{
				SetString (key + "_" + i.ToString(), values[i]);
			}
			SetInt (key + "_ArrayLength", values.Length);
		}
		#endregion
		
		#region DATETIME
		public static System.DateTime GetDateTime (string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				return System.DateTime.Parse(PlayerPrefs.GetString(key));
			}
			else
			{
				return System.DateTime.MinValue;
			}
		}
		
		public static void SetDateTime (string key, System.DateTime value)
		{
			PlayerPrefs.SetString(key, value.ToString());
		}
		#endregion
	}
}
