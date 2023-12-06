using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public int charges = 0;
    public GameObject ExplosionEffect;
    public GameObject Roadblock;
    public GameObject flamingMetal;
    // Update is called once per frame
    void Update()
    {
        if(charges == 2 && NoEnemies())
        {
            Explode();
        }

    }

    public void Explode()
    {
        for(int i = 0; i < 4; i++)
        {
            Instantiate(ExplosionEffect, this.gameObject.transform.GetChild(i).transform.position, Quaternion.identity);
            Instantiate(flamingMetal, this.gameObject.transform.GetChild(i).transform.position, Quaternion.identity);
        }
            
        Roadblock.SetActive(false);
        Destroy(gameObject);
        
    }

    bool NoEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length == 0)
            return true;
        else
            return false;
    }


}
