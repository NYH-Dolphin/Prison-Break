using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        TutorialAEvents.current.Panning();
        Destroy(gameObject);
    }
}
