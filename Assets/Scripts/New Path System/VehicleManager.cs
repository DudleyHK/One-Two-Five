using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleManager : MonoBehaviour
{
    public delegate void TargetRequired();

    public static event TargetRequired targetRequired;

    public List<Vector2[]> Path = new List<Vector2[]>();
    public GameObject EnemyPrefab;
    public SpriteRenderer Map;
    public GameObject Target;

    [HideInInspector] public GameObject PatrolVehicle;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float damping = 150f;
    [SerializeField] private float midpointRadius = 100f;
    [SerializeField] private bool complete = false;
    [SerializeField] private bool midway = false;
    [SerializeField] private bool slow = false;
    [SerializeField] private Vector2 start, mid, end;

    private Vector3 dir;


    private void FixedUpdate()
    {
        FollowPath();

        // if(PatrolVehicle != null)
        //     Debug.Log("Position - " + PatrolVehicle.transform.position);
    }


    public void AddPath(Vector2 _start, Vector2 _mid, Vector2 _end)
    {
        Path.Add(new[] {_start, _mid, _end});
    }


    public IEnumerator GenerateVehicle(System.Action<bool> _flag)
    {
        Vector3 position = Vector2.zero;

        var carSizeX = EnemyPrefab.GetComponent<SpriteRenderer>().bounds.size.x + 10f;
        var carSizeY = EnemyPrefab.GetComponent<SpriteRenderer>().bounds.size.y + 10f;

        var xMin = Map.sprite.bounds.min.x - carSizeX;
        var xMax = Map.sprite.bounds.max.x + carSizeX;
        var yMin = Map.sprite.bounds.min.y - carSizeY;
        var yMax = Map.sprite.bounds.max.y + carSizeY;

        PatrolVehicle = null;

        while (PatrolVehicle == null)
        {
            position.x = Random.Range(xMin, xMax);
            position.z = Random.Range(yMin, yMax);
            position.y = Map.transform.position.y;

            if (!Map.sprite.bounds.Contains(new Vector3(position.x, position.z)))
                PatrolVehicle = Instantiate(EnemyPrefab, position, EnemyPrefab.transform.rotation);

            _flag(false);
        }

        _flag(true);
        yield return true;
    }


    private void FollowPath()
    {
        if (PatrolVehicle == null || Path == null || Path.Count <= 0) return;
        if (complete) return;

        start = Path[0][0];
        mid = Path[0][1];
        end = Path[0][2];

        var pos = BezierCurve.QuadraticBezierCurve(start, mid, end, speed, Time.fixedDeltaTime, out complete,
            out midway); 


        if (midway && targetRequired != null)
            targetRequired();


        if (complete)
        {
            // StopCoroutine(StopBike());
            // StartCoroutine(StopBike());

            return;
        }

        Rotate();
        PatrolVehicle.transform.position = new Vector3(pos.x, 0f, pos.y);

        // Debug.DrawRay(PatrolVehicle.transform.position, dir, Color.black, 1f);
    }

    private void Rotate()
    {
        dir = new Vector3(Path[0][2].x, 0f, Path[0][2].y) - PatrolVehicle.transform.position;
        dir.y = 0f;

        PatrolVehicle.transform.right = dir;
        PatrolVehicle.transform.Rotate(90f, PatrolVehicle.transform.localRotation.y, 0f);
    }


    private bool Midpoint(Vector2 _pos, Vector2 _midpoint, float _d)
    {
        return Vector2.Distance(_pos, _midpoint) <= _d;
    }


    private IEnumerator StopBike()
    {
        var temp = damping;
        var rate = temp / 2f;

        while (temp >= 0f)
        {
            temp -= rate * Time.fixedDeltaTime;
            PatrolVehicle.transform.position += transform.right * temp * Time.fixedDeltaTime;

            Debug.DrawLine(PatrolVehicle.transform.position, PatrolVehicle.transform.position + transform.right * 10f,
                Color.green, 10f);

            yield return false;
        }

        yield return true;
    }


    private void OnDrawGizmos()
    {
        if (PatrolVehicle == null) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(PatrolVehicle.transform.position, 100f);


        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(mid, midpointRadius);
    }
}