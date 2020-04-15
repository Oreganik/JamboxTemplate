using UnityEngine;
using System.Collections;

namespace Jambox
{
		
	public class SpriteSheet : MonoBehaviour 
	{

		public float spriteWidth;
		public float spriteHeight;
		public int spriteX;
		public int spriteY;

		Mesh mesh;
		Vector2[] uvs;
		Texture texture;
		Vector2 pixelDims;
		
		void Awake ()
		{
			texture = GetComponent<Renderer>().material.mainTexture;
			mesh = GetComponent<MeshFilter>().mesh;
			SetSpriteXY(spriteX,spriteY);
		}

		public void SetSpriteXY (int spriteX, int spriteY)
		{
			float x = spriteX * spriteWidth;
			float y = spriteY * spriteHeight;

			uvs = new Vector2[mesh.uv.Length];

			pixelDims = new Vector2(spriteWidth / (float)texture.width, -spriteHeight / (float)texture.height);

			Vector2 pixelMin = new Vector2(x / (float)texture.width, 1.0f - y / (float)texture.height);
			
			//Debug.Log(pixelMin.ToString() + " -//- " + pixelDims.ToString());
			
			Vector2 min = pixelMin;
			uvs[0] = min + new Vector2(pixelDims.x * 0.0f, pixelDims.y * 1.0f);
			uvs[2] = min + new Vector2(pixelDims.x * 1.0f, pixelDims.y * 1.0f);
			uvs[3] = min + new Vector2(pixelDims.x * 0.0f, pixelDims.y * 0.0f);
			uvs[1] = min + new Vector2(pixelDims.x * 1.0f, pixelDims.y * 0.0f);
			mesh.uv = uvs;		
		}

	}
}
