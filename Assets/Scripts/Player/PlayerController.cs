using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float playerSpeed;
    public float moveSpeed;
    public float runSpeed;
    private bool isRun;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity; // 카메라 민감도

    private Vector2 mouseDelta;
    
    [Header("SpeedUp")]
    private bool isSpeedUp;
    private float speedUpDefaultTimer = 5f;
    public float speedUpTimer = 0f;
    private Coroutine speedUpCoroutine;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;
    public PlayerAniController aniController;

    private void Awake()
    {
        playerSpeed = moveSpeed;
        rigidbody = GetComponent<Rigidbody>();
        aniController = GetComponent<PlayerAniController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void FixedUpdate()
    {
        Move();
        SpeedUpTimer();
    }
    
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded() )
        {
            aniController.Jump();
            //Invoke("Jump", 0.7f);
            Jump();
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        { 
            isRun = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
        }
    }

    private void Move()
    {
        if (isRun && CharacterManager.Instance.Player.condition.UseStamina(1f))
            playerSpeed = runSpeed;
        else
            playerSpeed = moveSpeed;
        
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= playerSpeed; 
        dir.y = rigidbody.velocity.y;
        
        rigidbody.velocity = dir;
        aniController.Move(dir);
    }

    public void SpeedUp()
    {
        isSpeedUp = true;
        if (speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
            moveSpeed /= 2;
            runSpeed /= 2;
        }
        speedUpCoroutine = StartCoroutine(SpeedUpSecond(speedUpDefaultTimer + speedUpTimer));
    }

    private void SpeedUpTimer()
    {
        if (isSpeedUp)
        {
            speedUpTimer += Time.deltaTime;
        }
    }

    private IEnumerator SpeedUpSecond(float seconds)
    {
        moveSpeed *= 2;
        runSpeed *= 2;
        yield return new WaitForSeconds(seconds);
        isSpeedUp = false;
        moveSpeed /= 2;
        runSpeed /= 2;
        speedUpTimer = 0f;
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}