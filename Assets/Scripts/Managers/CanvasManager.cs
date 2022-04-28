using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] private InformationText damageTextPrefab;
    [SerializeField] private InformationText gatheredItemTextPrefab;

    private float screensizeX;
    private float screensizeY;

    Vector3 middleOfScreen;

    private void Start()
    {
        screensizeX = Screen.width;
        screensizeY = Screen.height;

        middleOfScreen = new Vector3(screensizeX / 2, screensizeY / 2, 0);
    }

    public void SpawnDamage(int damage, Vector3 spawnPos)
    {
        InformationText g;
        g = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity, this.transform);
        g.textValue = damage.ToString();
        Destroy(g.gameObject, 1f);
    }
    public void ShowInfo(string info, float timeToDestroy)
    {
        InformationText g;
        Vector3 middleOfScreenOffset = middleOfScreen;
        middleOfScreenOffset.y = middleOfScreenOffset.y + (screensizeY * 0.1f);

        g = Instantiate(gatheredItemTextPrefab, middleOfScreenOffset, Quaternion.identity, this.transform);
        g.textValue = info;
        Destroy(g.gameObject, timeToDestroy);
    }
}
