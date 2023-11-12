using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hitPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            Debug.Log("here");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
