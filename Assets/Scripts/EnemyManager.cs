using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Persistence;


/* // ENEMIES //
- Set a collision point to the position of the player.
- Select two rand positions offscreen.
- Add ray collision test before instantiation, repeat previous step if not.   
- Enemies drive from random off screen positions (inc. a set offset position on x and y)
- Get speed which both bikes.
- Add screen shake on collision
- Explosion Animation
- SFX



*/

public enum Sides
{
    None,
    Top,
    Bottom,
    Left,
    Right
};


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Text debugText;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject collisionRetical;
    [SerializeField] private GameObject player;

    [SerializeField] private int attackEnemiesNumber = 2;
    [SerializeField] private float timeBeforeCollision = 5f;

    [SerializeField] private bool gate = false;

    private List<GameObject> collisionReticalList = new List<GameObject>();
    private List<EnemyBehaviour> enemyList = new List<EnemyBehaviour>();
    private SpriteRenderer reticalSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;
    private Vector3 collisionReticalPosition;
    private float timer = 0f;
    private float maxTime = 60f;
    private float offset = 50f;
    private float maxSpawnDist = 5f;

    private Sides[] sides;


    private void Start()
    {
        reticalSpriteRenderer = collisionRetical.GetComponent<SpriteRenderer>();
        enemySpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();


        timer = maxTime;
    }


    private void Update()
    {
        if (gate || timer <= 0f)
        {
            SpawnPoint();
            SpawnEnemies();

            gate = false;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


    private void SpawnEnemies()
    {
        StartCoroutine(SelectSides(result =>
        {
            if (result)
            {
                var enemyOffsetX = enemySpriteRenderer.sprite.bounds.size.x + offset;
                var enemyOffsetY = enemySpriteRenderer.sprite.bounds.size.y + offset;

                var screenPos = Vector2.zero;
                
                Debug.Log("Side 1  - " + sides[0] + " Side 2 - " + sides[1]);

                for (int i = 0; i < attackEnemiesNumber; i++)
                {
                    switch (sides[i])
                    {
                        case Sides.Top:
                            screenPos.x = Random.Range(0f - enemyOffsetX, Camera.main.pixelWidth + enemyOffsetX);
                            screenPos.y = Camera.main.pixelHeight + enemyOffsetY;
                            break;

                        case Sides.Bottom:
                            screenPos.x = Random.Range(0f - enemyOffsetX, Camera.main.pixelWidth + enemyOffsetX);
                            screenPos.y = 0f - enemyOffsetY;
                            break;
                        
                        case Sides.Left:
                            screenPos.x = 0f - enemyOffsetX;
                            screenPos.y = Random.Range(0f - enemyOffsetY, Camera.main.pixelHeight + enemyOffsetY);
                            break;
                        
                        case Sides.Right:
                            screenPos.x = Camera.main.pixelWidth + enemyOffsetX;
                            screenPos.y = Random.Range(0f - enemyOffsetY, Camera.main.pixelHeight + enemyOffsetY);
                            break;
                        
                        default:
                            Debug.LogWarning("Warning: Invalid Side (" + sides[i] + ") entered!");
                            break;
                    }

                    
                    if (Offscreen(screenPos.x, screenPos.y))
                    {
                        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
                        InstantiateEnemy(worldPos);
                    }
                    else
                    {
                        Debug.Log("Position (" + screenPos + ") NOT offscreen.");
                        i--;
                    }

                }
            }
        }));
    }


    private IEnumerator SelectSides(System.Action<bool> _flag)
    {
        bool complete = false;

        // TODO: Optimise. 
        sides = new Sides[attackEnemiesNumber];

        while (!complete)
        {
            complete = true;

            for (int i = 0; i < attackEnemiesNumber; i++)
            {
                sides[i] = (Sides) Random.Range(1, 5);

                _flag(false);
                yield return null;
            }

            // Validate both sides are different
            var first = sides[0];
            for (int i = 0; i < sides.Length; i++)
            {
                if (i == 0)
                    continue;

                if (sides[i] == first)
                    complete = false;

                _flag(false);
                yield return null;
            }

            _flag(false);
            yield return null;
        }

        _flag(true);
        yield return true;
    }


    private void SpawnPoint()
    {
        var reticalOffsetX = reticalSpriteRenderer.sprite.bounds.size.x + offset;
        var reticalOffsetY = reticalSpriteRenderer.sprite.bounds.size.y + offset;

        var x = Random.Range(0f + reticalOffsetX, Camera.main.pixelWidth - reticalOffsetX);
        var y = Random.Range(0f + reticalOffsetY, Camera.main.pixelHeight - reticalOffsetY);

        var pos = Random.Range(0f, 1f) <= 0.3f
            ? player.transform.localPosition
            : Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0f));
        var clone = Instantiate(collisionRetical, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(90f, 0f, 0f));

        collisionReticalPosition = pos;

        collisionReticalList.Add(clone);

        debugText.text = "collision point (world position) -" + pos;
        Debug.Log(debugText.text);
    }


    private void InitEnemies()
    {
        enemyList.Clear();

        StartCoroutine(InstantiateAttackEnemies());
    }


    private IEnumerator InstantiateAttackEnemies()
    {
        int i = 0;

        StartCoroutine(EnemySpawn(n =>
        {
            if (n) i++;
        }));

        while (enemyList.Count <= attackEnemiesNumber)
        {
            if (enemyList.Count != i)
            {
                StartCoroutine(EnemySpawn(n =>
                {
                    if (n) i++;
                }));
            }

            Debug.Log("Enemy Count - " + enemyList.Count + " and i - " + i);
            yield return null;
        }

        yield return true;
    }


    private bool Offscreen(float _x, float _y)
    {
        if (_x >= 0f && _x <= Camera.main.pixelWidth &&
            _y >= 0f && _y <= Camera.main.pixelHeight)
        {
            return false;
        }
        return true;
    }


    private IEnumerator EnemySpawn(System.Action<bool> _result)
    {
        Vector3 enemyWorldPos = Vector3.zero;
        bool valid = false;

        var enemyOffsetX = enemySpriteRenderer.sprite.bounds.size.x + offset;
        var enemyOffsetY = enemySpriteRenderer.sprite.bounds.size.y + offset;

        while (!valid)
        {
            var x = Random.Range(0f - enemyOffsetX, Camera.main.pixelWidth + enemyOffsetX);
            var y = Random.Range(0f - enemyOffsetY, Camera.main.pixelHeight + enemyOffsetY);

            if (x >= 0f && x <= Camera.main.pixelWidth &&
                y >= 0f && y <= Camera.main.pixelHeight)
            {
                Debug.Log("Position inside Screen - " + ScreenToWorld(new Vector3(x, y, 0f)));
                valid = false;
            }
            else if (enemyList.Count > 0)
            {
                var dist = Vector3.Distance(enemyList[enemyList.Count - 1].transform.position,
                    ScreenToWorld(new Vector3(x, y, 0f)));

                if (dist >= maxSpawnDist)
                {
                    Debug.Log("Distance is " + dist);
                    valid = false;
                }
            }
            else
            {
                enemyWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0f));
                valid = true;
            }

            _result(false);
            yield return null;
        }

        InstantiateEnemy(enemyWorldPos);

        _result(true);
        yield return true;
    }


    private void InstantiateEnemy(Vector3 _pos)
    {
        var dist = Vector3.Distance(collisionReticalPosition, _pos);
        var speed = dist / timeBeforeCollision;

        var enemyBehaviour = Instantiate(enemyPrefab, new Vector3(_pos.x, 0f, _pos.z), Quaternion.Euler(90f, 0f, 0f))
            .GetComponent<EnemyBehaviour>();
        enemyBehaviour.Speed = speed;
        enemyBehaviour.Target = collisionReticalPosition;

        enemyList.Add(enemyBehaviour);

        Debug.Log("New Enemy ~~~ \n - Pos: " + _pos +
                  " \n - Dist: " + dist +
                  " \n Speed: " + speed +
                  " \n Target: " + collisionReticalPosition);
    }


    private Vector3 ScreenToWorld(Vector3 _p)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(_p.x, _p.y, 0f));
    }
}