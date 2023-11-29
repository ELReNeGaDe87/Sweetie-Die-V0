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
    private bool isPatrolling = true;
    private int currentWaypointIndex = 0;
    public float distanceToPlayer;
    private Coroutine patrolCoroutine;
    private Coroutine chaseCoroutine;
    private float timeRemainingForChase = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Inicializar patrolCoroutine al inicio
        patrolCoroutine = StartCoroutine(Patrol());
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius && !DetectObstacle())
        {
            if (isPatrolling)
            {
                isPatrolling = false;

                // Asegurarse de detener la patrulla antes de iniciar la persecución
                if (patrolCoroutine != null)
                {
                    StopCoroutine(patrolCoroutine);
                }

                chaseCoroutine = StartCoroutine(ChasePlayer());
            }

            // Reiniciar el tiempo de persecución
             timeRemainingForChase = 15f;
        }
        else if (!isPatrolling && distanceToPlayer > detectionRadius)
        {
            if (chaseCoroutine != null)
            {
                StopCoroutine(chaseCoroutine);
            }

            // Si hay tiempo restante de persecución, sigue persiguiendo
            if (timeRemainingForChase > 0)
            {
                timeRemainingForChase -= Time.deltaTime;
                Debug.Log("Tiempo de persecución: " + timeRemainingForChase);
            }
            else
            {
                isPatrolling = true;
                patrolCoroutine = StartCoroutine(Patrol());
            }
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

    void SetNextWaypoint()
    {
        int randomIndex = Random.Range(0, waypoints.Length);

        while (randomIndex == currentWaypointIndex)
        {
            randomIndex = Random.Range(0, waypoints.Length);
        }

        currentWaypointIndex = randomIndex;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        Debug.Log("Dirigiéndose al Waypoint: " + currentWaypointIndex);
    }

    IEnumerator Patrol()
    {
        while (isPatrolling)
        {
            SetNextWaypoint();
            yield return null;

            while (navMeshAgent.remainingDistance > 0.1f)
            {
                yield return null;
            }

            Debug.Log("Patrullando. Distancia restante: " + navMeshAgent.remainingDistance);
        }
    }

    IEnumerator ChasePlayer()
    {
        navMeshAgent.speed = chaseSpeed;
        Debug.Log("¡Se ha activado la persecución!");
        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            navMeshAgent.SetDestination(player.position);
            yield return null;
        }

        Debug.Log("Persiguiendo al jugador");

        // Asegurarse de detener la persecución antes de volver a patrullar
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
        }
        Debug.Log("¡Se ha desactivado la persecución!");
        isPatrolling = true;
        patrolCoroutine = StartCoroutine(Patrol());
    }
}
