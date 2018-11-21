using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    private CollisionManager collisionManager;
    private ObjectManager objectManager;


    private void Awake()
    {
        collisionManager = new CollisionManager();
        if(collisionManager == null)
            Debug.LogWarning("Warning: CollisionManager creation error!");
        
        objectManager = new ObjectManager();
        if(objectManager == null)
            Debug.LogWarning("Warning: CollisionManager creation error!");
    }
}
