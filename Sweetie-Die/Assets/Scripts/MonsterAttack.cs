using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    public Transform player;
    public Transform teleportWaypoint;
    public float attackRange = 3f;

    // Update is called once per frame
    void Update()
    {
        // Calcula la distancia entre el NPC y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de ataque, teleporta al jugador al waypoint
        if (distanceToPlayer < attackRange)
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        // Teletransporta al jugador al waypoint
        player.position = teleportWaypoint.position;

        // Puedes agregar cualquier lógica adicional aquí, como reproducir un sonido, mostrar efectos, etc.

        Debug.Log("¡El jugador ha sido teletransportado!");
    }
}
