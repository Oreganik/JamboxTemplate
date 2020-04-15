using UnityEngine;
using System.Collections;

public class AnimatedSpriteSheet : MonoBehaviour {

	public int spritesPerRow;
	public int spritesPerColumn;
	public float framesPerSecond;
	public bool isLoop;

	public bool isFinished { get; private set; }

	public void SetSpriteXY (int spriteX, int spriteY)
	{
		float x = spriteX * _spriteWidth;
		float y = spriteY * _spriteHeight;
		
		Vector2[] uvs = new Vector2[_mesh.uv.Length];
		
		Vector2 pixelDims = new Vector2(_spriteWidth / (float)_texture.width, -_spriteHeight / (float)_texture.height);
		
		Vector2 pixelMin = new Vector2(x / (float)_texture.width, 1.0f - y / (float)_texture.height);
		
		//Debug.Log(pixelMin.ToString() + " -//- " + pixelDims.ToString());
		
		Vector2 min = pixelMin;
		uvs[0] = min + new Vector2(pixelDims.x * 0.0f, pixelDims.y * 1.0f);
		uvs[2] = min + new Vector2(pixelDims.x * 1.0f, pixelDims.y * 1.0f);
		uvs[3] = min + new Vector2(pixelDims.x * 0.0f, pixelDims.y * 0.0f);
		uvs[1] = min + new Vector2(pixelDims.x * 1.0f, pixelDims.y * 0.0f);
		_mesh.uv = uvs;		
	}

	float _animationTimer;
	float _animationDelay;
	float _spriteHeight;
	float _spriteWidth;
	int _totalFrames;
	int _currentFrame;
	Mesh _mesh;
	Texture _texture;

	void Awake ()
	{
		_texture = GetComponent<Renderer>().material.mainTexture;
		_spriteWidth = _texture.width / spritesPerRow;
		_spriteHeight = _texture.height / spritesPerColumn;

		_totalFrames = spritesPerRow * spritesPerColumn;

		_mesh = GetComponent<MeshFilter>().mesh;

		if (framesPerSecond == 0) framesPerSecond = 1;
		_animationDelay = 1 / framesPerSecond;

		SetSpriteXY(0,0);
	}

	void Update ()
	{
		_animationTimer -= TimeManager.Instance.DeltaGameTime ();

		if (_animationTimer <= 0)
		{
			_animationTimer = _animationDelay;

			if (_currentFrame == _totalFrames - 1) 
			{
				isFinished = true;
				if (isLoop) _currentFrame = 0;
			}
			else _currentFrame += 1;

			int spriteX = (int) ((float) _currentFrame % (float) spritesPerRow);
			int spriteY = (int) ((float) _currentFrame / (float) spritesPerColumn);
			SetSpriteXY(spriteX, spriteY);
		}
	}
}
