using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime.DynamicDispatching.Emitters;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private VehicleManager vehicleManager;
    [SerializeField] private List<Vector2> points;
    [SerializeField] private Vector3 mapPosition;
    [SerializeField] private float mapAreaRadius;


    private void OnEnable()
    {
        VehicleManager.targetRequired += ExtendPath;
    }

    private void OnDisable()
    {
        VehicleManager.targetRequired -= ExtendPath;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(vehicleManager.GenerateVehicle(result =>
            {
                if (result)
                {
                    var pos = vehicleManager.PatrolVehicle.transform.position;
                    Init(new Vector2(pos.x, pos.z));
                }
            }));
        }
    }


    private void Init(Vector2 _start)
    {
        var mid = GetRandomPos(mapPosition, mapAreaRadius);
        var end = GetRandomPos(mapPosition, mapAreaRadius);

        points = new List<Vector2> {_start, mid, end};

        vehicleManager.AddPath(points[0], points[1], points[2]);
    }

    private void ExtendPath()
    {
        Debug.Log("Extending Vehicle path");
        var tempMid = ConvertToVector3(points[1]);
        var tempEnd = vehicleManager.PatrolVehicle.transform.position;

        var dir = (tempEnd - tempMid).normalized;

        // Begin Ray outside Sphere
        var ray = new Ray(tempEnd, dir);
        ray.origin = ray.GetPoint(10000);
        ray.direction = -ray.direction;
        
        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.green, 100f);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("HIT NAME - " + hit.transform.name + " HIT POINT - " + hit.point);

            var start = ConvertToVector2(tempEnd);
            var mid = ConvertToVector2(hit.point);
            var end = ConvertToVector2(GetRandomPos(mapPosition, mapAreaRadius));
            
            points = new List<Vector2> { start, mid, end };
            vehicleManager.AddPath(points[0], points[1], points[2]);
        }
        else
        {
            Debug.LogWarning("RAYCAST NO HIT");
        }
    }


    private Vector2 GetRandomPos(Vector3 _origin, float _radius)
    {
        var temp = (Random.insideUnitSphere + _origin) * _radius;
        return new Vector2(temp.x, temp.z);
    }


    private void OnDrawGizmos()
    {
        if (points == null || points.Count < 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mapPosition, mapAreaRadius);

        Gizmos.color = Color.white;
        foreach (var point in points)
        {
            Gizmos.DrawWireSphere(new Vector3(point.x, 0f, point.y), 100f);
            Gizmos.color = Color.blue;

            // Debug.Log("Position - " + new Vector3(point.x, 0f, point.y));
        }
    }

    private static Vector3 ConvertToVector3(Vector2 _v)
    {
        return new Vector3(_v.x, 0f, _v.y);
    }
    
    private static Vector3 ConvertToVector2(Vector3 _v)
    {
        return new Vector2(_v.x, _v.z);
    }
}