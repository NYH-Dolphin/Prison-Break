using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(Input.GetKeyDown(KeyCode.K))
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies)
                {
                    Destroy(enemy);
                }
            }
    }
}
