using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player_Inventory : Singleton<Player_Inventory>
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject hotBarHolder;
    [SerializeField] private GameObject Cursor;

    [SerializeField] private SlotClass[] startingItems;
    private SlotClass[] items;

    private GameObject[] slots;

    private GameObject[] hotBar;

    private SlotClass movingSlot;
    private SlotClass originalSlot;
    private SlotClass tempSlot;

    private bool isMovingItem = false;

    private Vector2 mousePosition;

    void Start()
    {
        isMovingItem = false;

        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        hotBar = new GameObject[hotBarHolder.transform.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            items[i] = new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < 10; i++)
        {
            hotBar[i] = hotBarHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();
    }
    public void AddItem(ItemClass newItem, int amount)
    {
        SlotClass slot = Contains(newItem);
        if (slot != null)
        {
            if (slot.Quantity + amount <= newItem.MaxStack)
            {
                slot.Quantity += amount;
            }
            else
            {
                Debug.Log("Can't Carry Anymore");
                slot.Quantity = newItem.MaxStack;
            }
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Item == null)
                {
                    items[i] = new SlotClass(newItem, amount);
                    break;
                }
            }
        }

        RefreshUI();
    }
    public void RemoveItem(ItemClass removingItem, int amount)
    {
        SlotClass slotContains = Contains(removingItem);
        if (slotContains != null)
        {
            if (slotContains.Quantity - amount <= 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].Item == removingItem)
                    {
                        items[i].Clear();
                        break;
                    }
                }
            }
            else
            {
                slotContains.Quantity -= amount;
            }
            RefreshUI();
        }
        else
        {
            return;
        }
    }
    public SlotClass Contains(ItemClass item)
    {
        foreach (SlotClass slot in items)
        {
            if (slot.Item == item)
            {
                return slot;
            }
        }
        return null;
    }
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].Item.ImageSprite;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].Quantity.ToString();
                if (i >= 30)
                {
                    int h = i - 30;
                    hotBar[h].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    hotBar[h].transform.GetChild(0).GetComponent<Image>().sprite = items[i].Item.ImageSprite;
                    hotBar[h].transform.GetChild(1).GetComponent<Text>().text = items[i].Quantity.ToString();
                }
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
                if (i >= 30)
                {
                    int h = i - 30;
                    hotBar[h].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    hotBar[h].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    hotBar[h].transform.GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
    }

    public void OnMouseMovement(InputAction.CallbackContext value)
    {
        mousePosition = Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        if (isMovingItem)
        {
            Cursor.SetActive(true);
            Cursor.GetComponent<Image>().sprite = movingSlot.Item.ImageSprite;
            Cursor.transform.position = mousePosition;
        }
        else
        {
            Cursor.SetActive(false);

        }
    }

    public void OnInventoryClick(InputAction.CallbackContext value)
    {
        if (!value.started)
        {
            return;
        }
        if (isMovingItem)
        {
            EndMoveItem();
            return;
        }
        StartMoveItem();
    }

    private void StartMoveItem()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.Item == null)
        {
            return;
        }
        movingSlot = new SlotClass(originalSlot.Item, originalSlot.Quantity);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
    }

    private void EndMoveItem()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            return;
        }
        else
        {
            if(originalSlot.Item != null)
            {
                tempSlot = new SlotClass(movingSlot.Item, movingSlot.Quantity);
                movingSlot = new SlotClass(originalSlot.Item, originalSlot.Quantity);
                originalSlot.Item = tempSlot.Item;
                originalSlot.Quantity = tempSlot.Quantity;
                tempSlot.Clear();
                RefreshUI();
            }
            else
            {
                originalSlot.Item = movingSlot.Item;
                originalSlot.Quantity = movingSlot.Quantity;
                movingSlot.Clear();
                isMovingItem = false;
                RefreshUI();
            }

        }
    }

    private SlotClass GetClosestSlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, mousePosition) <= 70)
            {
                return items[i];
            }
        }
        return null;
    }
}
