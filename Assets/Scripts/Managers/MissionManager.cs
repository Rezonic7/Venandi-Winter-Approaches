using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : Singleton<MissionManager>
{
    private MissionData missionData;
    private GameObject areaHolder;
    private Image inGameClock;

    // make private after testing is done
    private List<int> areasNotSpawnable;
    private AreaClass[] areas;
    private float maxTime;
    private float currentTime;

    private int maxObjectiveGoal;
    private int currentObjectiveGoal;
    
    private bool objectiveIsHunt;

    private AnimalData huntObjective;
    private ItemClass gatheringObjective;

    private Text textGoal;

    private int playerLives;

    private bool isGameOver;
    private GameObject player;
    private GameObject respawnPoint;
    private GameObject gameOverScreen;
    private GameObject winScreen;
    private Animator faderAnim;
    private void Start()
    {
        missionData = GameManager.instance.MissionToLoad;

        player = Player_Controller.instance.transform.gameObject;

        textGoal = GameObject.FindWithTag("TextGoal")?.GetComponent<Text>();
        if (!textGoal)
        {
            Debug.Log("Text Goal not found, this may cause issues with the player's mission");

        }

        if (missionData)
        {
            maxTime = missionData.MissionTime;
            currentTime = missionData.MissionTime;
            playerLives = missionData.PlayerLives;
            maxObjectiveGoal = missionData.ObjectiveGoal;

            currentObjectiveGoal = 0;
            switch (missionData.ObjectiveType)
            {
                case MissionData.ObjectiveTypes.Hunt:
                    objectiveIsHunt = true;
                    huntObjective = missionData.HuntObjective;
                    textGoal.text = ("Objective: " + missionData.ObjectiveType + " " + huntObjective.AnimalName + " " + currentObjectiveGoal + " / " + maxObjectiveGoal);
                    break;
                case MissionData.ObjectiveTypes.Gather:
                    objectiveIsHunt = false;
                    gatheringObjective = missionData.GatherObjective;
                    textGoal.text = ("Objective: " + missionData.ObjectiveType + " " + gatheringObjective.ItemName + " " + currentObjectiveGoal + " / " + maxObjectiveGoal);
                    break;
            }

        }
        else
        {
            Debug.Log("There is no mission data, game will not function properly");
        }

        faderAnim = GameObject.FindWithTag("FadeTransition")?.GetComponent<Animator>();
        if (!faderAnim)
        {
            Debug.Log("Heads up no GameOverPanel detected, Game Over will not work properly");
        }

        gameOverScreen = GameObject.FindWithTag("GameOverPanel")?.gameObject;
        if (!gameOverScreen)
        {
            Debug.Log("Heads up no GameOverPanel detected, Game Over will not work properly");
        }
        else
        {
            gameOverScreen.SetActive(false);
        }

        winScreen = GameObject.FindWithTag("WinPanel")?.gameObject;
        if(!winScreen)
        {
            Debug.Log("Heads up no WinPanel detected, Winning will not work properly");
        }
        else
        {
            winScreen.SetActive(false);
        }

        inGameClock = GameObject.FindWithTag("InGameClock")?.GetComponent<Image>();

        if (!inGameClock)
        {
            Debug.Log("Heads up no InGameClock detected, Mission Time will not work properly");
        }

        respawnPoint = GameObject.FindWithTag("RespawnPoint")?.gameObject;
        if (!respawnPoint)
        {
            Debug.Log("Respawn Point not found, player will no be able to respawn properly");
        } else
        {
            player.transform.position = respawnPoint.transform.position;
            player.transform.rotation = respawnPoint.transform.rotation;
        }

       

        isGameOver = false;

        areasNotSpawnable = new List<int>();
        areaHolder = GameObject.FindGameObjectWithTag("AreaHolder");
        areas = new AreaClass[areaHolder.transform.childCount];
        for (int i = 0; i < areaHolder.transform.childCount; i++)
        {
            areas[i] = areaHolder.transform.GetChild(i).GetComponent<AreaClass>();
        }

        for (int i = 0; i < missionData.AnimalsToSpawn.Count; i++)
        {
            AreaClass currentArea = GetRandomAreaSpawn(missionData.AnimalsToSpawn[i]);
            for (int q = 0; q < (missionData.AnimalsToSpawn[i].Quantity); q++)
            {
                if (currentArea != null)
                {
                    Vector3 areaPos = currentArea.gameObject.transform.position;

                    float scaleX = currentArea.transform.localScale.x / 2;
                    float scaleZ = currentArea.transform.localScale.z / 2;

                    float randomX = Random.Range(-scaleX, scaleX);
                    float randomZ = Random.Range(-scaleZ, scaleZ);
                    Vector3 spawnPos = new Vector3(randomX, 0, randomZ) + currentArea.transform.position;
                    spawnPos.y = 10f;

                    AnimalClass animal = Instantiate(missionData.AnimalsToSpawn[i].AnimalClass, spawnPos, Quaternion.identity, this.transform);
                    animal.CurrentArea = currentArea;
                }
            }
        }

    }
    public void PlayerDeath()
    {
        if (playerLives - 1 > 0)
        {
            playerLives -= 1;
            faderAnim.SetTrigger("FadeOut");
            StartCoroutine(RespawnPlayer());
        }
        else
        {
            if (!isGameOver)
            {
                Invoke("GameOver", 5f);
                isGameOver = true;
            }
        }
    }
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5f);
        player.transform.position = respawnPoint.transform.position;
        player.transform.rotation = respawnPoint.transform.rotation;
        Player_Movement.instance.PlayerController.enabled = true;
        Player_Variables.instance.Respawn();
        Player_Controller.instance.SwitchActionMap("Player3DMovement");
        Player_Controller.instance.IsDead = false;
        Player_Animations.instance.Respawn();
        yield return new WaitForSeconds(2.5f);
        faderAnim.SetTrigger("FadeIn");
        CanvasManager.instance.ShowInfo("Only " + playerLives + " Respawns left");

    }
   
    public void GameOver()
    {
        Player_Controller.instance.IsDead = true;
        gameOverScreen.SetActive(true);
    }
    private void Update()
    {
        UpdateTime();
    }
    public void UpdateHuntGoal(AnimalClass checkIfGoal)
    {
        if(missionData)
        {
            if(objectiveIsHunt)
            {
                if (checkIfGoal.AnimalData == huntObjective)
                {
                    if (currentObjectiveGoal + 1 < maxObjectiveGoal)
                    {
                        currentObjectiveGoal += 1;
                    }
                    else
                    {
                        currentObjectiveGoal = maxObjectiveGoal;
                        Player_Controller.instance.IsDead = true;
                        winScreen.SetActive(true);
                        Player_Controller.instance.SwitchActionMap("DeathScreen");
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
                textGoal.text = ("Objective: " + missionData.ObjectiveType + " " + huntObjective.AnimalName + " " + currentObjectiveGoal + " / " + maxObjectiveGoal);
            }
        }
    }
    public void UpdateGatherGoal(ItemClass checkIfGoal, int amount)
    {
        if (missionData)
        {
            if(!objectiveIsHunt)
            {
                if(checkIfGoal == gatheringObjective)
                {
                    if(currentObjectiveGoal + amount < maxObjectiveGoal)
                    {
                        currentObjectiveGoal += amount;
                    }
                    else
                    {
                        currentObjectiveGoal = maxObjectiveGoal;
                        Player_Controller.instance.IsDead = true;
                        winScreen.SetActive(true);
                        Player_Controller.instance.SwitchActionMap("DeathScreen");
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
                textGoal.text = ("Objective: " + missionData.ObjectiveType + " " + gatheringObjective.ItemName + " " + currentObjectiveGoal + " / " + maxObjectiveGoal);
            }
        }
    }
    void UpdateTime()
    {
        if(Player_Controller.instance.IsDead)
        {
            return;
        }
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            float percentageTime = Mathf.InverseLerp(0, maxTime, currentTime);
            if (inGameClock)
            {
                inGameClock.fillAmount = percentageTime;
            }
            //Debug.Log("Percentage " + percentageTime + ". AND CURRENT TIME " + currentTime);
        }
        else
        {
            if(!isGameOver)
            {
                GameOver();
                isGameOver = true;
            }
        }    

    }
    private AreaClass GetRandomAreaSpawn(AnimalSpawnerClass animalData)
    {
        List<int> currentSpawnData = new List<int>(animalData.AreasToSpawn);
        for (int i = 0; i < animalData.AreasToSpawn.Count; i++)
        {
            int randomIndex = Random.Range(0, currentSpawnData.Count);
            if(!areasNotSpawnable.Contains(currentSpawnData[randomIndex]))
            {
                areasNotSpawnable.Add(currentSpawnData[randomIndex]);
                int GetAreaInt = currentSpawnData[randomIndex];
                AreaClass randomSpawnArea = areas[GetAreaInt - 1];
                return randomSpawnArea;
            }
            else
            {
                currentSpawnData.Remove(currentSpawnData[randomIndex]);
            }
        }
        return null;
    }
}
