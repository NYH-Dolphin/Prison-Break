using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivate : MonoBehaviour
{
    public static DoorActivate Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void MakeBreakable()
    {
        gameObject.layer = LayerMask.NameToLayer("Breakable");
    }

}
