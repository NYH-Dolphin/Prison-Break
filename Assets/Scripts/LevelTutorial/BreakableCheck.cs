using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableCheck : MonoBehaviour
{

    private GameObject[] breakable;
    [SerializeField] private Sprite sprite;
    public GameObject message;

    // Update is called once per frame
    void Update()
    {
        CheckBreakable();
    }

    void CheckBreakable()
    {
        
        breakable = GameObject.FindGameObjectsWithTag("Breakable");
        if(breakable.Length == 0)
        {
            message.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = sprite;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
