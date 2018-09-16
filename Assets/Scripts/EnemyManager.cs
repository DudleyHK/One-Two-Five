using System.Collections;
using System.Collections.Generic;
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


public class EnemyManager : MonoBehaviour
{
    public static Quaternion ObjectRotation
    {
        get { return Quaternion.Euler(90f, 0f, 0f); }
    }

    [SerializeField] private Text debugText;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject collisionRetical;
    [SerializeField] private GameObject player;

    [SerializeField] private float timeBeforeCollision = 5f;

    [SerializeField] private bool gate = false;


    private List<GameObject> collisionReticalList = new List<GameObject>();
    private SpriteRenderer collisionReticalRenderer;

    private Vector3 enemyPos;
    private Vector3 collisionReticalPosition;

    private float offsetX;
    private float offsetY;
    private float timer = 0f;
    private float maxTime = 60f;
    private float offset = 10f;


    private void Start()
    {
        collisionReticalRenderer = collisionRetical.GetComponent<SpriteRenderer>();

        offsetX = collisionReticalRenderer.sprite.bounds.size.x + 50f;
        offsetY = collisionReticalRenderer.sprite.bounds.size.y + 50f;

        timer = maxTime;
    }


    private void Update()
    {
        if (gate || timer <= 0f)
        {
            SpawnPoint();
            InitEnemies();

            gate = false;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


    private void SpawnPoint()
    {
        var pos = Random.Range(0f, 1f) <= 0.3f ? player.transform.localPosition : RandomPoint();
        var clone = Instantiate(collisionRetical, new Vector3(pos.x, 0f, pos.z), ObjectRotation);

        collisionReticalPosition = pos;

        collisionReticalList.Add(clone);

        debugText.text = "collision point (world position) -" + pos;
        Debug.Log(debugText.text);
    }


    private Vector3 RandomPoint()
    {
        // get random position in pixels. 
        var x = Random.Range(0f + offsetX, Camera.main.pixelWidth - offsetX);
        var y = Random.Range(0f + offsetY, Camera.main.pixelHeight - offsetY);

        return Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0f));
    }


    private void InitEnemies()
    {
        StartCoroutine(EnemySpawn());
    }


    private IEnumerator EnemySpawn()
    {
        bool valid = false;
        while (!valid)
        {
            var x = Random.Range(0f - offsetX, Camera.main.pixelWidth + offsetX);
            var y = Random.Range(0f - offsetY, Camera.main.pixelHeight + offsetY);

            if (x >= 0f && x <= Camera.main.pixelWidth && 
                y >= 0f && y <= Camera.main.pixelHeight)
            {
                valid = false;
            }
            else
            {
                enemyPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0f));
                valid = true;
            }
            
            yield return null;
        }

        InstantiateEnemy(enemyPos);

        yield return true;
    }


    private void InstantiateEnemy(Vector3 _pos)
    {
        var dist = Vector3.Distance(collisionReticalPosition, enemyPos);
        var speed = dist / timeBeforeCollision;


        var inst = Instantiate(enemyPrefab, new Vector3(_pos.x, 0f, _pos.z), ObjectRotation);
        inst.GetComponent<EnemyBehaviour>().Speed = speed;
        inst.GetComponent<EnemyBehaviour>().Target = collisionReticalPosition;

        Debug.Log("Enemy A ~~~ \n - Pos: " + enemyPos + " \n - Dist: " + dist + " \n Speed: " + speed);
    }
}