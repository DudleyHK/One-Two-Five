using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class BikeController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent nvAgent;
    [SerializeField] private PlayerMovement pMovement;



    // Start is called before the first frame update
    private void OnEnable()
    {
        EventManager.AddListener(EventManager.Events.CollectablePlaced, new System.Action<Vector3>(RouteToCollectable));
    }


    private void OnDisable()
    {
        EventManager.RemoveListener(EventManager.Events.CollectablePlaced, new System.Action<Vector3>(RouteToCollectable));

    }


    private void Start()
    {
        nvAgent.updatePosition = true;
        nvAgent.updateRotation = false;
        nvAgent.updateUpAxis = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    // T = D / S

    private void RouteToCollectable(Vector3 cPos)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, cPos, NavMesh.AllAreas, path))
        {
            Vector3 start = transform.position;
            for (int i = 0; i < path.corners.Length; i++)
            {
                Vector3 c = path.corners[i];
                Debug.DrawLine(start, c, Color.yellow);
                start = c;
            }

            nvAgent.SetPath(path);
            nvAgent.isStopped = true;

            Debug.Log("Remaining Distance " + nvAgent.remainingDistance);
        }
        else
        {
            Debug.LogError("Error: Cannot find valid path to Collectable?");
        }



    }






}
