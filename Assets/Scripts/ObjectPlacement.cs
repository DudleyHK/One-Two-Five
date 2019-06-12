using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class ObjectPlacement : MonoBehaviour
{

    [SerializeField] private float radius;



    private void Start()
    {
       

    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 rv2 = Random.insideUnitCircle * radius;
            Vector3 pos = new Vector3(rv2.x, 0f, rv2.y); 


            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                pos = hit.position;
            }
            else
            {
                Debug.LogError("Error: Position not found within area?");
            }

            GameObject prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            prim.transform.position = pos;
            prim.name = "Collectable";


            EventManager.Trigger(EventManager.Events.CollectablePlaced, new { cPos = pos });
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }










}
