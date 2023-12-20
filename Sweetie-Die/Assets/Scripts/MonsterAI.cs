using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 15f;
    public float timeToLose = 15f;
    private Vector3 lastCalculatedPoint;
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

    if (distanceToPlayer < detectionRadius && IsPlayerVisible())
    {
        if (isPatrolling)
        {
            isPatrolling = false;

            // Asegurarse de detener la patrulla antes de iniciar la persecución
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
            }

            Debug.Log("Iniciando persecución");
            chaseCoroutine = StartCoroutine(ChasePlayer());
        }

        // Reiniciar el tiempo de persecución
        timeRemainingForChase = timeToLose;
    }
         else if (!isPatrolling && (distanceToPlayer > detectionRadius || !IsPlayerVisible()))
        {
            // Si hay tiempo restante de persecución, sigue persiguiendo
            if (timeRemainingForChase > 0)
            {
                timeRemainingForChase -= Time.deltaTime;
                Debug.Log("Tiempo de persecución restante: " + timeRemainingForChase);
            }
            else
            {
                // Volver a la rutina de patrullaje
                StopCoroutine(chaseCoroutine);
                isPatrolling = true;
                patrolCoroutine = StartCoroutine(Patrol());
                Debug.Log("Fin de la persecución. Volviendo a patrullar.");
            }
        }
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
    bool IsPlayerVisible()
{
    Vector3 directionToPlayer = player.position - transform.position;
    RaycastHit hit;

    // Verificar si hay obstáculos (colisiones) entre el monstruo y el jugador
    if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
    {
        if (hit.collider.CompareTag("Player"))
        {
            // El jugador está en la línea de visión del monstruo
            return true;
        }
    }

    // El jugador no está en la línea de visión del monstruo
    return false;
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
            NavMeshHit playerHit;
            if (NavMesh.SamplePosition(player.position, out playerHit, 20f, NavMesh.AllAreas))
            {
                Vector3 playerSampledPosition = playerHit.position;

                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(playerSampledPosition, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    navMeshAgent.SetDestination(playerSampledPosition);
                }
                else
                {
                    // Si no hay una ruta directa, encontrar el punto más cercano al jugador
                    Vector3 closestPoint = GetClosestPointToPlayer();

                    // Verificar si el punto más cercano está dentro del rango de detección
                    if (Vector3.Distance(transform.position, closestPoint) <= detectionRadius)
                    {
                        navMeshAgent.SetDestination(closestPoint);
                    }
                    else
                    {
                        // Si el punto más cercano está fuera del rango de detección, volver al patrullaje
                        Debug.Log("No se encontró una ruta válida. Volviendo a patrullar.");
                        break; // Salir del bucle y la función ChasePlayer
                    }
                }
            }
            else
            {
                // Si no se pudo samplear la posición del jugador en el NavMesh, volver al patrullaje
                Debug.Log("No se pudo samplear la posición del jugador en el NavMesh. Volviendo a patrullar.");
                break; // Salir del bucle y la función ChasePlayer
            }

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

    Vector3 GetClosestPointToPlayer()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 20f, NavMesh.AllAreas))
        {
            lastCalculatedPoint = hit.position;
            return lastCalculatedPoint;
        }

        // Si no se encuentra un punto válido, devolver la posición actual del jugador
        return player.position;
    }
}
