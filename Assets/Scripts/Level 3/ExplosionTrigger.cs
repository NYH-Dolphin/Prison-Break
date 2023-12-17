using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public int charges = 0;
    public GameObject ExplosionEffect;
    public GameObject Roadblock;
    public GameObject flamingMetal;
    public GameObject player;
    public GameObject message;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(charges);
        if(charges >= 2 && Vector3.Distance(player.transform.position, transform.position) > 30f)
        {
            AudioControl.Instance.PlayExplode();
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
        message.SetActive(true);
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
