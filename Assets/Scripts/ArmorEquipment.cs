using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmorEquipment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ArmorData _armorData;
    public ArmorData ArmorData { get { return _armorData; } set { _armorData = value; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_armorData)
        {
            EquipmentManager.instance.ShowArmorPreview(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_armorData)
        {
            EquipmentManager.instance.HideArmorPreview();
        }
    }
    public void EquipArmor()
    {
        if (_armorData)
        {
            EquipmentManager.instance.UpdateEquippedArmor(this, true);
            transform.GetChild(1).GetComponent<Text>().text = "E";
        }
    }
}
