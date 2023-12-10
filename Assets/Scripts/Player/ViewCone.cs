using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ViewCone : MonoBehaviour
{
    private HashSet<Collider> _objectsInTrigger = new HashSet<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_objectsInTrigger != null && _objectsInTrigger.Count > 0)
            GetComponentInParent<PlayerWeapon>()._enemyDetected = GetClosestObjectInTrigger();
    }

    public void DirectionCheck()
    {
        float hor = GetComponentInParent<PlayerWeapon>().animator.GetFloat("Horizontal");
        float vert = GetComponentInParent<PlayerWeapon>().animator.GetFloat("Vertical");
        if(hor > 0)
        {
            if(vert > 0)
            {
                transform.eulerAngles = new Vector3(0,90,0);
            }
            else if(vert < 0)
            {
                transform.eulerAngles = new Vector3(0,180,0);
            }
            else
                transform.eulerAngles = new Vector3(0,135,0);
        }
        else if(hor < 0)
        {
            if(vert > 0)
            {
                transform.eulerAngles = new Vector3(0,0,0);
            }
            else if(vert < 0)
            {
                transform.eulerAngles = new Vector3(0,-90,0);
            }
            else
                transform.eulerAngles = new Vector3(0,-45,0);
        }
        else
        {
            if(vert > 0)
            {
                transform.eulerAngles = new Vector3(0,45,0);
            }
            else if(vert < 0)
            {
                transform.eulerAngles = new Vector3(0,225,0);
            }
        }

        
    }

    GameObject GetClosestObjectInTrigger()
    {
        GameObject closestObject = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Collider objectInTrigger in _objectsInTrigger)
        {
            if(objectInTrigger == null) _objectsInTrigger.Remove(objectInTrigger);
            float sqrDistanceToObject = (objectInTrigger.transform.position - currentPosition).sqrMagnitude;
            if(sqrDistanceToObject < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDistanceToObject;
                closestObject = objectInTrigger.gameObject;
            }
        }
    
        return closestObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
            _objectsInTrigger.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy")){
            _objectsInTrigger.Remove(other);
            if(other.gameObject ==  GetComponentInParent<PlayerWeapon>()._enemyDetected)
            {
                GetComponentInParent<PlayerWeapon>()._enemyDetected.transform.GetChild(3).gameObject.SetActive(false);
                GetComponentInParent<PlayerWeapon>()._enemyDetected = null;
            }
                
        }
            
    }

        
}
