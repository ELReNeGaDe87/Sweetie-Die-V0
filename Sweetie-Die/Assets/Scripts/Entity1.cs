using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;  // Agrega el namespace necesario
using System.Collections;

public class Entity1 : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] waypoints;
    public float tiempoVisible = 120f;
    public float distanciaFlash = 5f;
    public float alturaSuelo = 1f;
    public float velocidadEntidad = 5f;
    public float duracionFlash = 0.1f;
    public float retrasoGravedad = 0.5f;

    private Transform currentSpawnPoint;
    private Transform currentWaypoint;
    private bool isVisible = false;
    private bool flashActivo = false;
    private float tiempoRestante;
    private float tiempoEsperaDespuesFlash;
    private Image flashPanel;
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;  // Agrega la referencia al NavMeshAgent

    void Start()
    {
        flashPanel = GameObject.Find("FlashPanel").GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();  // Obtén la referencia al NavMeshAgent
        ElegirNuevoSpawnPoint();
        ElegirNuevoWaypoint();
        tiempoEsperaDespuesFlash = tiempoVisible;
    }

    void Update()
    {
        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f && !flashActivo)
        {
            Desaparecer();
            ElegirNuevoSpawnPoint();
            ElegirNuevoWaypoint();
            tiempoEsperaDespuesFlash = tiempoVisible;
            MoverEntidad();
        }

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 1f)
        {
            ElegirNuevoSpawnPoint();
            ElegirNuevoWaypoint();
            tiempoEsperaDespuesFlash = tiempoVisible;
            rb.useGravity = false;
            navMeshAgent.enabled = false;
            Aparecer();
        }

        if (Vector3.Distance(transform.position, Camera.main.transform.position) < distanciaFlash)
        {
            FlashBlanco();
            rb.useGravity = true;

            if (navMeshAgent != null)
            {
                StartCoroutine(EsperarYActivarNavMeshAgent()); // Establece el destino del NavMeshAgent
            }
            else
            {
                Debug.LogError("navMeshAgent es nulo. Asegúrate de que esté inicializado correctamente.");
            }
        }

    }

    void ElegirNuevoSpawnPoint()
    {
        currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = currentSpawnPoint.position;
        tiempoRestante = tiempoVisible;
        Aparecer();
    }

    void ElegirNuevoWaypoint()
    {
        currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
    }

    void Aparecer()
    {
        isVisible = true;
        flashActivo = false;
    }

    void Desaparecer()
    {
        isVisible = false;
    }

    void MoverEntidad()
    {
        if (isVisible && !flashActivo && tiempoRestante > tiempoEsperaDespuesFlash)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, velocidadEntidad * Time.deltaTime);
        }
    }

    void FlashBlanco()
    {
        StartCoroutine(HacerTransparente());
        flashActivo = true;
    }
    IEnumerator EsperarYActivarNavMeshAgent()
    {
        yield return new WaitForSeconds(0.8f);

        if (navMeshAgent != null)
        {
            // Activa el NavMeshAgent y establece su destino si está en el NavMesh
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentWaypoint.position);
            }
            else
            {
                Debug.LogError("El NavMeshAgent no está en el NavMesh.");
            }
        }
        else
        {
            Debug.LogError("El NavMeshAgent es nulo.");
        }
    }
    IEnumerator HacerTransparente()
    {
        float elapsedTime = 0f;
        Color colorInicial = Color.white;
        Color colorFinal = Color.clear;

        while (elapsedTime < duracionFlash)
        {
            flashPanel.color = Color.Lerp(colorInicial, colorFinal, elapsedTime / duracionFlash);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        flashPanel.color = colorFinal;
    }
}

