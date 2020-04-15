using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/2018.3/Documentation/ScriptReference/WWWForm.html
// https://docs.unity3d.com/2018.3/Documentation/ScriptReference/Networking.UnityWebRequest.Post.html
// http://www.nuf.dk/creating-highscore-system-for-unity-using-sql-and-php/

public class WebUtil : MonoBehaviour 
{
	public const string ARRAY_DELIMITER = ":";

	// https://wiki.unity3d.com/index.php/MD5
	// Author: Matthew Wegner
	public static string GetMd5Sum (string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
	
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
	
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
	
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
	
		return hashString.PadLeft(32, '0');
	}
}
