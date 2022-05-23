using UnityEngine;

public class Player_Movement : Singleton<Player_Movement>
{
    private Camera mainCamera;
    private CharacterController playerController;

    private float defaultMinTurningSpeed;
    private float turnSmoothVelocity;
    private float currentSpeed;
    private float minSpeed;
    private float timeForMaxAccel;

    [SerializeField] private float aimTurnSpeed = 0.2f;
    [SerializeField] private float defaultTurningSpeed = 0.2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rollMultiplier = 6f;

    [SerializeField] private GameObject aimPivot;
    [SerializeField] private GameObject aimAt;
    [SerializeField] private GameObject aimPos;

    [SerializeField] LayerMask layerToFollow;


    public float moveSpeed;
    public float maxSpeed;

    public float runSpeed;
    public float walkSpeed;

    public float timeToReachMaxAccel;
    private Vector3 gravityForce;

    public Vector2 captureDirection;
    public Vector2 movement;

    public bool isRunning = false;
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

        RollAction(captureDirection);

        if (Player_Controller.instance.Aiming)
        {
            float targetAngle = mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, aimTurnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            aimPivot.transform.rotation = mainCamera.transform.rotation;

            aimAt.transform.position = aimPos.transform.position;
        }
        if (!Player_Controller.instance.CanRecieveInput || Player_Controller.instance.IsDoingSpecialAttack)
        {
            return;
        }
        if(!Player_Controller.instance.CanWalk)
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

    public void Move(Vector2 inputMovement)
    {
        Vector3 MovePosition = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        
        if (MovePosition.magnitude >= 0.1f)
        {
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, timeForMaxAccel / timeToReachMaxAccel);

            Vector3 MoveDirection = Vector3.zero;
            float targetAngle = 0;
            if (!isRunning)
            {
                defaultMinTurningSpeed = Mathf.Clamp(((maxSpeed - currentSpeed) / maxSpeed) - 0.2f, defaultTurningSpeed, 1f);
            }
            else
            {
                defaultMinTurningSpeed = 0.05f;
            }
            
            if (!Player_Controller.instance.Aiming)
            {
                targetAngle = Mathf.Atan2(MovePosition.x, MovePosition.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, defaultMinTurningSpeed);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                MoveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                
            }
            else
            {
                targetAngle = Mathf.Atan2(MovePosition.x, MovePosition.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            RaycastHit hit;

            Vector3 checkWall = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Physics.Raycast(transform.position, checkWall, out hit, 1, layerToFollow))
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                Debug.Log(angle);
                if (angle > 50)
                {
                    return;
                }
            }

            playerController.Move(MoveDirection * Time.deltaTime * currentSpeed);
            timeForMaxAccel += Time.deltaTime;
        }
        else
        {
            timeForMaxAccel = 0;
        }
    }
    public void RollAction(Vector2 movement)
    {
        if(!Player_Controller.instance.IsRolling)
        {
            return;
        }
        Vector3 Movement = new Vector3(movement.x, 0, movement.y);
        float targetAngle = Mathf.Atan2(Movement.x, Movement.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        
        if(movement.magnitude != 0)
        {
            playerController.Move((transform.forward * rollMultiplier) * Time.deltaTime);
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
