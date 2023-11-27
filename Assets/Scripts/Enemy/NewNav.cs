using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewNav : MonoBehaviour
{
    [SerializeField] private float surprisedTime;
    [SerializeField] private SpriteRenderer exclaim;
    [SerializeField] float acceleration = 40f;
    private NavMeshAgent agent;
    public float speedRun = 16f;
    public bool unconscious;
    

    public float viewRadius = 15;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    private Transform player;
    Vector3 playerPosition;
    public bool stunned = false;
    private bool first;
    bool playerInRange;
    public bool isSurprised;
    public bool chasing = false;
    private EnemyBehaviour enBe;


    private AudioControl SFX;

    void Start()
    {
        playerPosition = Vector3.zero;
        playerInRange = false;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.speed = speedRun;
        agent.acceleration = acceleration;
        first = true;
        enBe = this.GetComponent<EnemyBehaviour>();
        SFX = GameObject.Find("AudioController").GetComponent<AudioControl>();
        player = GameObject.Find("[Player]").transform;
    }




    void Update()
    {
        EnvironmentView();

        if(stunned || unconscious)
        {
            chasing = false;
            agent.speed = 0;
            agent.isStopped = true;
            agent.SetDestination(this.transform.position);
        }
        else if(playerInRange && !isSurprised)
            Chasing();
    }

    public void Stunned(float time)
    {
        StartCoroutine(StunTime(time));
        exclaim.enabled = false;
        isSurprised = false;
        playerInRange = true;

    }

    void EnvironmentView()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, viewRadius); //NEEDS TO BE CHANGED
        
        for(int i = 0; i < inRange.Length; i++)
        {
            if(inRange[i].gameObject.name.Equals("[Player]"))
            {
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    if(first && !unconscious)
                    {
                        StartCoroutine(Surprised());
                        first = false;
                    }
                }
            }
            
        }
        if(player != null){
            if(Vector3.Distance(player.transform.position, this.transform.position) > viewRadius * 4f)
            playerInRange = false;
        }
        

    }

    private void Chasing()
    {
        chasing = true;
        agent.isStopped = false;
        agent.speed = speedRun;
        if(player != null) agent.SetDestination(player.transform.position);
    }


    private IEnumerator Surprised()
    {
        SFX.PlaySurprised();
        exclaim.enabled = true;
        isSurprised = true;
        yield return new WaitForSeconds(surprisedTime);
        exclaim.enabled = false;
        isSurprised = false;
        playerInRange = true;
    }

    private IEnumerator StunTime(float time)
    {
        stunned = true;
        if(unconscious)
            enBe.OnHit(2,false);
        yield return new WaitForSeconds(time);
        stunned= false;
        enBe.notStunned = true;
    }
}
