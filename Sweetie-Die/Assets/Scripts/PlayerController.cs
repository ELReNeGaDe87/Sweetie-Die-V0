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
    public Transform waypoint;
    public LayerMask EnemyLayer;
    public int vida = 4;
    private GameOverScript gameOver;
    public Transform Waypoint_CommonArea1;
    public Transform Waypoint_CommonArea2;
    public Transform Waypoint_CommonArea3;
    public Transform Waypoint_MonsterRoom;
    public VideoPlayer MonsterVideo;
    public RecogerObjeto recogerObjeto;
    private float timer = 0;
    private bool canExecute = true;
    private ConversationStarter conversationStarter;
    private ShowFantasmaComments showFantasmaComments;
    private bool ControllerIsActive = true;
    [SerializeField]
    private GameObject aimDot;

    private MonsterAudioManager monsterAudioManager;

    private bool wasMoving = false;

    public void Deactivate()
    {
        ControllerIsActive = false;
    }

    public void Activate()
    {
        ControllerIsActive = true;
    }

    void Start()
    {
        recogerObjeto = FindObjectOfType<RecogerObjeto>();
        gameOver = GetComponent<GameOverScript>();
        characterController = GetComponent<CharacterController>();
        conversationStarter = FindObjectOfType<ConversationStarter>();
        showFantasmaComments = FindObjectOfType<ShowFantasmaComments>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused | conversationStarter.ConversationIsActive | !ControllerIsActive)
        {
            if (FindObjectOfType<AudioManager>().IsPlaying("StepsPlayer"))
            {
                FindObjectOfType<AudioManager>().Pause("StepsPlayer");
            }
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

                // Realiza un raycast hacia abajo para obtener información sobre el terreno.
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 + 0.1f))
                {
                    // Calcula el ángulo entre la normal del terreno y el vector up.
                    float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);

                    // Chequea la pendiente del terreno antes de aplicar movimiento vertical.
                    if (slopeAngle <= characterController.slopeLimit)
                    {
                        moveDirection.y = 0.0f;
                    }
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);

            // Limita la altura del jugador
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.Clamp(newPosition.y, 0.5f, 1.18f);
            transform.position = newPosition;

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
                        PlayVideo();
                    }
                }

            }
        }
    }

    private void PlayVideo()
    {
        Invoke("DelayedTeleport", 0.3f);
        canExecute = false;
        UnityEngine.Debug.Log(vida);
        if (MonsterVideo.isPlaying) return;
        if (MonsterVideo != null)
        {
            UnityEngine.Debug.Log("MonsterVideo ejecutado");
            monsterAudioManager = FindObjectOfType<MonsterAudioManager>();
            monsterAudioManager.PauseFootsteps();
            aimDot.SetActive(false);
            MonsterVideo.Play();
        }
        if (vida < 1) gameOver.GameOver();
        else
        {
            vida--;
            recogerObjeto.ReturnToSender();
            Invoke("ShowFantasmaComment", 1.6f);
        }
    }

    private void TeleportToWaypoint()
    {
        if (waypoint != null)
        {
            // Teleporta al jugador al waypoint
            characterController.enabled = false;
            transform.position = waypoint.position;
            characterController.enabled = true;
        }
    }

    public void Teleport(Transform teleportPosition)
    {
        if (teleportPosition != null)
        {
            // Teleporta al jugador al waypoint
            characterController.enabled = false;
            transform.position = teleportPosition.position;
            characterController.enabled = true;
        }
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            characterController.height = 1.0f;
            moveSpeed = 2.0f;
        }
        else
        {
            characterController.height = 2.0f;
            moveSpeed = 5.0f;
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
        TeleportToWaypoint();
    }

    private void ShowFantasmaComment()
    {
        MonsterVideo.Stop();
        monsterAudioManager.PlayFootsteps();
        aimDot.SetActive(true);
        if (vida == 3) showFantasmaComments.ShowComment(1);
        else if (vida == 2) showFantasmaComments.ShowComment(2);
        else if (vida == 1) showFantasmaComments.ShowComment(4);
        else if (vida == 0) showFantasmaComments.ShowComment(5);
    }
}
