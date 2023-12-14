using System;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Enemy;

public class ViewCone : MonoBehaviour
{
    public static ViewCone Instance;
    public HashSet<Collider> _objectsInTrigger = new();


    public void DeRegister(Collider obj)
    {
        if (_objectsInTrigger.Contains(obj))
        {
            Transform marker = obj.gameObject.transform.GetChild(2);
            marker.localScale *= 0.5f;
            marker.GetComponent<SpriteRenderer>().color = new Color(.2f, .2f, .2f);
            _objectsInTrigger.Remove(obj);
        }
    }

    public void Register(Collider obj)
    {
        Transform marker = obj.gameObject.transform.GetChild(2);
        marker.localScale *= 2f;
        marker.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        _objectsInTrigger.Add(obj);
    }


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_objectsInTrigger != null && _objectsInTrigger.Count > 0)
            GetComponentInParent<PlayerWeapon>().EnemyDetected = GetClosestObjectInTrigger();
    }

    public void DirectionCheck()
    {
        float hor = GetComponentInParent<PlayerWeapon>().animator.GetFloat("Horizontal");
        float vert = GetComponentInParent<PlayerWeapon>().animator.GetFloat("Vertical");
        if (hor > 0)
        {
            if (vert > 0)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (vert < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
                transform.eulerAngles = new Vector3(0, 135, 0);
        }
        else if (hor < 0)
        {
            if (vert > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (vert < 0)
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else
                transform.eulerAngles = new Vector3(0, -45, 0);
        }
        else
        {
            if (vert > 0)
            {
                transform.eulerAngles = new Vector3(0, 45, 0);
            }
            else if (vert < 0)
            {
                transform.eulerAngles = new Vector3(0, 225, 0);
            }
        }
    }

    GameObject GetClosestObjectInTrigger()
    {
        GameObject closestObject = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider objectInTrigger in _objectsInTrigger)
        {
            if (objectInTrigger == null) _objectsInTrigger.Remove(objectInTrigger);
            float sqrDistanceToObject = (objectInTrigger.transform.position - currentPosition).sqrMagnitude;
            if (sqrDistanceToObject < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDistanceToObject;
                closestObject = objectInTrigger.gameObject;
            }
        }

        return closestObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<EnemyBehaviour>().notStunned)
            Register(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DeRegister(other);
            // _objectsInTrigger.Remove(other);
            // if (other.gameObject == GetComponentInParent<PlayerWeapon>().EnemyDetected)
            // {
            //     GetComponentInParent<PlayerWeapon>().EnemyDetected.transform.GetChild(2).GetComponent<SpriteRenderer>()
            //         .color = new Color(50, 50, 50);
            //     GetComponentInParent<PlayerWeapon>().EnemyDetected.transform.GetChild(2).transform.localScale =
            //         new Vector3(1.5f, 1.5f, 1.5f);
            //     GetComponentInParent<PlayerWeapon>().EnemyDetected = null;
            // }
        }
    }
}