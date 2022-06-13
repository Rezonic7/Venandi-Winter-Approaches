using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalClass : MonoBehaviour
{
    [SerializeField] private AnimalData _animalData;
    [SerializeField] LayerMask layerToFollow;
    [SerializeField] private Collider _hurtBox;
    [SerializeField] private Collider _hitBox;
    [SerializeField] private Collider _carveHitBox;
    private GameObject _areaHolder;
    private AreaClass[] areas;
    private AreaClass _currentArea;
    private NavMeshAgent _agent;
    private Animator _anim;
    private Player_Controller _player;
    
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
    private float _aggroRadius;
    private float _outOfRangeRadius;

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
    private bool _hasChosenAggitationAction;
    private bool _isAggressive;
    private bool _isGatherable;


    public AnimalData AnimalData { get { return _animalData; } set { _animalData = value; } }
    public GameObject AreaHolder { get { return _areaHolder; } }
    public Animator Anim { get { return _anim; } }
    public AreaClass CurrentArea { get { return _currentArea; } set { _currentArea = value; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public Player_Controller Player { get { return _player; } set { _player = value; } }
    public Collider CarveHitBox { get { return _carveHitBox; } set { _carveHitBox = value; } }
    public Collider HurtBox { get { return _hurtBox; } set { _hurtBox = value; } }
    public Collider HitBox { get { return _hitBox; } set { _hitBox = value; } }
    public string AnimalName { get { return _animalName; }}
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public float AggroRadius { get { return _aggroRadius; } set { _aggroRadius = value; } }
    public float OutOfRangeRadius { get { return _outOfRangeRadius; } set { _outOfRangeRadius = value; } }
    public int BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
    public int TotalDamage { get { return _totalDamage; } set { _totalDamage = value; } }
    public float BaseSpeed { get { return _baseSpeed; } set { _baseSpeed = value; } }
    public float LoiterTimer { get { return _loiterTimer; } set { _loiterTimer = value; } }
    public float StuckTimer { get { return _stuckTimer; } set { _stuckTimer = value; } }
    public float WanderTimer { get { return _wanderTimer; } set { _wanderTimer = value; } }
    public float BaseLoiterTime { get { return _baseLoiterTime; } }
    public float BaseWanderTime { get { return _baseWanderTime; } }
    public float AttackRange { get { return _attackRange; } }
    public bool IsGatherable { get { return _isGatherable; } set { _isGatherable = value; } }
    public bool HasChosenAggitationAction { get { return _hasChosenAggitationAction; } set { _hasChosenAggitationAction = value; } }
    public bool IsAggressive { get { return _isAggressive; } set { _isAggressive = value; } }
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
    }
    public virtual void Start()
    {
        if (_animalData)
        {
            _animalName = _animalData.AnimalName;
            _isPassive = _animalData.IsPassive;
            _baseSpeed = _animalData.BaseMovementSpeed;
            _agent.speed = _baseSpeed;
            _baseWanderTime = _animalData.BaseWanderTime;
            _baseLoiterTime = _animalData.BaseLoiterTime;
            _aggroRadius = _animalData.AggroRange;
            _outOfRangeRadius = _animalData.OutofRange;
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

        _isAggressive = false;
        _hasDamaged = false;
        _isAgitated = false;
        _hasChosenAggitationAction = false;
        _isGoingToNextArea = false;
        _hurtBox.enabled = false;
        _carveHitBox.enabled = false;
        CanMove = true;


        areas = new AreaClass[_areaHolder.transform.childCount];
        for(int i = 0; i < _areaHolder.transform.childCount; i++)
        {
            areas[i] = _areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }
        RandomCalmAnimation();
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
            Debug.Log("Error Area of this animal could not be determined");
            return;
        }

        if(IsDead)
        {
            if(Agent.enabled)
            {
               Agent.ResetPath();
                _agentHasPath = false;
                _carveHitBox.enabled = true;
            }

            Agent.enabled = false;
            _hitBox.enabled = false;
            _hurtBox.enabled = false;
            return;
        }
        DetermineState();
        SetWalkAnimaion();
    }

    private void LateUpdate()
    {
        //if (_agentHasPath)
        //{
        //    float currentDistance = Vector3.Distance(transform.position, Agent.destination);
        //    if (currentDistance < lastDistance)
        //    {
        //        // Agent is getting closer, this is we want to see, go on, also reset give up time
        //        lastDistance = currentDistance;
        //        _stuckTimer = 3f;
        //    }
        //    else
        //    {
        //        // Cannot proceed closer, countdown time to giveup
        //        _stuckTimer  -= Time.deltaTime;
        //        if (_stuckTimer <= 0)
        //        {
        //            RandomWanderAroundCurrentArea();
        //        }
        //    }
        //}
    }


    #region Public Methods To Call

    public void RotateTowardsPlayer(float speed)
    {
        //if (_isAttacking)
        //{
        //    return;
        //}
        Vector3 direction = Player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, speed);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    public virtual void DoRandomAttack()
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
        for (int i = 0; i < _currentArea.NeighboringAreas.Count; i++)
        {
            if (i == randomArea)
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
    public float MultiplySpeed(float multiplier)
    {
        float totalSpeed = _baseSpeed * multiplier;
        if(totalSpeed >= 15)
        {
            totalSpeed = 15;
        }
        return totalSpeed;
    }
    public void ChargeForward()
    {
        Agent.ResetPath();
        Agent.Move(transform.forward * (_baseSpeed * 10 ) * Time.deltaTime);
    }
    public void HasBeenAgitated()
    {
        if(!_hasChosenAggitationAction)
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
                if (!_isAggressive)
                {
                    Anim.SetTrigger("isAggressive");
                    _isAggressive = true;
                }
            }
            _hasChosenAggitationAction = true;
        }
    }
    public IEnumerator StartAttackCoolDown(float attackCoolDown)
    {
        yield return new WaitForSeconds(attackCoolDown);
        CanMove = true;
        _isAttacking = false;
    }
    public virtual void TakeDamage(int value)
    {
        if (_currentHealth - value >= 0)
        {
            _currentHealth -= value;
        }
        else
        {
            _currentHealth = 0;
            _anim.SetTrigger("isDead");
            if(this.gameObject)
            {
                Destroy(this.gameObject, 180f);
            }
            _isDead = true;
            return;
        }
    }
    public void SetHurtBox(int isTrue)
    {
        if (isTrue <= 0)
        {
            _hurtBox.enabled = false;
        }
        else
        {
            _hurtBox.enabled = true;
        }
    }
    public void SetWalkAnimaion()
    {
        if (_agent.remainingDistance > 0)
        {
            _anim.SetBool("isWalking", true);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }
    }
    public void Add_MotionValue(float percentage)
    {
        _totalDamage = MotionValue(BaseDamage, percentage);
    }
    #endregion Public Methods To Call

    #region States
    public void DetermineState()
    {
        if (!IsGoingToNextArea)
        {
            if (_isPlayerInRange)
            {
                //Debug.Log("Player is in range");
                if (_isPassive)
                {
                    //Debug.Log("Animal is NOT Aggressive");
                    if (_isAgitated)
                    {
                        //Debug.Log("Animal is Aggittated");
                        AggressiveState();
                    }
                    else
                    {
                        //Debug.Log("Animal is NOT Aggittated");
                        CalmState();
                    }
                }
                else
                {
                    // Debug.Log("Animal is Aggressive");
                    AggressiveState();
                }
            }
            else
            {
                //Debug.Log("Player is NOT in range");
                CalmState();
            }

        }
        else
        {
            Agent.speed = MultiplySpeed(4);
            if (Agent.remainingDistance <= 5)
            {
                IsAgitated = true;
                IsGoingToNextArea = false;
                if (_isAggressive)
                {
                    RandomCalmAnimation();
                    _isAggressive = false;
                }
            }
        }

        if(_isPlayerInRange)
        {
            if(GetDistanceToPlayer() >= _outOfRangeRadius)
            {
                _isPlayerInRange = false;
            }
        }
        else
        {
            if (GetDistanceToPlayer() <= _aggroRadius)
            {
                _isPlayerInRange = true;
            }
        }
    }
   
    public virtual void AggressiveState()
    {
        if(_isAttacking)
        {
            return;
        }

        Agent.speed  = MultiplySpeed(3);
        if (CanMove)
        {
            if (GetDistanceToPlayer() <= AttackRange)
            {
               
                Agent.SetDestination(transform.position);
                _agentHasPath = false;

                RotateTowardsPlayer(0.75f);
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

        if (!_isAggressive)
        {
            Anim.SetTrigger("isAggressive");
            _isAggressive = true;
        }
    }
    public virtual void CalmState()
    {
        if(_isAttacking)
        {
            return;
        }
        _agent.speed = _baseSpeed;
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
        if (_isAggressive)
        {
            RandomCalmAnimation();
            _isAggressive = false;
        }
    }
    public void RandomCalmAnimation()
    {
        int randomAnim = Random.Range(0, 2);
        if (randomAnim == 0)
        {
            Anim.SetTrigger("isCalm1");

        }
        else if (randomAnim == 1)
        {
            Anim.SetTrigger("isCalm2");
        }
    }
    
    #endregion States

    #region GetInfo
    public void GetAreaData()
    {
        scaleX = _currentArea.transform.localScale.x / 2;
        scaleZ = _currentArea.transform.localScale.z / 2;
    }
    public float GetDistanceToPlayer()
    {
        float distance = Vector3.Distance(transform.position, Player.gameObject.transform.position);
        return distance;
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

