using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalClass : MonoBehaviour
{
    [SerializeField] private AnimalData animalData;
    [SerializeField] private float _attackRange;
    [SerializeField] LayerMask layerToFollow;
    private GameObject areaHolder;
    private AreaClass[] areas;
    private AreaClass _currentArea;
    private NavMeshAgent _agent;

    private float _baseWanderTime;
    private float _baseLoiterTime;
    private float _wanderTimer;
    private float _loiterTimer;
    private float scaleX;
    private float scaleZ;

    private bool isGoingToNextArea;
    [SerializeField] private bool _isPassive;
    private bool _isAgitated;
    private bool _isPlayerInRange;
    private bool _isAttacking;
    
    public AreaClass CurrentArea { get { return _currentArea; } set { _currentArea = value; } }
    public float LoiterTimer { get { return _loiterTimer; } set { _loiterTimer = value; } }
    public float WanderTimer { get { return _wanderTimer; } set { _wanderTimer = value; } }
    public float BaseLoiterTime { get { return _baseLoiterTime; } }
    public float BaseWanderTime { get { return _baseWanderTime; } }
    public float AttackRange { get { return _attackRange; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public bool IsAgitated { get { return _isAgitated; } set { _isAgitated = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsPlayerInRange { get { return _isPlayerInRange; } set { _isPlayerInRange = value; } }
    public bool IsPassive { get { return _isPassive; } set { _isPassive = value; } }

    public Player_Controller player;
    void Start()
    {
        if(Player_Controller.instance != null)
        {
            player = Player_Controller.instance;
        }

        areaHolder = GameObject.FindGameObjectWithTag("AreaHolder");

        _isAgitated = false;
        _wanderTimer = RandomizeTimer(_baseWanderTime);

        isGoingToNextArea = false;
        _agent = GetComponent<NavMeshAgent>();

        areas = new AreaClass[areaHolder.transform.childCount];
        for(int i = 0; i < areaHolder.transform.childCount; i++)
        {
            areas[i] = areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }
        if (animalData != null)
        {
            _isPassive = animalData.IsPassive;
            _agent.speed = animalData.BaseMovementSpeed;
            _baseWanderTime = animalData.BaseWanderTime;
            _baseLoiterTime = animalData.BaseLoiterTime;
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
        if(_isAgitated)
        {
            if (_isAttacking)
            {
                return;
            }
            if (IsPlayerInRange)
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
        int RandomAttack = Random.Range(0,1);
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
            if (!isGoingToNextArea)
            {
                Debug.Log("Im going to the next Area");
                MoveToNextArea();
                _agent.speed = 15;
            }
            if (_agent.remainingDistance <= 0)
            {
                WanderTimer = RandomizeTimer(_baseWanderTime);
                isGoingToNextArea = false;
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
                    isGoingToNextArea = true;
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

