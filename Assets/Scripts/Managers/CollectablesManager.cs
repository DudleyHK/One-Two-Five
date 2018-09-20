using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
///  Turn collectablesmanager into object factory. 
/// </summary>
public class CollectablesManager : MonoBehaviour
{
    public enum CollectableType
    {
        None,
        Bud
    }

    [SerializeField] private List<GameObject> collectablePrefabs = new List<GameObject>();
    [SerializeField] private List<Collectable> collectableList = new List<Collectable>();

    private float timer = 0;
    private float dropIntervalTime = 5f;


    private void OnEnable()
    {
        PlayerCollect.collected += HandleCollection;
    }

    private void OnDisable()
    {
        PlayerCollect.collected -= HandleCollection;
    }


    private void Start()
    {
        timer = dropIntervalTime;
    }


    private void Update()
    {
        if (timer <= 0)
        {
            // Randomly place around the map.
            var x = Random.Range(0f, Camera.main.pixelWidth);
            var y = Random.Range(0f, Camera.main.pixelHeight);

            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0f));

            var idx = Random.Range(0, collectablePrefabs.Count);
            var collectable = Instantiate(collectablePrefabs[idx], new Vector3(worldPos.x, 0f, worldPos.z),
                Quaternion.Euler(90f, 0f, 0f));

            var script = collectable.GetComponent<Collectable>();
            script.ID = collectableList.Count;
            
          //  Debug.Log("ID - " + script.ID);

            collectableList.Add(script);
            timer = dropIntervalTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


    private void HandleCollection(CollectableType _collectableType, ushort _score, int _ID)
    {
        if (collectableList.Count <= 0)
            return;

        collectableList[_ID].gameObject.SetActive(false);
    }
}