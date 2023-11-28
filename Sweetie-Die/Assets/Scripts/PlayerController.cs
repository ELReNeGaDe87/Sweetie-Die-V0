using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float gravity =9.8f;
    private CharacterController characterController;
    private Camera playerCamera;
    private Vector3 moveDirection;
    private float verticalRotation = 0.0f;
    private float horizontalRotation = 0.0f;
    private bool isCrouching = false;
    public float vibrationIntensity = 0.1f;
    private bool puedeMoverse = true;
    private bool estaEnLaPuerta = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
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
        }
        else
        {
            if (estaEnLaPuerta == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                horizontalRotation += mouseX;
                horizontalRotation = Mathf.Clamp(horizontalRotation, -90f, 90f);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, horizontalRotation, transform.rotation.eulerAngles.z);
            }
        }
    }

    float SetGravity(){
        return moveDirection.y=-gravity* Time.deltaTime;
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        // Ajusta la altura del CharacterController y la velocidad de movimiento según sea necesario.
        if (isCrouching)
        {
            characterController.height = 1.0f; // Establece la altura del jugador cuando está agachado.
            moveSpeed = 2.0f; // Reduz la velocidad de movimiento cuando está agachado (ajusta según tus necesidades).
        }
        else
        {
            characterController.height = 2.0f; // Restablece la altura normal del jugador.
            moveSpeed = 5.0f; // Restablece la velocidad de movimiento normal (ajusta según tus necesidades).
        }
    }

    public void ToggleControls()
    {
        puedeMoverse = !puedeMoverse;
    }

    public void ToggleCamera()
    {
        estaEnLaPuerta = !estaEnLaPuerta;
    }
}