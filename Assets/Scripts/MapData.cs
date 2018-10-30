using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour 
{
    public Vector3 Top;
    public Vector3 Bottom;
    public Vector3 Left;
    public Vector3 Right;
    public Vector3 Centre;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Warning: Map SpriteRenderer object missing!");
            return;
        }

        var bounds = spriteRenderer.bounds;
        
       // Top = new Vector3(bounds.center)
        
        


    }
}
