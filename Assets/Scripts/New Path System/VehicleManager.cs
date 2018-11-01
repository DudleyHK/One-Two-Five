using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleManager : MonoBehaviour
{
    public delegate void TargetRequired();

    public static event TargetRequired targetRequired;

    public List<VehiclePath> Path = new List<VehiclePath>();
    public GameObject EnemyPrefab;
    public SpriteRenderer Map;
    public GameObject Target;

    [SerializeField] public GameObject PatrolVehicle;

    [SerializeField] private VehiclePath currentPath;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damping = 150f;
    [SerializeField] private float midpointRadius = 100f;
    [SerializeField] private bool complete = false;
    [SerializeField] private bool midway = false;
    [SerializeField] private bool slow = false;
    [SerializeField] private Vector2 start, mid, end;

    private Vector3 dir;
    private int index;
    [SerializeField] private bool stopBike = false;
    [SerializeField] private float turn = 50f;

    [SerializeField] private float timer = 0f;
    [SerializeField] private float maxtime = 3f;
    [SerializeField] private Vector3 target;

    private float temp;
    private Vector3 currentAngle = Vector3.zero;


    private void Start()
    {
        currentAngle = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        if (timer <= maxtime)
        {
            timer += Time.deltaTime;
            if (PatrolVehicle != null)
            {
                if (Vector3.Distance(PatrolVehicle.transform.position, target) <= 500f)
                {
                    if (!stopBike)
                    {
                        Debug.Log("STOP");
                        StopCoroutine(StopBike());
                        StartCoroutine(StopBike());

                        stopBike = true;
                    }
                }
                else
                {
                    Debug.Log("MOVING");
                    var direction = target - PatrolVehicle.transform.position;
                    direction.y = 0f;

                    Debug.DrawRay(PatrolVehicle.transform.position, direction, Color.green, 4f);

                    var offset = new Vector2(target.x - PatrolVehicle.transform.position.x,
                        target.z - PatrolVehicle.transform.position.z);
                    var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                    var targetAngle = new Vector3(90f, 0f, angle);

                    currentAngle = new Vector3(
                        Mathf.LerpAngle(currentAngle.x, targetAngle.x, turn * Time.fixedDeltaTime),
                        Mathf.LerpAngle(currentAngle.y, targetAngle.y, turn * Time.fixedDeltaTime),
                        Mathf.LerpAngle(currentAngle.z, targetAngle.z, turn * Time.fixedDeltaTime));

                    PatrolVehicle.transform.eulerAngles = currentAngle;
                    PatrolVehicle.transform.position += direction.normalized * speed * Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            target = GetRandomPos(Vector3.zero, 4000f);
            timer = 0f;
        }


        // FollowPath();
    }

    private Vector3 GetRandomPos(Vector3 _origin, float _radius)
    {
        var temp = (Random.insideUnitSphere + _origin) * _radius;
        return new Vector3(temp.x, 0f, temp.z);
    }


    public void AddPath(Vector2 _start, Vector2 _mid, Vector2 _end)
    {
        Path.Add(new VehiclePath(_start, _mid, _end));
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

        start = Path[index].Start;
        mid = Path[index].Mid;
        end = Path[index].End;

        var pos = BezierCurve.QuadraticBezierCurve(start, mid, end, speed, Time.fixedDeltaTime,
            out complete,
            out midway);

        if (midway && targetRequired != null)
            targetRequired();

        Rotate();
        PatrolVehicle.transform.position = new Vector3(pos.x, 0f, pos.y);

        if (complete)
        {
            complete = false;
            index++;
        }

        Debug.DrawLine(PatrolVehicle.transform.position, new Vector3(Path[index].End.x, 0f, Path[index].End.y),
            Color.cyan, 1f);
    }

    private void Rotate()
    {
        dir = new Vector3(Path[0].End.x, 0f, Path[0].End.y) - PatrolVehicle.transform.position;
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
            PatrolVehicle.transform.position += PatrolVehicle.transform.right * temp * Time.fixedDeltaTime;

            Debug.DrawLine(PatrolVehicle.transform.position,
                PatrolVehicle.transform.position + PatrolVehicle.transform.right * 10f,
                Color.green, 10f);

            yield return false;
        }

        stopBike = false;
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