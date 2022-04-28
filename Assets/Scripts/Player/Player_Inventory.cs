using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Inventory : Singleton<Player_Inventory>
{
    public List<ItemData> items;
    public List<int> currentStacks;
    public List<Image> Image;

    public void AddItem(ItemData newItem, int amount)
    {
        if(items.Count >= 10)
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
        UpdateInventory();
        Stack(newItem, amount);
    }

    void Stack(ItemData newItem, int amount)
    {
        if (currentStacks[items.IndexOf(newItem)] + amount <= newItem.MaxStack)
        {
            int totalAmount = currentStacks[items.IndexOf(newItem)] + amount;
            currentStacks[items.IndexOf(newItem)] = totalAmount;
            Image[items.IndexOf(newItem)].sprite = newItem.ImageSprite;
            Image[items.IndexOf(newItem)].gameObject.GetComponentInChildren<Text>().text = totalAmount.ToString();
        }
        else
        {
            Debug.Log("Can't Carry Anymore");
            currentStacks[items.IndexOf(newItem)] = newItem.MaxStack;
        }
    }
    void UpdateInventory()
    {
        for(int i = 0; i < items.Count; i++)
        {

            if (items.Count != currentStacks.Count)
            {
                currentStacks.Add(0);
            }
            else
            {
                return;
            }
        }
    }
}
