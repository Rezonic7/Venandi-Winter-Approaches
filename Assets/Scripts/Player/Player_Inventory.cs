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
    [SerializeField] private GameObject hotBarSelecter;
    
    private SlotClass[] items;
    private GameObject[] slots;
    private GameObject[] hotBar;

    private SlotClass movingSlot;
    private SlotClass originalSlot;
    private SlotClass tempSlot;
    private SlotClass backupSlot;

    private int hotBarInt = 0;

    private bool _isMovingItem = false;
    private bool _canUseItem = false;

    private Vector2 mousePosition;

    public bool IsMovingItem { get { return _isMovingItem; } }
    public bool CanUseItem {  get { return _canUseItem; } }

    void Start()
    {
        hotBarInt = 0;


        _isMovingItem = false;

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

        hotBarSelecter.transform.position = GetHotbarPosition();

    }
    public void AddItem(ItemClass newItem, int amount)
    {
        SlotClass slot = Contains(newItem);
        if (slot != null)
        {
            if (slot.Quantity + amount <= newItem.MaxStack)
            {
                CanvasManager.instance.ShowInfo("You have recieved " + newItem.ItemName + " x" + amount + "!");
                slot.Quantity += amount;
            }
            else
            {
                CanvasManager.instance.ShowInfo(newItem.ItemName + " is already at Max Capacity");
                slot.Quantity = newItem.MaxStack;
            }
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Item == null)
                {
                    CanvasManager.instance.ShowInfo("You have recieved " + newItem.ItemName + " x" + amount + "!");
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
        if (_isMovingItem)
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

    public void UseItem()
    {
        SlotClass useItem = items[hotBarInt + 30];
        if(useItem == null)
        {
            return;
        }
        if(useItem.Item.GetConsumable() == null)
        {
            return;
        }
        RemoveItem(useItem.Item, 1);
        return;
    }

    private Vector3 GetHotbarPosition()
    {
        for(int i = 0; i < hotBar.Length; i++)
        {
            if(hotBarInt == i)
            {
                return hotBar[i].transform.position;
            }
        }
        return Vector3.zero;
    }

    private void CheckItemIfUsable()
    {
        SlotClass itemChecking = items[hotBarInt + 30];
        if (itemChecking == null)
        {
            _canUseItem = false;
            return;
        }
        if (itemChecking.Item is ConsumableClass)
        {
            _canUseItem = true;
        }
        else
        {
            _canUseItem = false;
        }
    }

    public void ScrollRight()
    {
        if (hotBarInt < 9)
        {
            hotBarInt += 1;
        }
        else
        {
            hotBarInt = 0;
        }
        hotBarSelecter.transform.position = GetHotbarPosition();
        CheckItemIfUsable();
    }
    public void ScrollLeft()
    {
        if (hotBarInt > 0)
        {
            hotBarInt -= 1;
        }
        else
        {
            hotBarInt = 9;
        }
        hotBarSelecter.transform.position = GetHotbarPosition();
    }

    public void OnInventoryClick(InputAction.CallbackContext value)
    {
        if (!value.started)
        {
            return;
        }
        if (_isMovingItem)
        {
            EndMoveItem();
            return;
        }
        StartMoveItem();
    }

    private void StartMoveItem()
    {
        originalSlot = GetClosestSlot();
        backupSlot = originalSlot;
        if (originalSlot == null || originalSlot.Item == null)
        {
            return;
        }
        movingSlot = new SlotClass(originalSlot.Item, originalSlot.Quantity);
        originalSlot.Clear();
        _isMovingItem = true;
        RefreshUI();
    }

    private void EndMoveItem()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            ReturnToOriginalSlot();
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
                CheckItemIfUsable();
            }
            else
            {
                originalSlot.Item = movingSlot.Item;
                originalSlot.Quantity = movingSlot.Quantity;
                movingSlot.Clear();
                _isMovingItem = false;
                RefreshUI();
                CheckItemIfUsable();
            }

        }
    }
    public void ReturnToOriginalSlot()
    {
        backupSlot.Item = movingSlot.Item;
        backupSlot.Quantity = movingSlot.Quantity;
        movingSlot.Clear();
        _isMovingItem = false;
        RefreshUI();
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
