using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaTest : MonoBehaviour
{
    public Vector2 Target;
    public float Curve;

    private float theta;


    private void Update()
    {
        if(Vector2.Distance(transform.position, Target) >= 0.5f)
        {
            // theta = Time.deltaTime;
            // theta = theta % 5f;

            transform.position = TweenLibrary.Parabola(transform.position, Target * 10f, Curve, Time.deltaTime);
        }
    }
}
