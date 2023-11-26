using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField] private float surprisedTime;
    [SerializeField] private SpriteRenderer exclaim;
    private NavMeshAgent agent;
    public float startWaitTime = 4f;
    public float startTimeToRotate = 2f;
    public float speedWalk = 8f;
    public float speedRun = 16f;
    public float acceleration = 40f;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    //private Transform waypoint;
    private Transform player;
    int currentWaypointIndex;
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 playerPosition;
    public bool stunned = false;
    private bool first;

    float waitTime;
    float timeToRotate;
    bool playerInRange;
    bool playerNear;
    public bool isPatrol;
    public bool isSurprised;
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
        //waypoint = this.transform;

        currentWaypointIndex = 0;
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.speed = speedWalk;
        agent.acceleration = acceleration;
        //agent.SetDestination(waypoint.position);
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        EnvironmentView();

        if(isPatrol || isSurprised)
            Patrolling();
        else if(stunned)
        {
            Stop();
        }
            
        else
            Chasing();

    }

    private void Chasing()
    {
        playerNear = false;
        playerLastPosition = Vector3.zero;


        Move(speedRun);
        agent.SetDestination(playerPosition);
        
        // if(agent.remainingDistance <= agent.stoppingDistance)
        // {
        //     if(waitTime <= 0 && !caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= viewRadius * 8)
        //     {
        //         isPatrol = true;
        //         playerNear = false;
        //         Move(speedWalk);
        //         timeToRotate = startTimeToRotate;
        //         waitTime = startWaitTime;
        //     }
        //     else
        //     {
        //         if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
        //         {
        //             Stop();
        //         }
        //         waitTime -= Time.deltaTime;
        //     }
        // }
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
            //agent.SetDestination(waypoint.position);
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(waitTime <= 0)
                {
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
                //agent.SetDestination(waypoint.position);
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
            player = playersInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToPlayer)< viewAngle/2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerInRange = true;
                    if(first)
                    {
                        StartCoroutine(Surprised());
                        first = false;
                    }
                }
                else
                {
                    playerInRange = false;
                    first = true;
                }

            }
            if(Vector3.Distance(transform.position, player.position)> viewRadius)
            {
                playerInRange = false;
            }
        }

        if(!first)
        {
            playerPosition = player.transform.position;
        }

    }

    private IEnumerator Surprised()
    {
        exclaim.enabled = true;
        isPatrol = false;
        isSurprised = true;
        yield return new WaitForSeconds(surprisedTime);
        exclaim.enabled = false;
        isSurprised = false;
    }


}
