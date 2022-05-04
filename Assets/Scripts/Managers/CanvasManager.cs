using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] private InformationText damageTextPrefab;
    [SerializeField] private InformationText gatheredItemTextPrefab;
    [SerializeField] private GameObject infoTextParent;

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
        g.ChangeText(damage.ToString());
        Destroy(g.gameObject, 1f);
    }
    public void ShowInfo(string info)
    {
        InformationText g;

        g = Instantiate(gatheredItemTextPrefab, transform.position, Quaternion.identity, infoTextParent.transform);
        g.ChangeText(info);
        Destroy(g.gameObject, 3.2f);
    }
}
