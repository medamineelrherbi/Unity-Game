using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Look Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Drag and Drop")]
    public float pickUpDistance = 5f;
    public float throwForce = 10f;
    public Transform holdPosition; // Assign an empty GameObject in front of the camera
    public LayerMask draggableLayer; // Set a layer for draggable objects in Inspector

    [Header("State")]
    public bool canMove = true; // Added missing variable

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private bool isHolding = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Auto-create holdPosition if not assigned
        if (holdPosition == null)
        {
            GameObject holdPos = new GameObject("HoldPosition");
            holdPos.transform.SetParent(playerCamera.transform);
            holdPos.transform.localPosition = new Vector3(0, 0, 1.5f);
            holdPosition = holdPos.transform;
        }
    }

    void Update()
    {
        HandleMovement();

        if (canMove)
        {
            HandleMouseLook();
        }

        HandleDragAndDrop();
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void HandleDragAndDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHolding)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject(false);
            }
        }

        if (Input.GetMouseButtonDown(1) && isHolding)
        {
            DropObject(true);
        }

        if (isHolding && heldObject != null)
        {
            heldObject.transform.position = holdPosition.position;
            heldObject.transform.rotation = holdPosition.rotation;
        }
    }

    private void TryPickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickUpDistance, draggableLayer))
        {
            heldObject = hit.collider.gameObject;
            heldObjectRb = heldObject.GetComponent<Rigidbody>();

            if (heldObjectRb != null)
            {
                heldObjectRb.isKinematic = true;
                heldObjectRb.detectCollisions = true;
            }

            heldObject.transform.SetParent(holdPosition);
            isHolding = true;
        }
    }

    private void DropObject(bool shouldThrow)
    {
        if (heldObject == null) return;

        heldObject.transform.SetParent(null);

        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = false;

            if (shouldThrow)
            {
                heldObjectRb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
            }
        }

        isHolding = false;
        heldObject = null;
        heldObjectRb = null;
    }
}