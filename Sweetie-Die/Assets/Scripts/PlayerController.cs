using UnityEngine;
using UnityEngine.Video;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float gravity = 9.8f;
    private CharacterController characterController;
    private Camera playerCamera;
    private Vector3 moveDirection;
    private float verticalRotation = 0.0f;
    private bool isCrouching = false;
    public float vibrationIntensity = 0.1f;
    private bool puedeMoverse = true;
    public float teleportDistance = 5.0f;
    public Transform waypoint; // Asigna el Waypoint en el Inspector.
    public LayerMask EnemyLayer;
    public int vida = 4;
    private GameOverScript gameOver;
    public Transform Waypoint_CommonArea1;
    public Transform Waypoint_CommonArea2;
    public Transform Waypoint_CommonArea3;
    public Transform Waypoint_MonsterRoom;
    public VideoPlayer MonsterVideo;
    private float timer = 0;
    private bool canExecute = true;

    private bool wasMoving = false;

    void Start()
    {
        gameOver = GetComponent<GameOverScript>();
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused | ConversationStarter.ConversationIsActive)
        {
            return;
        }
        if (!canExecute)
        {
            timer += Time.deltaTime;
            if (timer >= 20)
            {
                canExecute = true;
                timer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Teleport(Waypoint_CommonArea1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Teleport(Waypoint_CommonArea2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Teleport(Waypoint_CommonArea3);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Teleport(Waypoint_MonsterRoom);
        }

        if (wasMoving && characterController.velocity.magnitude == 0)
        {
            FindObjectOfType<AudioManager>().Pause("StepsPlayer");
            wasMoving = false;
        }
        if (characterController.velocity.magnitude > 0)
        {
            if (!FindObjectOfType<AudioManager>().IsPlaying("StepsPlayer"))
            {
                FindObjectOfType<AudioManager>().Play("StepsPlayer");
            }
            wasMoving = true;
        }

        // Movimiento
        if (puedeMoverse)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed;
            if (characterController.isGrounded)
            {
                moveDirection = transform.TransformDirection(new Vector3(moveX, 0, moveZ));
            }
            else
            {
                moveDirection = new Vector3(moveX, moveDirection.y, moveZ);
            }

            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);

            // Rotacion
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX, 0);
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ToggleCrouch();
            }

            // Detecta enemigos en un radio alrededor del jugador
            Collider[] enemies = Physics.OverlapSphere(transform.position, teleportDistance, EnemyLayer);
            if (enemies.Length > 0 && canExecute)
            {
                foreach (Collider enemyCollider in enemies)
                {
                    if (enemyCollider.CompareTag("Enemy"))
                    {
                        Debug.Log(vida);
                        if (vida < 1)
                        {
                            MonsterVideo.Play();
                            gameOver.GameOver();
                        }
                        else
                        {
                            vida--;
                            if (MonsterVideo.isPlaying) return;
                            if (MonsterVideo != null)
                            {
                                Debug.Log("MonsterVideo ejecutado");
                                MonsterVideo.Play();
                                Debug.Log(vida);
                            }
                            Invoke("DelayedTeleport", 1.6f);
                            break;
                        }
                    }
                }
                canExecute = false;
            }
        }
    }

    private void TeleportToWaypoint()
    {
        if (waypoint != null)
        {
            // Teleporta al jugador al waypoint
            characterController.enabled = false; // Desactiva temporalmente el CharacterController para evitar problemas de colisión
            transform.position = waypoint.position;
            characterController.enabled = true; // Vuelve a activar el CharacterController
        }
    }

    public void Teleport(Transform teleportPosition)
    {
        if (teleportPosition != null)
        {
            // Teleporta al jugador al waypoint
            characterController.enabled = false; // Desactiva temporalmente el CharacterController para evitar problemas de colisión
            transform.position = teleportPosition.position;
            characterController.enabled = true; // Vuelve a activar el CharacterController
        }
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        // Ajusta la altura del CharacterController y la velocidad de movimiento según sea necesario.
        if (isCrouching)
        {
            characterController.height = 1.0f; // Establece la altura del jugador cuando está agachado.
            moveSpeed = 2.0f; // Reduzca la velocidad de movimiento cuando está agachado (ajuste según sus necesidades).
        }
        else
        {
            characterController.height = 2.0f; // Restablece la altura normal del jugador.
            moveSpeed = 5.0f; // Restablece la velocidad de movimiento normal (ajuste según sus necesidades).
        }
    }

    public void ToggleControls()
    {
        puedeMoverse = !puedeMoverse;
    }

    public void ReceiveAttack()
    {
        TeleportToWaypoint();
    }
    private void DelayedTeleport()
    {
        MonsterVideo.Stop();
        TeleportToWaypoint();
    }

}
