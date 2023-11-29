using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportDestination;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("El script PlayerTeleport requiere un CharacterController adjunto al GameObject del jugador.");
        }
    }

    // Llamado cuando se activa el teletransporte (puede ser en respuesta a un evento, etc.).
    public void TeleportPlayer()
    {
        if (teleportDestination != null)
        {
            // Desactivar el control temporalmente
            characterController.enabled = false;

            // Teletransportar al jugador
            transform.position = teleportDestination.position;
            Debug.Log("Jugador teletransportado al destino.");

            // Volver a activar el control
            characterController.enabled = true;
        }
        else
        {
            Debug.LogError("No se ha asignado un destino de teletransporte en el Inspector.");
        }
    }
}
