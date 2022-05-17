using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalClass : MonoBehaviour
{
    [SerializeField] private AnimalData animalData;
    [SerializeField] private float _attackRange;
    [SerializeField] LayerMask layerToFollow;
    private GameObject _areaHolder;
    private AreaClass[] areas;
    private AreaClass _currentArea;
    private NavMeshAgent _agent;
    private Animator _anim;

    private string _animalName;
    private float _baseWanderTime;
    private float _baseLoiterTime;
    private float _wanderTimer;
    private float _loiterTimer;
    private float scaleX;
    private float scaleZ;
    private float AttackDistance;

    private SphereCollider _aggroCollider;
    private SphereCollider _OutOfRangeCollider;

    private bool _isGoingToNextArea;
    [SerializeField] private bool _isPassive;
    private bool _isAgitated;
    private bool _isPlayerInRange;
    private bool _isAttacking;

    public SphereCollider AggroCollider { get { return _aggroCollider; } set { _aggroCollider = value; } }
    public SphereCollider OutOfRangeCollider { get { return _OutOfRangeCollider; } set { _OutOfRangeCollider = value; } }
    public GameObject AreaHolder { get { return _areaHolder; } }
    public Animator Anim { get { return _anim; } }
    public string AnimalName { get { return _animalName; }}
    public AreaClass CurrentArea { get { return _currentArea; } set { _currentArea = value; } }
    public float LoiterTimer { get { return _loiterTimer; } set { _loiterTimer = value; } }
    public float WanderTimer { get { return _wanderTimer; } set { _wanderTimer = value; } }
    public float BaseLoiterTime { get { return _baseLoiterTime; } }
    public float BaseWanderTime { get { return _baseWanderTime; } }
    public float AttackRange { get { return _attackRange; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public bool IsGoingToNextArea { get { return _isGoingToNextArea; } set { _isGoingToNextArea = value; } }
    public bool IsAgitated { get { return _isAgitated; } set { _isAgitated = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsPlayerInRange { get { return _isPlayerInRange; } set { _isPlayerInRange = value; } }
    public bool IsPassive { get { return _isPassive; } set { _isPassive = value; } }

    public Player_Controller player;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _aggroCollider = transform.GetChild(1).GetComponent<SphereCollider>();
        _OutOfRangeCollider = transform.GetChild(2).GetComponent<SphereCollider>();

        if (animalData != null)
        {
            _animalName = animalData.AnimalName;
            _isPassive = animalData.IsPassive;
            _agent.speed = animalData.BaseMovementSpeed;
            _baseWanderTime = animalData.BaseWanderTime;
            _baseLoiterTime = animalData.BaseLoiterTime;
            _aggroCollider.radius = animalData.AggroRange;
            _OutOfRangeCollider.radius = animalData.OutofRange;
            Instantiate(animalData.AnimalMesh,  this.gameObject.transform.GetChild(0).transform);
        }
    }
    void Start()
    {
        _areaHolder = GameObject.FindGameObjectWithTag("AreaHolder")?.gameObject;

        if(!_areaHolder)
        {
            Debug.Log("Heads up! Area Holder does not exist. I cannot move");
            return;
        }

        _anim = GetComponent<Animator>();
        if(Player_Controller.instance != null)
        {
            player = Player_Controller.instance;
        }

        _loiterTimer = RandomizeTimer(_baseLoiterTime);

        _isAgitated = false;
        _wanderTimer = RandomizeTimer(_baseWanderTime);

        _isGoingToNextArea = false;

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
        if (_isAttacking)
        {
            _agent.SetDestination(transform.position);
            return;
        }

        if (_isAgitated)
        {
            if (_isAttacking)
            {
                return;
            }
            if (_isPlayerInRange)
            {
                _agent.SetDestination(player.transform.position);
                if (_agent.remainingDistance <= _attackRange)
                {
                    _agent.SetDestination(transform.position);
                    DoRandomAttack();
                    _isAttacking = true;
                }
            }
            else
            {
                PassiveState();
            }
        }
        else
        {
            PassiveState();
        }
    }

    public void DoRandomAttack()
    {
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
    }
    #region UnaltertedState
    public void GetAreaData()
    {
        scaleX = _currentArea.transform.localScale.x / 2;
        scaleZ = _currentArea.transform.localScale.z / 2;
    }
    public virtual void PassiveState()
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
                    Debug.Log("I will be Idle here");
                }
                else
                {
                    _loiterTimer = RandomizeTimer(_baseLoiterTime);
                    RandomWanderAroundCurrentArea();
                    Debug.Log("Im gonna move around here a bit");
                }
            }
        }
        else
        {
            if (!_isGoingToNextArea)
            {
                Debug.Log("Im going to the next Area");
                MoveToNextArea();
                _agent.speed = 15;
            }
            if (_agent.remainingDistance <= 0)
            {
                WanderTimer = RandomizeTimer(_baseWanderTime);
                _isGoingToNextArea = false;
                _agent.speed = 3;
                Debug.Log("Arrived at new Area");
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
    public Vector3 RaycastDownArea(Vector3 referenceDestination)
    {
        RaycastHit hit;
        if (Physics.Raycast(referenceDestination, Vector3.down, out hit, Mathf.Infinity, layerToFollow))
        {
            Debug.DrawLine(referenceDestination, hit.point, Color.green, 5f);
            return hit.point;
        }
        return Vector3.zero;
    }
    #endregion UnaltertedState
}

