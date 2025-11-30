using UnityEngine;

public class WardenPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 200f;
    public Transform cameraTransform;
    public float normalFOV = 60f;
    public float zoomedFOV = 20f;
    public float shotForce = 10f;
    public GameObject bulletPrefab;
    public Transform shotPoint;
    public Transform scopeShotPoint;
    public GameObject scopeOverlay;   
    public GameObject weaponModel;    
    private CharacterController controller;
    public bool isScoped = false;
    private float xRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform.GetComponent<Camera>().fieldOfView = normalFOV;
        if (scopeOverlay) scopeOverlay.SetActive(false);
        if (weaponModel) weaponModel.SetActive(true);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleScope();
        HandleShot();
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
            isScoped = true;
            cameraTransform.GetComponent<Camera>().fieldOfView = zoomedFOV;
            if (scopeOverlay) scopeOverlay.SetActive(true);
            if (weaponModel) weaponModel.SetActive(false);
        }
        else 
        {
            isScoped = false;
            cameraTransform.GetComponent<Camera>().fieldOfView = normalFOV;
            if (scopeOverlay) scopeOverlay.SetActive(false);
            if (weaponModel) weaponModel.SetActive(true);
        }
    }

    void HandleShot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform currentShotPoint;
            if (isScoped)
            {
                currentShotPoint = scopeShotPoint;
            }
            else
            {
                currentShotPoint = shotPoint;
            }

            if (bulletPrefab != null && currentShotPoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, currentShotPoint.position, currentShotPoint.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                if (rb != null)
                {

                    rb.AddForce(currentShotPoint.forward * shotForce, ForceMode.VelocityChange);
                }
            }
        }
    }
}
