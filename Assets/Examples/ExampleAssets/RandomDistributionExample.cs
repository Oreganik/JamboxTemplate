using UnityEngine;
using System.Collections;
using Jambox;

namespace Jambox.Examples 
{
	public class RandomDistributionExample : MonoBehaviour 
	{

		public bool ShowGrid = true;

		Color gridColor = new Color(.7f, .7f, 1);
		float width = 10;
		float depth = 10;
		int cellCountX = 10;
		int cellCountY = 10;
		float boxSize = 0.3f;
		Vector3 origin = Vector3.zero;

		void Start () 
		{
			//CreateGridPoints();
			Vector3[] spawnPoints = Jambox.RandomDistribution.GetFlatGridDistribution(origin, width, depth, cellCountX, cellCountY, boxSize * 0.5f);

			for (int i = 0; i < spawnPoints.Length; i++)
			{
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = spawnPoints[i];
				cube.transform.localScale = Vector3.one * boxSize;
			}

			/*
			Vector2 cellSize = new Vector2(width/(float)cellCountX, depth/(float)cellCountY);
			Vector3[] gridSquareOrigins = Jambox.RandomDistribution.GetFlatGridLocalOrigins(origin, width, depth, cellCountX, cellCountY);
			int littleCountX = 4;
			int littleCountY = 4;
			float littleMargin = 0.1f;
			for (int i = 0; i < gridSquareOrigins.Length; i++)
			{
				Vector3[] littleSpawnPoints = Jambox.RandomDistribution.GetFlatGridDistribution(gridSquareOrigins[i], cellSize.x, cellSize.y, littleCountX, littleCountY, littleMargin);
				for (int z = 0; z < littleSpawnPoints.Length; z++)
				{
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = littleSpawnPoints[z];
					cube.transform.localScale = Vector3.one * littleMargin * 2;
				}
			}
			*/
		}
		
		void Update () 
		{
			Vector2 cellSize = new Vector2(width/(float)cellCountX, depth/(float)cellCountY);

			// Fix camera position
			Camera.main.transform.position = origin + Vector3.right * cellSize.x * cellCountX * 0.5f;
			Camera.main.transform.position += Vector3.forward * cellSize.y * cellCountY * 0.5f;
			Camera.main.transform.position += Vector3.up * 10;

			// Draw debug grid lines
			if (ShowGrid)
			{
				for (int x = 0; x < cellCountX + 1; x++)
				{
					Vector3 begin = origin + Vector3.right * x * cellSize.x;
					Vector3 end = begin + Vector3.forward * depth;
					Debug.DrawLine(begin, end, gridColor);
				}

				for (int y = 0; y < cellCountY + 1; y++)
				{
					Vector3 begin = origin + Vector3.forward * y * cellSize.y;
					Vector3 end = begin + Vector3.right * width;
					Debug.DrawLine(begin, end, gridColor);
				}
			}
		}

		void CreateGridPoints ()
		{
			Vector2 cellSize = new Vector2(width/(float)cellCountX, depth/(float)cellCountY);
			
			for (int x = 0; x < cellCountX + 1; x++)
			{
				for (int y = 0; y < cellCountY + 1; y++)
				{
					//Debug.DrawLine(nodes[i - 1].position, nodes[i].position);
					GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					primitive.transform.localScale = Vector3.one * 0.1f;
					primitive.transform.position = origin + Vector3.right * x * cellSize.x;
					primitive.transform.position += Vector3.forward * y * cellSize.y;
				}
			}

		}
	}
}
