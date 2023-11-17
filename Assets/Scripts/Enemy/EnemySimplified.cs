using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySimplified : MonoBehaviour
{
    private NavMeshAgent agent;
    public float runSpeed = 20f;
    public float acceleration = 40f;

    public float viewRadius = 15;

    private GameObject player;

    bool isPatrol = true;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        isPatrol = true;
        agent.speed = 0;
        agent.acceleration = acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, player.transform.position));

        if (Vector3.Distance(transform.position, player.transform.position) <= viewRadius);
        {
            isPatrol = false;
        }

        if(!isPatrol)
        {
            agent.speed = runSpeed;
            agent.SetDestination(player.transform.position);
        }



    }


}
