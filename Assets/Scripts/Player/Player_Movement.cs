using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : Singleton<Player_Movement>
{
    private Camera mainCamera;
    private CharacterController playerController;

    private float defaultMinTurningSpeed;
    private float turnSmoothVelocity;
    private float currentSpeed;
    private float minSpeed;
    private float timeForMaxAccel;
    private float timeForRoll;

    [SerializeField] private float aimTurnSpeed = 0.2f;
    [SerializeField] private float defaultTurningSpeed = 0.2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rollMultiplier = 6f;
    [SerializeField] private float RollDuration = 5f;

    [SerializeField] private GameObject aimPivot;
    [SerializeField] private GameObject aimAt;
    [SerializeField] private GameObject aimPos;

    public float moveSpeed;
    public float maxSpeed;

    public float runSpeed;
    public float walkSpeed;

    public float timeToReachMaxAccel;
    private Vector3 gravityForce;

    public Vector2 captureDirection;
    public Vector2 movement;

    public bool isRunning = false;
    public bool isRolling = false;
    public bool isMoving = false;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        walkSpeed = maxSpeed;
        runSpeed = maxSpeed * 1.5f;

        defaultMinTurningSpeed = defaultTurningSpeed;

        minSpeed = currentSpeed;
        timeForMaxAccel = 0;
    }

    void Update()
    {
        isGroundedCheck();
        
        if(isRolling)
        {
            RollAction(captureDirection);
            return;
        }
        if (Player_Controller.instance.aiming)
        {
            float targetAngle = mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, aimTurnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            aimPivot.transform.rotation = mainCamera.transform.rotation;

            aimAt.transform.position = aimPos.transform.position;
        }
        if (!Player_Controller.instance.canRecieveInput)
        {
            return;
        }
        Move(movement);
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    private bool isGroundedCheck()
    {
        RaycastHit hit;
        float distance = (playerController.height / 2) + 0.2f;
        Vector3 dir = -transform.up;

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Gravity()
    {
        if(!isGroundedCheck())
        {
            gravityForce.y += -gravity * Time.deltaTime;
            playerController.Move(gravityForce * Time.deltaTime);
        }
        else
        {
            gravityForce.y = -2f;
        }
    }

    public void Roll()
    {
        isRolling = true;
    }

    public void Move(Vector2 inputMovement)
    {
        Vector3 MovePosition = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        
        if (MovePosition.magnitude >= 0.1f)
        {
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, timeForMaxAccel / timeToReachMaxAccel);

            if (!isRunning)
            {
                defaultMinTurningSpeed = Mathf.Clamp(((maxSpeed - currentSpeed) / maxSpeed) - 0.2f, defaultTurningSpeed, 1f);
            }
            else
            {
                defaultMinTurningSpeed = 0.05f;
            }
            
            if (!Player_Controller.instance.aiming)
            {
                float targetAngle = Mathf.Atan2(MovePosition.x, MovePosition.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, defaultMinTurningSpeed);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 MoveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                playerController.Move(MoveDirection * Time.deltaTime * currentSpeed);
                timeForMaxAccel += Time.deltaTime;
            }
            else
            {
                float targetAngle = Mathf.Atan2(MovePosition.x, MovePosition.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                Vector3 MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                playerController.Move(MoveDirection * Time.deltaTime * currentSpeed);
                timeForMaxAccel += Time.deltaTime;
            }
        }
        else
        {
            timeForMaxAccel = 0;
        }
    }
    public void RollAction(Vector2 movement)
    {
        Vector3 Movement = new Vector3(movement.x, 0, movement.y);
        float targetAngle = Mathf.Atan2(Movement.x, Movement.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        
        if(timeForRoll <= RollDuration && movement.magnitude != 0)
        {
            playerController.Move((transform.forward * rollMultiplier) * Time.deltaTime);
            timeForRoll -= Time.deltaTime;
        }
        else
        {
            timeForRoll = 0;
        }
        timeForMaxAccel = timeToReachMaxAccel;
    }

    public void On_PlayerCollision()
    {
        playerController.detectCollisions = true;
    }
    public void Off_PlayerCollision()
    {
        playerController.detectCollisions = false;
    }
}
