using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class OpenTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource openSFX;
    private bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject enemies = GameObject.FindGameObjectWithTag("Enemy");
        if(enemies == null)
        {
           Open();
        }
        else if(!enemies.GetComponent<EnemyBehaviour>().notStunned)
        {
            Open();
        }
    }

    void Open()
    {
         if(first) 
            {
                openSFX.Play();
                first = false;
            }
            Vector3 endPoint = transform.eulerAngles;
            endPoint.y = 0;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, endPoint, 0.02f);
    }
}
