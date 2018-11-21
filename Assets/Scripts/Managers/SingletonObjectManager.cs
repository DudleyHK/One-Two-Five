using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingletonObjectManager
{
    public static SingletonObjectManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SingletonObjectManager();

            return instance;
        }
    }
    
    private List<PoolObject> objectList = new List<PoolObject>();
    private static SingletonObjectManager instance;

    
    
    public List<PoolObject> Update()
    {
        // Have any objects been instantiated into the game?
        // Add them to an null positions in the list.
        // If not push back.

        return objectList;
    }


    public void RegisterObject(PoolObject _obj)
    {
        Debug.Log("Object added");
        Instance.objectList.Add(_obj);
    }
    
    
}
