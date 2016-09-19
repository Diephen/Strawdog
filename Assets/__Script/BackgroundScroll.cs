using UnityEngine;
using System.Collections;

public class BackgroundScroll : MonoBehaviour
{
	public float tileSizeX;

	Vector3 _bgPos;

	void Start ()
	{
		_bgPos = transform.position;
	}

	void Update ()
	{
		if (Camera.main.transform.position.x > _bgPos.x + tileSizeX) {
			_bgPos.x += tileSizeX * 2;
			transform.position = _bgPos;
		} else if (Camera.main.transform.position.x < _bgPos.x - tileSizeX) {
			_bgPos.x -= tileSizeX * 2;
			transform.position = _bgPos;
		}
	}
}