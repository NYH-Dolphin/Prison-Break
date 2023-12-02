using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "hitbox")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
