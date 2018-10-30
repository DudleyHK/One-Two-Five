using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private VehicleManager vehicleManager;

    [SerializeField] private List<Vector2> points;
    [SerializeField] private Vector3 mapPosition = Vector3.zero;
    [SerializeField] private float mapAreaRadius;
    //[SerializeField] private float vehicleAreaRadius;


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
            StartCoroutine(vehicleManager.GenerateVehicle(n =>
            {
                if (n)
                {
                    var pos = vehicleManager.PatrolVehicle.transform.position;
                    Init(new Vector2(pos.x, pos.z));
                }
            }));
        }
    }


    private void Init(Vector2 _start)
    {
        points = new List<Vector2>();

        var mid = GetRandomPos(mapPosition, mapAreaRadius);
        var end = GetRandomPos(mapPosition, mapAreaRadius);

        points.Add(_start);
        points.Add(mid);
        points.Add(end);
        
        vehicleManager.AddPath(points[0], points[1], points[2]);
    }


    private void ExtendPath()
    {
        Debug.Log("Extending Vehicle path");
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
}