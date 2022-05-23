using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalClass : MonoBehaviour
{
    [SerializeField] private AnimalData _animalData;
    [SerializeField] LayerMask layerToFollow;
    private GameObject _areaHolder;
    private AreaClass[] areas;
    private AreaClass _currentArea;
    private NavMeshAgent _agent;
    private Animator _anim;
    private Player_Controller _player;

    private SphereCollider _aggroCollider;
    private SphereCollider _OutOfRangeCollider;
    private Collider hitBox;

    private string _animalName;

    private float _attackRange;
    private float turnSmoothVelocity;
    private float _baseWanderTime;
    private float _baseLoiterTime;
    private float _baseSpeed;
    private float _wanderTimer;
    private float _stuckTimer;
    private float _loiterTimer;
    private float scaleX;
    private float scaleZ;
    private float lastDistance;

    private int _maxHealth;
    private int _currentHealth;
    private int _baseDamage;
    private int _totalDamage;

    private bool _isGoingToNextArea;
    private bool _agentHasPath;
    private bool _canMove;
    private bool _isPassive;
    private bool _isAgitated;
    private bool _isPlayerInRange;
    private bool _isAttacking;
    private bool _isDead;
    private bool _hasDamaged;

    public AnimalData AnimalData { get { return _animalData; } set { _animalData = value; } }
    public SphereCollider AggroCollider { get { return _aggroCollider; } set { _aggroCollider = value; } }
    public SphereCollider OutOfRangeCollider { get { return _OutOfRangeCollider; } set { _OutOfRangeCollider = value; } }
    public GameObject AreaHolder { get { return _areaHolder; } }
    public Animator Anim { get { return _anim; } }
    public AreaClass CurrentArea { get { return _currentArea; } set { _currentArea = value; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public Player_Controller Player { get { return _player; } set { _player = value; } }
    public string AnimalName { get { return _animalName; }}
    public int Health { get { return _maxHealth; } set { _maxHealth = value; } }
    public int BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
    public int TotalDamage { get { return _totalDamage; } set { _totalDamage = value; } }
    public float BaseSpeed { get { return _baseSpeed; } set { _baseSpeed = value; } }
    public float LoiterTimer { get { return _loiterTimer; } set { _loiterTimer = value; } }
    public float StuckTimer { get { return _stuckTimer; } set { _stuckTimer = value; } }
    public float WanderTimer { get { return _wanderTimer; } set { _wanderTimer = value; } }
    public float BaseLoiterTime { get { return _baseLoiterTime; } }
    public float BaseWanderTime { get { return _baseWanderTime; } }
    public float AttackRange { get { return _attackRange; } }
    public bool AgentHasPath { get { return _agentHasPath; } set { _agentHasPath = value; } }
    public bool HasDamaged { get { return _hasDamaged; } set { _hasDamaged = value; } }
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public bool IsGoingToNextArea { get { return _isGoingToNextArea; } set { _isGoingToNextArea = value; } }
    public bool IsAgitated { get { return _isAgitated; } set { _isAgitated = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsPlayerInRange { get { return _isPlayerInRange; } set { _isPlayerInRange = value; } }
    public bool IsPassive { get { return _isPassive; } set { _isPassive = value; } }


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        hitBox = transform.GetChild(0).GetComponent<Collider>();
        _aggroCollider = transform.GetChild(1).GetComponent<SphereCollider>();
        _OutOfRangeCollider = transform.GetChild(2).GetComponent<SphereCollider>();
    }
    public virtual void Start()
    {
        if (_animalData)
        {
            _animalName = _animalData.AnimalName;
            _isPassive = _animalData.IsPassive;
            _baseSpeed = _animalData.BaseMovementSpeed;
            Agent.speed = _baseSpeed;
            _baseWanderTime = _animalData.BaseWanderTime;
            _baseLoiterTime = _animalData.BaseLoiterTime;
            _aggroCollider.radius = _animalData.AggroRange;
            _OutOfRangeCollider.radius = _animalData.OutofRange;
            _attackRange = _animalData.AttackRange;
            _maxHealth = _animalData.Health;
            _currentHealth = _maxHealth;
            _baseDamage = _animalData.BaseDamage;
            _totalDamage = _baseDamage;
        }

        _areaHolder = GameObject.FindGameObjectWithTag("AreaHolder")?.gameObject;

        if(!_areaHolder)
        {
            Debug.Log("Heads up! Area Holder does not exist. I cannot move");
            return;
        }

        _anim = GetComponent<Animator>();
        if(Player_Controller.instance != null)
        {
            _player = Player_Controller.instance;
        }

        _loiterTimer = RandomizeTimer(_baseLoiterTime);
        _wanderTimer = RandomizeTimer(_baseWanderTime);

        _hasDamaged = false;
        _isAgitated = false;
        _isGoingToNextArea = false;
        CanMove = true;


        areas = new AreaClass[_areaHolder.transform.childCount];
        for(int i = 0; i < _areaHolder.transform.childCount; i++)
        {
            areas[i] = _areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }
       
        GetAreaData();
    }
    public float RandomizeTimer(float baseTime)
    {
        float randomTime = Random.Range(baseTime - (baseTime * 0.2f), baseTime + (baseTime * 0.2f));
        return randomTime;
    }
    public virtual void Update()
    {
        if (!_areaHolder)
        {
            return;
        }

        if(IsDead)
        {
            if(Agent.enabled)
            {
               Agent.SetDestination(transform.position);
                _agentHasPath = false;
            }

            Agent.enabled = false;
            _aggroCollider.enabled = false;
            _OutOfRangeCollider.enabled = false;
            hitBox.enabled = false;
            return;
        }

        if (_isPlayerInRange)
        {
            if (_isPassive)
            {
                if (_isAgitated)
                {
                    AggressiveState();
                }
                else
                {
                    CalmState();
                }
            }
            else
            {
                AggressiveState();
            }
        }
        else
        {
            CalmState();
        }

        if (IsGoingToNextArea)
        {
            Agent.speed = _baseSpeed + (_baseSpeed * 0.5f);
            if (Agent.remainingDistance <= 5)
            {
                IsAgitated = true;
                IsGoingToNextArea = false;
            }
        }
        else
        {
            Agent.speed = _baseSpeed;
        }

        if (_agent.remainingDistance > 0)
        {
            _anim.SetBool("isWalking", true);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }

    }

    private void LateUpdate()
    {
        if (_agentHasPath)
        {
            float currentDistance = Vector3.Distance(transform.position, Agent.destination);
            if (currentDistance < lastDistance)
            {
                // Agent is getting closer, this is we want to see, go on, also reset give up time
                lastDistance = currentDistance;
                _stuckTimer = 3f;
            }
            else
            {
                // Cannot proceed closer, countdown time to giveup
                _stuckTimer  -= Time.deltaTime;
                if (_stuckTimer <= 0)
                {
                    RandomWanderAroundCurrentArea();
                }
            }
        }
    }


    #region Public Methods To Call
    public void HasBeenAgitated()
    {
        int DoRandomAction = Random.Range(0, 2);
        if (DoRandomAction == 0)
        {
            //Debug.Log("I have chosen to fight");
            IsAgitated = true;
        }
        else if (DoRandomAction == 1)
        {
            //Debug.Log("I have chosen to run away, but ill fight next time");
            MoveToNextArea();
        }
    }
    public IEnumerator StartAttackCoolDown(float attackCoolDown)
    {
        yield return new WaitForSeconds(attackCoolDown);
        CanMove = true;
        _isAttacking = false;
    }
    public void TakeDamage(int value)
    {
        if (_currentHealth - value >= 0)
        {
            _currentHealth -= value;
        }
        else
        {
            _currentHealth = 0;
            _anim.SetBool("isDead", true);
            _isDead = true;
        }
    }
    public void Add_MotionValue(float percentage)
    {
        _totalDamage = MotionValue(BaseDamage, percentage);
    }
    #endregion Public Methods To Call

    #region States
    public void AggressiveState()
    {
        float distance = Vector3.Distance(transform.position, Player.gameObject.transform.position);

        Agent.speed  = _baseSpeed +(_baseSpeed * 0.25f);
        if (CanMove)
        {
            if (distance <= AttackRange)
            {
                Agent.SetDestination(transform.position);
                _agentHasPath = false;

                RotateTowardsPlayer(1);
                IsFacingPlayer();
            }
            else
            {
                Agent.SetDestination(Player.gameObject.transform.position);
                RotateTowardsPlayer(0.2f);
            }
        }
        else
        {
            Agent.SetDestination(transform.position);
            _agentHasPath = false;

        }
    }
    public virtual void CalmState()
    {
        if (_wanderTimer > 0)
        {
            _wanderTimer -= Time.deltaTime;
            if (_agent.remainingDistance <= 0)
            {
                if(_loiterTimer > 0)
                {
                    _loiterTimer -= Time.deltaTime;
                    _agent.SetDestination(_agent.transform.position);
                    _agentHasPath = false;

                    //Debug.Log("I will be Idle here");
                }
                else
                {
                    _loiterTimer = RandomizeTimer(_baseLoiterTime);
                    RandomWanderAroundCurrentArea();
                    //Debug.Log("Im gonna move around here a bit");
                }
            }
        }
        else
        {
            if (!_isGoingToNextArea)
            {
                //Debug.Log("Im going to the next Area");
                MoveToNextArea();
                _agent.speed = 15;
            }
            if (_agent.remainingDistance <= 0)
            {
                WanderTimer = RandomizeTimer(_baseWanderTime);
                _isGoingToNextArea = false;
                _agent.speed = 3;
                //Debug.Log("Arrived at new Area");
            }
        }
    }
    public void RotateTowardsPlayer(float speed)
    {
        if (_isAttacking)
        {
            return;
        }
        Vector3 direction = Player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, speed);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    public void DoRandomAttack()
    {
        _hasDamaged = false;

        int RandomAttack = Random.Range(0, 2);
        if (RandomAttack == 0)
        {
            Anim.SetTrigger("Attack1");
        }
        else if (RandomAttack == 1)
        {
            Anim.SetTrigger("Attack2");
        }
        _isAttacking = true;
        CanMove = false;
    }
    public virtual void MoveToNextArea()
    {
        int randomArea = Random.Range(0, _currentArea.NeighboringAreas.Count);
        {
            for(int i = 0; i < _currentArea.NeighboringAreas.Count; i++)
            {
                if(i == randomArea)
                {
                    _currentArea = _currentArea.NeighboringAreas[i];
                    GetAreaData();

                    Vector3 referenceDestination = _currentArea.transform.position;
                    Vector3 newDestination = RaycastDownArea(referenceDestination);
                    _agent.SetDestination(newDestination);
                    _isGoingToNextArea = true;
                }
            }
        }
    }
    #endregion States

    #region GetInfo
    public void GetAreaData()
    {
        scaleX = _currentArea.transform.localScale.x / 2;
        scaleZ = _currentArea.transform.localScale.z / 2;
    }
    public void IsFacingPlayer()
    {
        Vector3 offsetTrasform = transform.position;
        offsetTrasform.y = transform.position.y + (Agent.height / 2);

        RaycastHit hit;
        if (Physics.Raycast(offsetTrasform, transform.forward, out hit, AttackRange + 0.5f))
        {
            if (hit.transform.tag == "Player")
            {
                DoRandomAttack();
            }
        }
    }
    public void RandomWanderAroundCurrentArea()
    {
        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 referenceDestination = new Vector3(randomX, 0, randomZ) + _currentArea.transform.position;

        Vector3 newDestinaion = RaycastDownArea(referenceDestination);

        _agent.SetDestination(newDestinaion);
    }
    public Vector3 RaycastDownArea(Vector3 referenceDestination)
    {
        RaycastHit hit;
        if (Physics.Raycast(referenceDestination, Vector3.down, out hit, Mathf.Infinity, layerToFollow))
        {
            Debug.DrawLine(referenceDestination, hit.point, Color.green, 5f);
            _agentHasPath = true;
            return hit.point;
        }
        return Vector3.zero;
    }
    private int MotionValue(int baseDamage, float PercentageModifier)
    {
        int totalValue = (int)((float)baseDamage + (float)((float)baseDamage * PercentageModifier));
        return totalValue;
    }
    #endregion GetInfo
}

