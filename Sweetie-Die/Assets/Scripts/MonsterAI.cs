using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 15f;
    public float laughRadius = 30f;
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

    public float minWaitBetweenLaughs = 3f;
    public float maxWaitBetweenLaughs = 15f;
    public float laughWaitTimeCountdown = -1f;

    private ConversationStarter conversationStarter;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        conversationStarter = FindObjectOfType<ConversationStarter>();

        // Inicializar patrolCoroutine al inicio
        patrolCoroutine = StartCoroutine(Patrol());
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        handleLaughter();

        if (distanceToPlayer < detectionRadius && IsPlayerVisible())
    {
        if (isPatrolling)
        {
            isPatrolling = false;

            // Asegurarse de detener la patrulla antes de iniciar la persecuci�n
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
            }

            Debug.Log("Iniciando persecuci�n");
            chaseCoroutine = StartCoroutine(ChasePlayer());
        }

        // Reiniciar el tiempo de persecuci�n
        timeRemainingForChase = timeToLose;
    }
         else if (!isPatrolling && (distanceToPlayer > detectionRadius || !IsPlayerVisible()))
        {
            // Si hay tiempo restante de persecuci�n, sigue persiguiendo
            if (timeRemainingForChase > 0)
            {
                timeRemainingForChase -= Time.deltaTime;
                Debug.Log("Tiempo de persecuci�n restante: " + timeRemainingForChase);
            }
            else
            {
                // Volver a la rutina de patrullaje
                StopCoroutine(chaseCoroutine);
                isPatrolling = true;
                patrolCoroutine = StartCoroutine(Patrol());
                Debug.Log("Fin de la persecuci�n. Volviendo a patrullar.");
            }
        }
    }

    void handleLaughter()
    {
        if (conversationStarter.ConversationIsActive) return;

        if (FindObjectOfType<MonsterAudioManager>().IsPlayingLaughter()) return;
        if (laughWaitTimeCountdown < 0f)
        {
            FindObjectOfType<MonsterAudioManager>().PlayRandomLaughter();
            laughWaitTimeCountdown = UnityEngine.Random.Range(minWaitBetweenLaughs, maxWaitBetweenLaughs);
        }
        else
        {
            laughWaitTimeCountdown -= Time.deltaTime;
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
        Debug.Log("Dirigi�ndose al Waypoint: " + currentWaypointIndex);
    }
    bool IsPlayerVisible()
{
    Vector3 directionToPlayer = player.position - transform.position;
    RaycastHit hit;

    // Verificar si hay obst�culos (colisiones) entre el monstruo y el jugador
    if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
    {
        if (hit.collider.CompareTag("Player"))
        {
            // El jugador est� en la l�nea de visi�n del monstruo
            return true;
        }
    }

    // El jugador no est� en la l�nea de visi�n del monstruo
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
        Debug.Log("�Se ha activado la persecuci�n!");

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
                    // Si no hay una ruta directa, encontrar el punto m�s cercano al jugador
                    Vector3 closestPoint = GetClosestPointToPlayer();

                    // Verificar si el punto m�s cercano est� dentro del rango de detecci�n
                    if (Vector3.Distance(transform.position, closestPoint) <= detectionRadius)
                    {
                        navMeshAgent.SetDestination(closestPoint);
                    }
                    else
                    {
                        // Si el punto m�s cercano est� fuera del rango de detecci�n, volver al patrullaje
                        Debug.Log("No se encontr� una ruta v�lida. Volviendo a patrullar.");
                        break; // Salir del bucle y la funci�n ChasePlayer
                    }
                }
            }
            else
            {
                // Si no se pudo samplear la posici�n del jugador en el NavMesh, volver al patrullaje
                Debug.Log("No se pudo samplear la posici�n del jugador en el NavMesh. Volviendo a patrullar.");
                break; // Salir del bucle y la funci�n ChasePlayer
            }

            yield return null;
        }

        Debug.Log("Persiguiendo al jugador");

        // Asegurarse de detener la persecuci�n antes de volver a patrullar
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
        }

        Debug.Log("�Se ha desactivado la persecuci�n!");
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

        // Si no se encuentra un punto v�lido, devolver la posici�n actual del jugador
        return player.position;
    }
}
