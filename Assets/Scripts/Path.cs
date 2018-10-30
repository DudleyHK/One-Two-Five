using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Path
{
	[SerializeField, HideInInspector] private List<Vector2> points = new List<Vector2>();
	[SerializeField] private float radius = 5f;



	public Path(Vector3 _start, Vector3 _end)
	{
		points.Add(new Vector2(_start.x, _start.z));

		var handle = _start + Random.insideUnitSphere * radius;
		points.Add(new Vector2(handle.x, handle.z));

		handle = _end + Random.insideUnitSphere * radius;
		points.Add(new Vector2(handle.x, handle.z));

		points.Add(new Vector2(_end.x, _end.z));

		
		AddSegment(Vector3.zero);
		
		int i = 0;
		foreach (var point in points)
		{
			var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			obj.transform.position = new Vector3(point.x, 0f, point.y);
			obj.transform.localScale *= 100f;
			obj.name = "Point " + i++;
		}
	}


	public void AddSegment(Vector3 _pos)
	{
		var temp = new Vector2(_pos.x, _pos.z);
		
		points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
		points.Add((points[points.Count - 1] + temp) * 0.5f);
		points.Add(temp);
	}

}
