using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBehavoiur : EnemyBehaviour
{
    public enum State
    {
        None,
        Patrol,
        Chase
    }

    public Transform Target;

    [SerializeField] private State state = State.Patrol;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            default:
                break;
        }
    }


    private void Patrol()
    {
    }


//    private void FixedUpdate()
//    {
//        if (Vector3.Distance(transform.position, Target.position) <= 5f) return;
//        
//        direction = Target.position - transform.position;
//        direction.y = 0f;
//
//        Debug.DrawRay(transform.position, direction, Color.green, 4f);
//
//        transform.right = direction;
//        transform.Rotate(90f, transform.localRotation.y, 0f);
//
//        transform.position += direction.normalized * Speed * Time.fixedDeltaTime;
//    }
}