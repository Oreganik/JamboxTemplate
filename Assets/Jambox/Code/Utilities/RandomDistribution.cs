using UnityEngine;
using System.Collections;

namespace Jambox
{
	public static class RandomDistribution
	{
		public static Vector3[] GetFlatGridDistribution (Vector3 origin, float width, float depth, int cellCountX, int cellCountY, float margin = 0)
		{
			Vector2 cellSize = new Vector2(width/(float)cellCountX, depth/(float)cellCountY);

			Vector3[] spawnPoints = new Vector3[cellCountX * cellCountY];

			for (int y = 0; y < cellCountY; y++)
			{
				for (int x = 0; x < cellCountX; x++)
				{
					Vector3 localOrigin = origin + Vector3.right * cellSize.x * x;
					localOrigin += Vector3.forward * cellSize.y * y;
					int id = x + (y * cellCountY);
					spawnPoints[id] = localOrigin + Vector3.right * Random.Range(margin, cellSize.x - margin);
					spawnPoints[id] += Vector3.forward * Random.Range(margin, cellSize.y - margin);
				}
			}

			return spawnPoints;
		}

		public static Vector3[] GetFlatGridLocalOrigins (Vector3 origin, float width, float depth, int cellCountX, int cellCountY)
		{
			Vector2 cellSize = new Vector2(width/(float)cellCountX, depth/(float)cellCountY);
			
			Vector3[] localOrigins = new Vector3[cellCountX * cellCountY];
			
			for (int y = 0; y < cellCountY; y++)
			{
				for (int x = 0; x < cellCountX; x++)
				{
					int id = x + (y * cellCountY);
					localOrigins[id] = origin + Vector3.right * cellSize.x * x;
					localOrigins[id] += Vector3.forward * cellSize.y * y;
				}
			}
			
			return localOrigins;
		}

	}
}