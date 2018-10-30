using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehiclePath
{
    public Vector2 Start;
    public Vector2 Mid;
    public Vector2 End;

    
    public VehiclePath(Vector2 _start, Vector2 _mid, Vector2 _end)
    {
        Start = _start;
        Mid = _mid;
        End = _end;
    }

}
