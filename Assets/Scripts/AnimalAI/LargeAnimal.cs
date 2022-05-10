using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LargeAnimal : MonoBehaviour
{
    [SerializeField] private AnimalClass animalData;
    [SerializeField] private GameObject areaHolder;
    private AreaClass[] areas;
    private NavMeshAgent agent;

    public GameObject destinationDebugger;

    private AreaClass currentArea;
    private float scaleX;
    private float scaleZ;

    private float wanderTimer;
    [SerializeField] private float wanderTime;
    private float loiterTimer;
    [SerializeField] private float loiterTime;

    private bool isGoingToNextArea;
    private bool isAlert;

    private void Start()
    {
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

        currentArea = areas[0];
        GetAreaData();
    }
    
    private void Update()
    {
        if(isAlert)
        {

        }
        else
        {
            UnalertedState();
        }
    }
    void UnalertedState()
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

                }
                else
                {
                    loiterTimer = loiterTime;
                    RandomWanderAroundCurrentArea();

                }
            }
        }
        else
        {
            if (!isGoingToNextArea)
            {
                Debug.Log("Im going to the next Area");
                MoveToNextArea();
            }
            if (agent.remainingDistance <= 0)
            {
                wanderTimer = wanderTime;
                isGoingToNextArea = false;
                Debug.Log("Arrived at new Area");
            }
        }
    }
    void GetAreaData()
    {
        scaleX = currentArea.transform.localScale.x/2;
        scaleZ = currentArea.transform.localScale.z/2;
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
    void RandomWanderAroundCurrentArea()
    {
        float randomX = Random.Range(-scaleX, scaleX);
        float randomZ = Random.Range(-scaleZ, scaleZ);
        Vector3 referenceDestination = new Vector3(randomX, 0, randomZ) + currentArea.transform.position;
        
        Vector3 newDestinaion = RaycastDownArea(referenceDestination);

        destinationDebugger.transform.position = newDestinaion;
        agent.SetDestination(newDestinaion);
    }
    private Vector3 RaycastDownArea(Vector3 referenceDestination)
    {
        RaycastHit hit;
        if (Physics.Raycast(referenceDestination, Vector3.down, out hit, Mathf.Infinity, LayerMask.NameToLayer("Ground")))
        {
            Debug.DrawLine(referenceDestination, hit.point, Color.green, 5f);
            return hit.point;
        }
        return Vector3.zero;
    }
}

