using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealMessage : MonoBehaviour
{
    public GameObject message;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
            message.SetActive(true);
    }
}
