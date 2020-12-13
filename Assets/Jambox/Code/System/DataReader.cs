// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine;

namespace Jambox
{
    public class DataReader
    {
		private const char DATA_SEPARATOR = '\t';

		/// <summary>
		/// Processes a Unity text asset with tab delimited data and returns a list of string arrays.
		/// </summary>
		public static bool GetDataAsStringArray (TextAsset textAsset, int columns, out List<string[]> data)
		{
			string[] textData = textAsset.text.Split(new[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			return GetDataAsStringArray(textData, columns, out data);
		}

		/// <summary>
		/// Processes a data file with tab delimited data and returns a list of string arrays.
		/// </summary>
		public static bool GetDataAsStringArray (string dataPath, int columns, out List<string[]> data)
		{
			string[] textData = File.ReadAllLines(dataPath);
			return GetDataAsStringArray(textData, columns, out data);
		}

		/// <summary>
		/// Processes a data file with tab delimited data and returns a list of string arrays.
		/// </summary>
		public static bool GetDataAsStringArray (string[] textData, int columns, out List<string[]> data)
		{
			// Initialize the list of data. Each entry is an array of raw data strings.
			data = new List<string[]>();

			bool errorOnLoad = false;
			string[] lineData = null;

			foreach (string line in textData)
			{
				// Ignore empty lines and comment lines
				if (string.IsNullOrEmpty(line) || line[0].Equals('#')) 
				{
					continue;
				}

				// Ignore empty lines and comment lines
				if (string.IsNullOrEmpty(line) || line[0].Equals('#')) 
				{
					continue;
				}

				// Split the line into an array
				lineData = line.Split(DATA_SEPARATOR);

				// Throw an error if the line gave us an incorrect amount of data
				if (lineData.Length != columns)
				{
					//Program.LogError(string.Format("GetDataAsStringArray: Data entry '{0}' did not have exactly {1} tab-delimited columns", line, columns));
					errorOnLoad = true;
					continue;
				}

				// Throw an error if any data columns are null or empty
				for (int i = 0; i < lineData.Length; i++)
				{
					if (string.IsNullOrEmpty(lineData[i]))
					{
						//Program.LogError(string.Format("GetDataAsStringArray: Data entry '{0}' had one or more empty columns", line));
						errorOnLoad = true;
						continue;
					}
				}

				data.Add(lineData);
			}

			return errorOnLoad;
		}
	}
}
