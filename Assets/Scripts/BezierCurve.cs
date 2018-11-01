using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour 
{
	public Transform[] Positions = new Transform[3];
	public float Speed = 1f;
	
	private static float delta = 0;

	
	public static Vector2 QuadraticBezierCurve(Vector2 _start, Vector2 _mid, Vector2 _end, float _speed, float _t, 
		out bool _complete, out bool _midway)
	{
		_complete = SetDelta(_speed, _t);
		_midway = Midway();
		
		var pos = Mathf.Pow(1 - delta, 2) * _start +
		          2f * (1 - delta) * delta * _mid +
		          Mathf.Pow(delta, 2) * _end;
		
		return new Vector2(pos.x, pos.y);
	}

	
	private static bool SetDelta(float _speed, float _t)
	{
		if (delta <= 1)
		{
			delta += _speed * _t;
			return false;
		}

		delta = 0f;
		return true;
	}

	
	private static bool Midway()
	{
		return delta > 0.499f && delta <= 0.5f;
	}
}
