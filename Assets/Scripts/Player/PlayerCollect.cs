using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour 
{
    public delegate void Collected(CollectablesManager.CollectableType _collectableType, ushort _score, int _ID);
    public static event Collected collected;
	
	
    private void OnCollisionEnter(Collision _other)
    {
        var script = _other.gameObject.GetComponent<Collectable>();
     
        if (_other.gameObject.CompareTag(ObjectNames.Collectable))
        {
            if (script == null)
            {
                Debug.LogWarning("Warning: CollectableType is missing CollectableType script.");
                return;
            }

            PlayerData.Score += script.Score;

            if (collected != null)
                collected(script.Type, script.Score, script.ID);
            
            // Debug.Log("Collected type " + script.Type + " at position " + _other.transform.position);
        }
    }
}
