using UnityEngine;

public class CambiarMateriales : MonoBehaviour
{
    public Material material1;
    public Material material2;

    private Renderer rend;
    private bool estaEnMaterial1 = true;
    private float tiempoMinimoEntreCambios = 2f;
    private float tiempoMaximoEntreCambios = 5f;
    private float tiempoParaSiguienteCambio;

    void Start()
    {
        // Obtén el componente Renderer del objeto
        rend = GetComponent<Renderer>();

        // Asegúrate de que se han asignado materiales en el Inspector
        if (material1 == null || material2 == null)
        {
            Debug.LogError("Por favor, asigna materiales en el Inspector.");
        }
        else
        {
            // Establece el material inicial
            rend.material = material1;
            // Inicializa el tiempo para el primer cambio
            tiempoParaSiguienteCambio = Time.time + ObtenerTiempoAleatorio();
        }
    }

    void Update()
    {
        // Verifica si es tiempo de realizar el cambio de material
        if (Time.time >= tiempoParaSiguienteCambio)
        {
            // Realiza el cambio de material
            CambiarMaterial();
            // Actualiza el tiempo para el próximo cambio
            tiempoParaSiguienteCambio = Time.time + ObtenerTiempoAleatorio();
        }
    }

    void CambiarMaterial()
    {
        // Cambia entre los materiales
        if (estaEnMaterial1)
        {
            rend.material = material2;
        }
        else
        {
            rend.material = material1;
        }

        // Cambia el estado para el próximo cambio
        estaEnMaterial1 = !estaEnMaterial1;
    }

    float ObtenerTiempoAleatorio()
    {
        // Devuelve un tiempo aleatorio entre el mínimo y máximo especificados
        return Random.Range(tiempoMinimoEntreCambios, tiempoMaximoEntreCambios);
    }
}
