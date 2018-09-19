using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    
    
    private void FixedUpdate()
    {
        var targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        
        transform.position = TweenLibrary.EaseInOutLinear(transform.position, targetPos, speed * Time.fixedDeltaTime);


    }
}
