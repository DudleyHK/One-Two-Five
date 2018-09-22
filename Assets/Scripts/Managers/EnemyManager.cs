using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text enemySpawnIntervalsText;
    
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject collisionRetical;
    [SerializeField] private GameObject player;

    [SerializeField] private int attackEnemiesNumber = 2;
    [SerializeField] private float timeBeforeCollision = 5f;
    [SerializeField] private float spawnIntervals = 60f;

    [SerializeField] private bool gate = false;

    private List<GameObject> collisionReticalList = new List<GameObject>();
    private List<EnemyBehaviour> enemyList = new List<EnemyBehaviour>();
    private SpriteRenderer reticalSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;
    private Vector3 collisionReticalPosition;
    private float timer = 0f;
    
    private float offset = 50f;

    private Sides[] sides;


    private void Start()
    {
        reticalSpriteRenderer = collisionRetical.GetComponent<SpriteRenderer>();
        if (reticalSpriteRenderer == null)
        {
            Debug.LogWarning("Warning: reticalSpriteRenderer not found!");
        }
        
        enemySpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer == null)
        {
            Debug.LogWarning("Warning: enemySpriteRenderer not found!");
        }

        timer = spawnIntervals;
    }


    private void Update()
    {
        if (gate || timer <= 0f)
        {
            InitCollisionPoint();
            SpawnEnemies();

            gate = false;
            timer = spawnIntervals;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        enemySpawnIntervalsText.text = "Enemy Spawn Intervals Timer: " + timer;
    }

    private void InitCollisionPoint()
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

    
    
    private void SpawnEnemies()
    {
        StartCoroutine(SelectScreenSides(result =>
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

    
    
    private IEnumerator SelectScreenSides(System.Action<bool> _flag)
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


    
    private bool Offscreen(float _x, float _y)
    {
        if (_x >= 0f && _x <= Camera.main.pixelWidth &&
            _y >= 0f && _y <= Camera.main.pixelHeight)
        {
            return false;
        }
        return true;
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
}