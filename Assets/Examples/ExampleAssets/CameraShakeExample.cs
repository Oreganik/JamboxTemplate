using Jambox;
using UnityEngine;

namespace Jambox.Examples 
{
	public class CameraShakeExample : MonoBehaviour 
	{
		void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				CameraShake.LittleShake();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				CameraShake.BigShake();
			}
		}
	}
}
