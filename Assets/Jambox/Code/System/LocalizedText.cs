// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System;
using System.Collections.Generic;
using System.IO;

namespace Jambox
{
	/// <summary>
	/// Processes a data file containing string id's and translations.
	/// NOTE: As currently built, only supports a single language.
	/// </summary>
    public class LocalizedText
    {
		public static bool ErrorOnLoad
		{
			get { return s_errorOnLoad; }
		}

		private static bool s_errorOnLoad;
		private static bool s_initialized;
		private static Dictionary<string, string> s_text;

		public static bool Initialize (string dataPath)
		{
			s_text = new Dictionary<string, string>();

			// Read data from the file into a list of string arrays. Return if an error is encountered.
			List<string[]> data;
			s_errorOnLoad = DataReader.GetDataAsStringArray(dataPath, 2, out data);
			if (s_errorOnLoad)
			{
				UnityEngine.Debug.LogError(string.Format("LocalizedText encountered an error processing data file '{0}'", dataPath));
				return false;
			}

			// Process every data entry.
			foreach (string[] entry in data)
			{
				// Add the data for this entry to the dictionary
				// Schema: Id [0], Text [1]

				// Make sure this id doesn't already exist in our database
				if (s_text.ContainsKey(entry[0]))
				{
					UnityEngine.Debug.LogError(string.Format("LocalizedText: An entry with id '{0}' has already been added to the database. Skipping", entry[0]));
					s_errorOnLoad = true;
					continue;
				}

				s_text.Add(entry[0], entry[1]);
			}

			s_initialized = true;

			return true;
		}

		public static string GetText (string id)
		{
			string text = string.Format("[[{0}]]", id);
			
			if (s_initialized == false)
			{
				UnityEngine.Debug.LogWarning(string.Format("LocalizedText could not get text for id '{0}' because it has not been initialized", id));
				return text;
			}

			if (s_text.TryGetValue(id, out text) == false)
			{
				UnityEngine.Debug.LogWarning(string.Format("LocalizedText could not find text for id '{0}'", id));
			}

			return text;
		}
	}
}
