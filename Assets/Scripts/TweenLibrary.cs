using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngineInternal;

public class TweenLibrary : ScriptableObject
{
    public static Vector3 LinearTween(Vector3 _scalar, Vector3 _target, float _change = .1f)
    {
        return _scalar + (_target - _scalar) * _change;
    }

    /// <summary>
    /// UNTESTED.
    /// </summary>
    /// <param name="_startVal"></param>
    /// <param name="_change"></param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    public static float QuadraticEaseInOut(float _startVal, float _change, float _duration)
    {
        var t = Time.time / (_duration / 2);

        if (t < 1)
            return _change / 2 * Time.time * Time.time + _startVal;

        t--;

        return -_change / 2 * (Time.time * (Time.time - 2) - 1) + _startVal;
    }


    public static Vector3 EaseInOutLinear(Vector3 _current, Vector3 _target, float _ease)
    {
        var dist = _target - _current;
        var vel = dist * _ease;

        return _current + vel;
    }

    /// <summary>
    /// UNTESTED
    /// </summary>
    /// <param name="_current"></param>
    /// <param name="_target"></param>
    /// <param name="_ease"></param>
    /// <returns></returns>
    public static float RotateLinear(Vector3 _current, Vector3 _target, float _ease)
    {
        // Find distance to Target
        var offset = new Vector3(_target.x - _current.x, _target.y - _current.y, _target.z - _current.z);
        var angle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;

        return Mathf.LerpAngle(0f, angle, _ease);
    }
}