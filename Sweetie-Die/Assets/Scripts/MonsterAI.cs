using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 15f;
    public float timeToLose = 5f;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private bool isChasing = false;

    void Start()
    {
       
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(Patrol());
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius && !DetectObstacle())
        {
            isChasing = true;
            StopAllCoroutines();
            StartCoroutine(ChasePlayer());
        }
        else if (isChasing)
        {
            StartCoroutine(StopChasing());
        }
    }

    bool DetectObstacle()
    {
        NavMeshHit hit;
        if (NavMesh.Raycast(transform.position, player.position, out hit, NavMesh.AllAreas))
        {
            return hit.hit;
        }
        return false;
    }

    void SetRandomDestination()
    {
        if (waypoints.Length > 0)
        {
            Transform randomWaypoint = waypoints[Random.Range(0, waypoints.Length)];
            navMeshAgent.SetDestination(randomWaypoint.position);
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            SetRandomDestination();
            yield return new WaitForSeconds(navMeshAgent.remainingDistance / navMeshAgent.speed);
        }
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.SetDestination(player.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator StopChasing()
    {
        yield return new WaitForSeconds(timeToLose);
        isChasing = false;
        StartCoroutine(Patrol());
    }
}