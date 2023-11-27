using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAEvents : MonoBehaviour
{
    public static TutorialAEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action cameraPan;
    public void Panning()
    {
        if(cameraPan != null)
            cameraPan();
    }

}
