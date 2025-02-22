﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/* // ENEMIES //
- Set a collision point to the position of the player.
- Select two rand positions offscreen.
- Add ray collision test before instantiation, repeat previous step if not.   
- Enemies drive from random off screen positions (inc. a set offset position on x and y)
- Get maxSpeed which both bikes.
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
    public enum Type
    {
        None,
        Chaser,
        Kamakazi
    }

    public static bool KamakaziActive = true;

    [Range(0f, 15f)] public static float ChaserSpeed = 5f;
    [Range(1f, 300f)] public static float ChaserInterval = 10f;
    [Range(1f, 300f)] public static float KamakaziInterval = 5f;
    
    public static List<EnemyBehaviour> EnemyList = new List<EnemyBehaviour>();

    [SerializeField] private GameObject kamakaziEnemyPrefab;
    [SerializeField] private GameObject collisionRetical;
    [SerializeField] private GameObject player;
    [SerializeField] private Text debugText;
    [SerializeField] private Text enemySpawnIntervalsText;
    [SerializeField] private float destroyReticalSeconds;
    [SerializeField] private bool gate;

    [SerializeField] private int attackEnemiesNumber = 2;
    [SerializeField] private float timeBeforeCollision = 5f;


    
    private SpriteRenderer reticalSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;
    private Vector3 collisionReticalPosition;
    private Sides[] sides;

    private float kamakaziTimer = 0f;
    private float chaserTimer = 0f;
    private float offset = 50f;


    private void Start()
    {
        reticalSpriteRenderer = collisionRetical.GetComponent<SpriteRenderer>();
        if (reticalSpriteRenderer == null)
        {
            Debug.LogWarning("Warning: reticalSpriteRenderer not found!");
        }

        enemySpriteRenderer = kamakaziEnemyPrefab.GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer == null)
        {
            Debug.LogWarning("Warning: enemySpriteRenderer not found!");
        }

        kamakaziTimer = KamakaziInterval;
    }


    private void Update()
    {
        UpdateKamakazi();
    }


    private void UpdateKamakazi()
    {
        if (!KamakaziActive) return;
        if (gate || kamakaziTimer <= 0f)
        {
            InitCollisionPoint();
            SpawnEnemies(Type.Kamakazi, attackEnemiesNumber);

            gate = false;
            kamakaziTimer = KamakaziInterval;
        }
        else
        {
            kamakaziTimer -= Time.deltaTime;
        }

        enemySpawnIntervalsText.text = "Kamakazi Enemies Spawn Intervals Timer: " + kamakaziTimer;
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
        var retical = Instantiate(collisionRetical, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(90f, 0f, 0f));
        Destroy(retical, destroyReticalSeconds);

        collisionReticalPosition = pos;

        debugText.text = "collision point (world position) -" + pos;
    }


    private void SpawnEnemies(Type _type, int _numberOfEnemies)
    {
        StartCoroutine(SelectScreenSides(result =>
        {
            if (result)
            {
                var enemyOffsetX = enemySpriteRenderer.sprite.bounds.size.x + offset;
                var enemyOffsetY = enemySpriteRenderer.sprite.bounds.size.y + offset;
                var screenPos = Vector2.zero;

                bool parentSelected = true;

                for (int i = 0; i < _numberOfEnemies; i++)
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

                        if (_type == Type.Kamakazi)
                        {
                            var dist = Vector3.Distance(collisionReticalPosition, worldPos);
                            var speed = dist / timeBeforeCollision;

                            InstantiateEnemy(kamakaziEnemyPrefab, worldPos, collisionReticalPosition, speed,
                                parentSelected);
                            
                            parentSelected = false;

                            //Debug.Log("New Kamakazi Enemy ~~~ \n - Pos: " + worldPos +
                            //          " \n - Dist: " + dist +
                            //          " \n Speed: " + speed +
                            //          " \n Target: " + collisionReticalPosition);
                        }
                    }
                    else
                    {
                        //Debug.Log("Position (" + screenPos + ") NOT offscreen.");
                        i--;
                    }
                }

                var index = EnemyList.Count - 1;
                EnemyList[index].Partner = EnemyList[index - 1].gameObject;
                EnemyList[index - 1].Partner = EnemyList[index].gameObject;

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


    private void InstantiateEnemy(GameObject _prefab, Vector3 _pos, Vector3 _target, float _speed, bool _main)
    {
        var enemyBehaviour = Instantiate(_prefab, new Vector3(_pos.x, 0f, _pos.z), Quaternion.Euler(90f, 0f, 0f))
            .GetComponent<EnemyBehaviour>();
        enemyBehaviour.Speed = _speed;
        enemyBehaviour.Target = _target;
        enemyBehaviour.Main = _main;

        EnemyList.Add(enemyBehaviour);
    }
}