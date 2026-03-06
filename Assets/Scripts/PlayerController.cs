using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public static bool isIntroPlaying = true;

    [Header("Cinématique d'intro")]
    public GameObject introCamera;
    public GameObject cinematicManager;

    [Header("Paramètres de déplacement")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.2f;

    [Header("Références")]
    public Transform playerCamera;

    [Header("Inputs (Configurer dans l'Inspector)")]
    public InputAction moveAction;
    public InputAction lookAction;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("SavedSensitivity", 0.2f);
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction.Enable();
        lookAction.Enable();

        isIntroPlaying = true;
        Invoke("EndIntro", 5f);
    }

    void EndIntro()
    {
        if (introCamera != null) Destroy(introCamera);

        if (cinematicManager != null) Destroy(cinematicManager);

        isIntroPlaying = false;
    }

    void Update()
    {

        if (AlbumManager.isOpen || QuestManager.isEndScreenOpen || PauseMenu.isPaused || isIntroPlaying) return;
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
        transform.Rotate(Vector3.up * mouseX); 

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.SimpleMove(move * moveSpeed); 
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }
}