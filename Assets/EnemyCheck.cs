using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{

    private GameObject[] enemies;

    // Update is called once per frame
    void Update()
    {
        CheckEnemies();
    }

    void CheckEnemies()
    {

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length == 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
