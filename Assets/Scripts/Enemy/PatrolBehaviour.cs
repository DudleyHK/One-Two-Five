using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour 
{
    private void OnCollisionEnter(Collision _other)
    {
        var n = _other.collider.name;
        Debug.Log("OTHER - " + n);

        if (n.Contains(ObjectNames.Bud) || n.Contains(ObjectNames.Kamakazi)) return;

        if (n.Contains(ObjectNames.Player))
        {
            ExplosionManager.InstantiateExplosion(transform.position);
            Destroy(gameObject);
        }
		
    }
}
