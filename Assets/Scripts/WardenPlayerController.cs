using UnityEngine;

public class WardenPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 200f;
    public Transform cameraTransform;
    public float normalFOV = 60f;
    public float zoomedFOV = 20f;
    public GameObject scopeOverlay;   
    public GameObject weaponModel;    
    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform.GetComponent<Camera>().fieldOfView = normalFOV;
        if (scopeOverlay) scopeOverlay.SetActive(false);
        if (weaponModel) weaponModel.SetActive(true);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleScope();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleScope()
    {
        if (Input.GetMouseButton(1))
        {
            cameraTransform.GetComponent<Camera>().fieldOfView = zoomedFOV;
            if (scopeOverlay) scopeOverlay.SetActive(true);
            if (weaponModel) weaponModel.SetActive(false);
        }
        else
        {
            cameraTransform.GetComponent<Camera>().fieldOfView = normalFOV;
            if (scopeOverlay) scopeOverlay.SetActive(false);
            if (weaponModel) weaponModel.SetActive(true);
        }
    }
}
