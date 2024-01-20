using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class Entity1 : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] waypoints;
    public GameObject lamparaPrefab;
    public GameObject cuboPrefab;
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
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        flashPanel = GameObject.Find("FlashPanel").GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
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
            TeleportarASpawnPoint();
            ElegirNuevoWaypoint();
            tiempoEsperaDespuesFlash = tiempoVisible;
            MoverEntidad();
        }

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 1f)
        {
            ReiniciarEntidad();
            ElegirNuevoWaypoint();
            tiempoEsperaDespuesFlash = tiempoVisible;
            rb.useGravity = false;
            navMeshAgent.enabled = false;
            Aparecer();
            CambiarModelo();
        }

        if (flashPanel != null)
        {
            if (Vector3.Distance(transform.position, Camera.main.transform.position) < distanciaFlash)
            {
                FlashBlanco();
                rb.useGravity = true;

                if (navMeshAgent != null)
                {
                    StartCoroutine(EsperarYActivarNavMeshAgent());
                }
                else
                {
                    Debug.LogError("navMeshAgent es nulo. Asegúrate de que esté inicializado correctamente.");
                }
            }
        }
        else
        {
            Debug.LogError("flashPanel es nulo. Asegúrate de que esté inicializado correctamente.");
        }
    }

    void ElegirNuevoSpawnPoint()
    {
        currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = currentSpawnPoint.position;
        tiempoRestante = tiempoVisible;
        Aparecer();
    }

    void TeleportarASpawnPoint()
    {
        transform.position = currentSpawnPoint.position;
    }

    void ReiniciarEntidad()
    {
        navMeshAgent.enabled = false;
        isVisible = true;
        flashActivo = false;
        rb.useGravity = false;
        ElegirNuevoSpawnPoint();
        ElegirNuevoWaypoint();
        tiempoEsperaDespuesFlash = tiempoVisible;
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

        // Desactivar el componente Rigidbody cuando aparece el modelo lamparaPrefab
        rb.isKinematic = true;
    }

    void Desaparecer()
    {
        isVisible = false;

        // Activar el componente Rigidbody cuando desaparece el modelo lamparaPrefab
        rb.isKinematic = false;
    }

    void MoverEntidad()
    {
        if (isVisible && !flashActivo && navMeshAgent.enabled && tiempoRestante > tiempoEsperaDespuesFlash)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, velocidadEntidad * Time.deltaTime);
        }
    }

    void FlashBlanco()
    {
        StartCoroutine(HacerTransparente());
        flashActivo = true;
        ReproducirSonido();
        if (cuboPrefab != null)
        {
            lamparaPrefab.SetActive(false);
            cuboPrefab.SetActive(true);
        }
        else
        {
            Debug.LogError("cuboPrefab es nulo. Asegúrate de que esté inicializado correctamente.");
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
                Debug.LogWarning("El NavMeshAgent no está en el NavMesh. Reiniciando...");
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
        navMeshAgent.enabled = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
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

        if (lamparaPrefab != null)
        {
            lamparaPrefab.SetActive(false);
        }
        else
        {
            Debug.LogError("lamparaPrefab es nulo. Asegúrate de que esté inicializado correctamente.");
        }
    }

    void CambiarModelo()
    {
        GameObject nuevoModelo = Instantiate(cuboPrefab, transform.position, Quaternion.identity);
        nuevoModelo.transform.rotation = transform.rotation;
        nuevoModelo.transform.localScale = transform.localScale;

        // Desactivar el componente Rigidbody en el objeto original (lamparaPrefab)
        rb.isKinematic = true;

        
    }
    void ReproducirSonido()
    {
        audioSource.Play();
    }
}
