using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{

    private GameObject[] enemies;
    public GameObject message;
    [SerializeField] private Sprite sprite;

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
            message.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = sprite;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
