using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class Entity1 : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] waypoints;
    public GameObject lamparaPrefab; // Prefab de la l�mpara
    public GameObject cuboPrefab;    // Prefab del cubo
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
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        flashPanel = GameObject.Find("FlashPanel").GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;  // Desactiva el NavMeshAgent inicialmente
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
            CambiarModelo(); // Cambia el modelo de la l�mpara al cubo
        }

        if (flashPanel != null)
        {
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
                    Debug.LogError("navMeshAgent es nulo. Aseg�rate de que est� inicializado correctamente.");
                }
            }
        }
        else
        {
            Debug.LogError("flashPanel es nulo. Aseg�rate de que est� inicializado correctamente.");
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
        lamparaPrefab.SetActive(true);
        cuboPrefab.SetActive(false);
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

        // Activar el objeto "monstruo" (cuboPrefab) al inicio del flash
        if (cuboPrefab != null)
        {
            lamparaPrefab.SetActive(false);
            cuboPrefab.SetActive(true);
        }
        else
        {
            Debug.LogError("cuboPrefab es nulo. Aseg�rate de que est� inicializado correctamente.");
        }
    }

    IEnumerator EsperarYActivarNavMeshAgent()
    {
        yield return new WaitForSeconds(0.8f);

        if (navMeshAgent != null)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentWaypoint.position);
            }
            else
            {
                Debug.LogWarning("El NavMeshAgent no est� en el NavMesh. Reiniciando...");
                ReiniciarNavMeshAgent();
            }
        }
        else
        {
            Debug.LogError("El NavMeshAgent es nulo.");
        }
    }

    void ReiniciarNavMeshAgent()
    {
        navMeshAgent.enabled = false; // Desactiva el NavMeshAgent
                                      // Puedes reinicializar el NavMeshAgent seg�n tus necesidades
                                      // Por ejemplo, podr�as volver a obtener el componente y establecer sus propiedades
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;  // Vuelve a activar el NavMeshAgent
        navMeshAgent.SetDestination(currentWaypoint.position);
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

        // Desactiva el objeto "cristal" (lamparaPrefab)
        if (lamparaPrefab != null)
    {
        lamparaPrefab.SetActive(false);
    }
    else
    {
        Debug.LogError("lamparaPrefab es nulo. Aseg�rate de que est� inicializado correctamente.");
    }
}

    void CambiarModelo()
    {
        // Crea un nuevo objeto del cubo en la posici�n actual de la l�mpara
        GameObject nuevoModelo = Instantiate(cuboPrefab, transform.position, Quaternion.identity);

        // Copia la rotaci�n y escala del objeto original al nuevo objeto
        nuevoModelo.transform.rotation = transform.rotation;
        nuevoModelo.transform.localScale = transform.localScale;

        // Destruye el objeto original
        Destroy(gameObject);
    }
}
