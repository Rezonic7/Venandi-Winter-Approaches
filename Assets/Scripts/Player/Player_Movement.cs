using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : Singleton<Player_Movement>
{
    private Camera camera;
    private CharacterController playerController;

    private float defaultMinTurningSpeed;
    private float turnSmoothVelocity;
    private float currentSpeed;
    private float minSpeed;
    private float timeForMaxAccel;
    private float timeForRoll;

    public float aimTurnSpeed = 0.2f;
    public float defaultTurningSpeed = 0.2f;
    public float moveSpeed;
    public float maxSpeed;
    public float timeToReachMaxAccel;
    public float gravity = 9.81f;
    public float rollMultiplier = 6f;
    public float RollDuration = 5f;

    public GameObject aimPivot;
    public GameObject aimAt;
    public GameObject aimPos;
    public Vector3 gravityForce;

    public Vector2 captureDirection;
    public Vector2 movement;

    public bool isRunning = false;
    public bool isRolling = false;
    public bool isMoving = false;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        camera = Camera.main;

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
            float targetAngle = camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, defaultTurningSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            aimPivot.transform.rotation = camera.transform.rotation;

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
                float targetAngle = Mathf.Atan2(MovePosition.x, MovePosition.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, defaultMinTurningSpeed);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 MoveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                playerController.Move(MoveDirection * Time.deltaTime * currentSpeed);
                timeForMaxAccel += Time.deltaTime;
            }
            else
            {
                float targetAngle = Mathf.Atan2(MovePosition.x, MovePosition.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
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
        float targetAngle = Mathf.Atan2(Movement.x, Movement.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
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
