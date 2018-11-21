using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    protected PoolObject()
    {
        EventManager.AddListener(
            EventManager.Events.RegisterObject, 
            new System.Action<PoolObject>(SingletonObjectManager.Instance.RegisterObject));
        
        EventManager.Trigger(EventManager.Events.RegisterObject, new { _obj = this });
    }
}
