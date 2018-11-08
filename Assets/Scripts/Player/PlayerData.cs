using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static GameObject PlayerObject { get; private set; }
    public static ushort Score { get; set; }


    private void Start()
    {
        PlayerObject = gameObject;
    }
}
