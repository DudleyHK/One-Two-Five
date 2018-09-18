using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Vector3 Target { get; set; }
    public float Speed { get; set; }

    [SerializeField] private Vector3 direction;


    private void Start()
    {
        direction = Target - transform.position;
        direction.y = 0f;
        
        Debug.DrawRay(transform.position, direction, Color.green, 100f);
        
        transform.right = direction;
        transform.Rotate(90f, transform.localRotation.y, 0f);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        transform.position += direction.normalized * Speed * Time.fixedDeltaTime;
    }
}