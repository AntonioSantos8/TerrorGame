using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cameraHolder;
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float jumpHeight = 3f;
    public static PlayerMove instance;
    Vector3 velocity;
    float gravity = -9.81f;
    float xRotation = 0f;

    bool isGrounded;
    bool isRunning;
    bool isCursorLocked = true;

    public float mouseSensitivity = 100f;
    public float lookXLimit = 45f;

    [Header("BlobCamera")]
    [SerializeField] private float walkBobSpeed = 8f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [Space]
    [SerializeField] private float runBobSpeed = 14f;
    [SerializeField] private float runBobAmount = 0.1f;
    [Space]
    private float bobTimer = 0f;
    private Vector3 initialCamPosition;

    public bool canMove;
    void Start()
    {
        instance = this;
        canMove = true;    
        initialCamPosition = cameraHolder.localPosition;
        LockCursor();
    }

    void Update()
    {
        if(canMove)
        {
            HandleMovement();
            HandleMouseLook();
        }
        

        HandleCursorLock();
        HandleHeadBob();
    }
    void HandleHeadBob()
    {
        if (!controller.isGrounded) return;

        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (isMoving)
        {
            float speed = isRunning ? runBobSpeed : walkBobSpeed;
            float amount = isRunning ? runBobAmount : walkBobAmount;

            bobTimer += Time.deltaTime * speed;

            float yOffset = Mathf.Sin(bobTimer) * amount;
            cameraHolder.localPosition = new Vector3(initialCamPosition.x, initialCamPosition.y + yOffset, initialCamPosition.z);
        }
        else
        {
         
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, initialCamPosition, Time.deltaTime * 5f);
            bobTimer = 0f;
        }
    }
    void HandleMovement()
    {

        if (!isCursorLocked) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = moveX != 0 || moveZ != 0;


        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && isMoving;
        isRunning = wantsToRun;


        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        if (!isCursorLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorLocked)
                UnlockCursor();
            else
                LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }
}
