using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : Singleton<Player_Inventory>
{
    [SerializeField] private GameObject slotHolder;

    public List<ItemClass> items = new List<ItemClass>();
    public int[] currentStacks;

    private GameObject[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        currentStacks = new int[slotHolder.transform.childCount];
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();
    }

    public void AddItem(ItemClass newItem, int amount)
    {
        if (items.Count >= slots.Length + 1)
        {
            Debug.Log("Inventory Full");
            return;
        }
        if (items.Contains(newItem))
        {
            Stack(newItem, amount);
            return;
        }
        items.Add(newItem);
        Stack(newItem, amount);
        RefreshUI();
    }

    void Stack(ItemClass newItem, int amount)
    {
        if (currentStacks[items.IndexOf(newItem)] + amount <= newItem.MaxStack)
        {
            int totalAmount = currentStacks[items.IndexOf(newItem)] + amount;
            currentStacks[items.IndexOf(newItem)] = totalAmount;
        }
        else
        {
            Debug.Log("Can't Carry Anymore");
            currentStacks[items.IndexOf(newItem)] = newItem.MaxStack;
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(1).GetComponent<Text>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].ImageSprite;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = currentStacks[i].ToString();
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = 0.ToString();
            }
        }
    }
}
