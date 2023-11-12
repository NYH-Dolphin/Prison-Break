using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private NavMeshAgent agent;
    public float startWaitTime = 4f;
    public float startTimeToRotate = 2f;
    public float speedWalk = 8f;
    public float speedRun = 16f;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int currentWaypointIndex;
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 playerPosition;

    float waitTime;
    float timeToRotate;
    bool playerInRange;
    bool playerNear;
    bool isPatrol;
    bool caughtPlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = Vector3.zero;
        isPatrol = true;
        caughtPlayer = false;
        playerInRange = false;
        waitTime = startWaitTime;
        timeToRotate = startTimeToRotate;

        currentWaypointIndex = 0;
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.speed = speedWalk;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnvironmentView();

        if(!isPatrol)
            Chasing();
        else
            Patrolling();



    }

    private void Chasing()
    {
        playerNear = false;
        playerLastPosition = Vector3.zero;

        if(!caughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(playerPosition);
        }
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            if(waitTime <= 0 && !caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                isPatrol = true;
                playerNear = false;
                Move(speedWalk);
                timeToRotate = startTimeToRotate;
                waitTime = startWaitTime;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                }
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void Patrolling()
    {
        if(playerNear)
        {
            if(timeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                timeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            playerNear = false;
            playerLastPosition = Vector3.zero;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(waitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    waitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }



    void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }
    void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }
    public void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        caughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        agent.SetDestination(player);
        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(waitTime <= 0)
            {
                playerNear = false;
                Move(speedWalk);
                agent.SetDestination(waypoints[currentWaypointIndex].position);
                waitTime = startWaitTime;
                timeToRotate = startTimeToRotate;
            }
            Stop();
            waitTime -= Time.deltaTime;
        }
    }

    void EnvironmentView()
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for(int i = 0; i < playersInRange.Length; i++)
        {
            Transform player = playersInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToPlayer)< viewAngle/2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerInRange = true;
                    isPatrol = false;
                }
                else
                {
                    playerInRange = false;
                }

            }
            if(Vector3.Distance(transform.position, player.position)> viewRadius)
            {
                playerInRange = false;
            }
            if(playerInRange)
            {
                playerPosition = player.transform.position;
            }
        }

    }


}
