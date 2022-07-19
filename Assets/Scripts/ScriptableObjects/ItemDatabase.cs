using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Create Data/Database/Item Database")]

public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemClass[] items;
    public Dictionary<ItemClass, int> GetItemID = new Dictionary<ItemClass, int>();
    public Dictionary<int, ItemClass> GetItemData = new Dictionary<int, ItemClass>();

    public void OnAfterDeserialize()
    {
        GetItemID = new Dictionary<ItemClass, int>();
        GetItemData = new Dictionary<int, ItemClass>();

        for (int i = 0; i < items.Length; i++)
        {
            GetItemID.Add(items[i], i);
            GetItemData.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
