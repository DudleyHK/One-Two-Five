using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehiclePath
{
    public Vector2 Start;
    public Vector2 Mid;
    public Vector2 End;

    public bool Completed;
    public bool MidwayReached;

    public bool Final;


    public VehiclePath(Vector2 _start, Vector2 _mid, Vector2 _end, bool _final = false)
    {
        Start = _start;
        Mid = _mid;
        End = _end;

        Final = _final;

        Completed = false;
        MidwayReached = false;
    }
}