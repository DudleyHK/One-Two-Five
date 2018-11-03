using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolEnemy : MonoBehaviour
{
    public enum PatrolEnemyStates
    {
        None,
        Patrol,
        Chase
    }

    [HideInInspector] public GameObject PatrolVehicle;
    [HideInInspector] public PatrolEnemyStates State;

    public GameObject Player;
    public GameObject EnemyPrefab;
    public SpriteRenderer Map;

    [SerializeField] private Vector3 target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float maxPlayerDistance;
    [SerializeField] private bool stopBike;

    [SerializeField] private float maxtime = 3f;

    private Vector3 currentAngle;
    private float timer;


    private void Start()
    {
        currentAngle = transform.eulerAngles;

        StartCoroutine(GenerateVehicle(n => { }));
    }


    private void FixedUpdate()
    {
        State = SetState();

        switch (State)
        {
            case PatrolEnemyStates.Patrol:
                Patrol();
                break;
            case PatrolEnemyStates.Chase:
                timer = maxtime;
                Chase();
                break;
        }
    }

    private PatrolEnemyStates SetState()
    {
        if (Vector3.Distance(PatrolVehicle.transform.position, Player.transform.position) <= maxPlayerDistance)
            return PatrolEnemyStates.Chase;
        return PatrolEnemyStates.Patrol;
    }


    private void Patrol()
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
                    var dir = target - PatrolVehicle.transform.position;
                    dir.y = 0f;

                    Rotate();

                    PatrolVehicle.transform.position += dir.normalized * moveSpeed * Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            target = GetRandomPos(Vector3.zero, 4000f);
            timer = 0f;
        }
    }


    private void Chase()
    {
        var dir = Player.transform.position - PatrolVehicle.transform.position;
        dir.y = 0f;

        Debug.DrawRay(PatrolVehicle.transform.position, dir, Color.green, 4f);

        PatrolVehicle.transform.right = dir;
        PatrolVehicle.transform.Rotate(90f, PatrolVehicle.transform.localRotation.y, 0f);

        PatrolVehicle.transform.position += dir.normalized * chaseSpeed * Time.fixedDeltaTime;
    }


    private void Rotate()
    {
        var offset = new Vector2(target.x - PatrolVehicle.transform.position.x,
            target.z - PatrolVehicle.transform.position.z);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var targetAngle = new Vector3(90f, 0f, angle);

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, turnSpeed * Time.fixedDeltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, turnSpeed * Time.fixedDeltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, turnSpeed * Time.fixedDeltaTime));

        PatrolVehicle.transform.eulerAngles = currentAngle;
    }


    private Vector3 GetRandomPos(Vector3 _origin, float _radius)
    {
        var temp = (Random.insideUnitSphere + _origin) * _radius;
        return new Vector3(temp.x, 0f, temp.z);
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


    private IEnumerator StopBike()
    {
        var temp = slowSpeed;
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
        if (PatrolVehicle != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(PatrolVehicle.transform.position, maxPlayerDistance);
        }
    }
}