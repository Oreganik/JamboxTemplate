using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WebData
{

	private string _key;
	private List<string> _values;

	public WebData (string key)
	{
		_key = key;
		_values = new List<string>();
	}

	public WebData (string key, string value)
	{
		_key = key;
		_values = new List<string>();
		AddValue(value);
	}

	public WebData (string key, params string[] values)
	{
		_key = key;
		_values = new List<string>();
		AddValues(values);
	}

	public WebData (string key, List<string> values)
	{
		_key = key;
		_values = new List<string>();
		AddValues(values);
	}

	public void AddValue (string value)
	{
		_values.Add(value);
	}

	public void AddValues (params string[] values)
	{
		_values.AddRange(values);
	}

	public void AddValues (List<string> values)
	{
		_values.AddRange(values);
	}

	public string GetUrlEncodedString ()
	{
		StringBuilder urlstring = new StringBuilder(_key + "=", 2048);
		int count = _values.Count;
		for (int i = 0; i < count; i++)
		{
			if (i > 0) urlstring.Append(WebUtil.ARRAY_DELIMITER);
			urlstring.Append(System.Uri.EscapeUriString(_values[i]));
		}
		return System.Uri.EscapeUriString(urlstring.ToString());
	}
}