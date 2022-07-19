using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventoryToggle : Singleton<ShopInventoryToggle>
{
    public Canvas inventoryCanvas;
    public Canvas chestCanvas;

    public Animator anim;

    public bool _isInChest;

    // Start is called before the first frame update
    void Start()
    {
        inventoryCanvas = this.transform.GetChild(0).GetComponent<Canvas>();
        chestCanvas = this.transform.GetChild(1).GetComponent<Canvas>();

        anim = GetComponent<Animator>();

        _isInChest = false;
    }

    public void Toggle()
    {
        if(!_isInChest)
        {
            anim.SetTrigger("ToggleToChest");
            _isInChest = true;
        }
        else
        {
            anim.SetTrigger("ToggleToInventory");
            _isInChest = false;
        }
    }

    public void ChestInFront()
    {
        inventoryCanvas.sortingOrder = 1;

        chestCanvas.sortingOrder = 20;
    }
    public void InventoryInFront()
    {
        inventoryCanvas.sortingOrder = 20;

        chestCanvas.sortingOrder = 1;
    }
}
