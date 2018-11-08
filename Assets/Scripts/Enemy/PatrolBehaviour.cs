using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour 
{
    public delegate void PatrolSmashPlayer();
    public static event PatrolSmashPlayer patrolSmashPlayer;
    
    private void Update()
    {
        if (Distance(transform.position, PlayerData.PlayerObject.transform.position))
        {
            //if (patrolSmashPlayer != null)
            //    patrolSmashPlayer();
        }
        else
        {
            // Do nothing
        }
    }
    
    
    private bool Distance(Vector3 _a, Vector3 _b)
    {
        return Vector3.Distance(_a, _b) <= 5f;
    }
}
