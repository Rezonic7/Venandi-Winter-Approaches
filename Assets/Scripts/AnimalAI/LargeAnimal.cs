using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LargeAnimal : MonoBehaviour
{
    [SerializeField] private AnimalClass animalData;
    [SerializeField] private GameObject areaHolder;
    [SerializeField] private float wanderTime;
    [SerializeField] private float loiterTime;
    [SerializeField] private float attackRange;
    [SerializeField] LayerMask layerToFollow;
    private AreaClass[] areas;
    private AreaClass currentArea;
    private NavMeshAgent agent;

    private float scaleX;
    private float scaleZ;
    private float wanderTimer;
    private float loiterTimer;

    private bool isGoingToNextArea;
    [SerializeField] private bool _isPassive;
    private bool _isPlayerInRange;
    private bool isAttacking;
    public bool IsPlayerInRange { get { return _isPlayerInRange; } set { _isPlayerInRange = value; } }
    public bool IsPassive { get { return _isPassive; } set { _isPassive = value; } }

    public Player_Controller player;
    public GameObject destinationDebugger;

    private void Start()
    {
        if(Player_Controller.instance != null)
        {
            player = Player_Controller.instance;
        }
        if (animalData != null)
        {
            _isPassive = animalData.IsPassive;
        }

        wanderTimer = wanderTime;

        isGoingToNextArea = false;
        agent = GetComponent<NavMeshAgent>();

        areas = new AreaClass[areaHolder.transform.childCount];
        for(int i = 0; i < areaHolder.transform.childCount; i++)
        {
            areas[i] = areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }

        int randomSpawnInt = Random.Range(2, areas.Length);
        Vector3 randomSpawn = areas[randomSpawnInt].gameObject.transform.position;
        randomSpawn.y = 10f;
        transform.position = randomSpawn;
        agent.SetDestination(randomSpawn);
        currentArea = areas[randomSpawnInt];
        GetAreaData();
    }
    
    private void Update()
    {
        if(IsPassive)
        {
            PassiveState();
        }
        else
        {
            if(isAttacking)
            {
                return;
            }
            if(IsPlayerInRange)
            {
                agent.SetDestination(player.transform.position);
                if(agent.remainingDistance <= attackRange)
                {
                    agent.SetDestination(transform.position);
                    DoRandomAttack();
                    isAttacking = true;
                }
            }
            else
            {
                PassiveState();
            }
        }
        
    }
    private void DoRandomAttack()
    {
        int RandomAttack = Random.Range(0,1);
    }
    #region UnaltertedState
    void GetAreaData()
    {
        scaleX = currentArea.transform.localScale.x / 2;
        scaleZ = currentArea.transform.localScale.z / 2;
    }
    void PassiveState()
    {
        if (wanderTimer > 0)
        {
            wanderTimer -= Time.deltaTime;
            if (agent.remainingDistance <= 0)
            {
                if(loiterTimer > 0)
                {
                    loiterTimer -= Time.deltaTime;
                    agent.SetDestination(agent.transform.position);
                    Debug.Log("I will be Idle here");
                }
                else
                {
                    loiterTimer = loiterTime;
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
                agent.speed = 15;
            }
            if (agent.remainingDistance <= 0)
            {
                wanderTimer = wanderTime;
                isGoingToNextArea = false;
                agent.speed = 3;
                Debug.Log("Arrived at new Area");
            }
        }
    }
    void RandomWanderAroundCurrentArea()
    {
        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 referenceDestination = new Vector3(randomX, 0, randomZ) + currentArea.transform.position;

        Vector3 newDestinaion = RaycastDownArea(referenceDestination);

        destinationDebugger.transform.position = newDestinaion;
        agent.SetDestination(newDestinaion);
    }
    void MoveToNextArea()
    {
        int randomArea = Random.Range(0, currentArea.NeighboringAreas.Count);
        {
            for(int i = 0; i < currentArea.NeighboringAreas.Count; i++)
            {
                if(i == randomArea)
                {
                    currentArea = currentArea.NeighboringAreas[i];
                    GetAreaData();

                    Vector3 referenceDestination = currentArea.transform.position;
                    Vector3 newDestination = RaycastDownArea(referenceDestination);
                    destinationDebugger.transform.position = newDestination;
                    agent.SetDestination(newDestination);
                    isGoingToNextArea = true;
                }
            }
        }
    }
    private Vector3 RaycastDownArea(Vector3 referenceDestination)
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

