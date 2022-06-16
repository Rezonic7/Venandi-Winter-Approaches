using UnityEngine;

public class Player_Movement : Singleton<Player_Movement>
{
    [SerializeField] private float aimTurnSpeed = 0.2f;
    [SerializeField] private float defaultTurningSpeed = 0.2f;
    [SerializeField] private float rollMultiplier = 1.1f;

    [SerializeField] LayerMask layerToFollow;

    private Camera mainCamera;
    private CharacterController _playerController;

    private float defaultMinTurningSpeed;
    private float turnSmoothVelocity;
    private float currentSpeed;
    private float timeForMaxAccel;
    private float gravity = 9.81f;
    private float timeToReachMaxAccel = 1;
    private float limitAngle = 37;
    private float negativeArray;

    private GameObject aimPivot;
    private GameObject aimAt;
    private GameObject aimPos;

    private Quaternion[] arrayofAngles;

    private int fieldOfView = 65;
    private int numberOfRays = 3;

    private float _minSpeed;
    private float _maxSpeed;
    private float _runModifier;
    private float _speedModifier;

    private Vector3 gravityForce;

    private Vector2 _captureDirection;
    private Vector2 _movement;

    private bool _isRunning = false;


    public float MinSpeed { get { return _minSpeed; } set { _minSpeed = value; } }
    public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    public float RunModifier { get { return _runModifier; } set { _runModifier = value; } }
    public float SpeedModifer { get { return _speedModifier; } set { _speedModifier = value; } }

    public Vector2 Movement { get { return _movement; } set { _movement = value; } }
    public Vector2 CaptureDirection { get { return _captureDirection; } set { _captureDirection = value; } }
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }

    public CharacterController PlayerController { get { return _playerController; } set { _playerController = value; } }
   

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        aimPivot = GameObject.FindWithTag("AimPivot")?.gameObject;
        aimAt = GameObject.FindWithTag("AimTarget")?.gameObject;
        aimPos = GameObject.FindWithTag("AimPosition")?.gameObject;

        if(!aimPivot || !aimAt || !aimPos)
        {
            Debug.Log("Heads up! Aiming will not work properly, some Aiming Components are missing.");
        }

        _minSpeed = 1f;
        _maxSpeed = 5f;
        _runModifier = 1;
        _speedModifier = 1f;
        timeToReachMaxAccel = 1;
        limitAngle = 37;

        defaultMinTurningSpeed = defaultTurningSpeed;

        timeForMaxAccel = 0;
    }

    void Update()
    {
        if(Player_Controller.instance.IsDead)
        {
            return;
        }
        isGroundedCheck();

        RollAction(_captureDirection);

        if (Player_Controller.instance.Aiming)
        {
            if(aimPivot || aimAt || aimPos)
            {
                float targetAngle = mainCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, aimTurnSpeed);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                aimPivot.transform.rotation = mainCamera.transform.rotation;

                aimAt.transform.position = aimPos.transform.position;
            }
        }
        if (!Player_Controller.instance.CanRecieveInput || Player_Controller.instance.IsDoingSpecialAttack)
        {
            return;
        }
        if(!Player_Controller.instance.CanWalk)
        {
            return;
        }
        if(Player_Controller.instance.IsRolling)
        {
            return;
        }
        Move(_movement);
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    private bool isGroundedCheck()
    {
        RaycastHit hit;
        float distance = (_playerController.height / 2) + 0.2f;
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
            _playerController.Move(gravityForce * Time.deltaTime);
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
            currentSpeed = (Mathf.SmoothStep(_minSpeed, _maxSpeed, timeForMaxAccel / timeToReachMaxAccel) * _runModifier) * _speedModifier;

            Vector3 MoveDirection = Vector3.zero;
            float targetAngle = 0;
            if (!_isRunning)
            {
                defaultMinTurningSpeed = Mathf.Clamp(((_maxSpeed - currentSpeed) / _maxSpeed) - 0.2f, defaultTurningSpeed, 1f);
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

            Vector3 LoSRef = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if(!LineOfSight(LoSRef))
            {
                return;
            }

            _playerController.Move(MoveDirection * Time.deltaTime * currentSpeed);
            timeForMaxAccel += Time.deltaTime;
        }
        else
        {
            timeForMaxAccel = 0;
        }
    }
    bool LineOfSight(Vector3 MoveDirection)
    {
        arrayofAngles = new Quaternion[numberOfRays + 1];
        negativeArray = -(fieldOfView / 2);

        for (int i = 0; i < arrayofAngles.Length; i++)
        {
            float angle = negativeArray + (i * (fieldOfView / numberOfRays));
            arrayofAngles[i] = Quaternion.AngleAxis(angle, Vector3.up);

            Vector3 rayDirection = arrayofAngles[i] * MoveDirection;

            RaycastHit hit;

            Vector3 yOffset = new Vector3(0, -0.8f, 0);
            Vector3 refPos = transform.position + yOffset;

            if (Physics.Raycast(refPos, rayDirection, out hit, 1, layerToFollow))
            {
                float normalAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (normalAngle > limitAngle)
                {
                    return false;
                }
            }
        }
        return true;
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
        if(!LineOfSight(transform.forward))
        {
            return;
        }
        if(movement.magnitude != 0)
        {
            _playerController.Move((transform.forward * (_maxSpeed * rollMultiplier)) * Time.deltaTime);
        }
        timeForMaxAccel = timeToReachMaxAccel;
    }

    public void On_PlayerCollision()
    {
        _playerController.detectCollisions = true;
    }
    public void Off_PlayerCollision()
    {
        _playerController.detectCollisions = false;
    }
}
