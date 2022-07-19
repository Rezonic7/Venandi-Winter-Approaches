using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemClass _itemData;

    public ItemClass ItemData { get { return _itemData; } set { _itemData = value; } }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_itemData)
        {
            InventoryManager.instance.ShowItemPreview(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_itemData)
        {
            InventoryManager.instance.HideItemPreview();
        }
    }

}
