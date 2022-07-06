using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponEquipment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private WeaponTypeData _weaponData;
    public WeaponTypeData WeaponData { get { return _weaponData; } set { _weaponData = value; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_weaponData)
        {
            EquipmentManager.instance.ShowWeaponPreview(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_weaponData)
        {
            EquipmentManager.instance.HideWeaponPreview();
        }
    }
    public void EquipWeapon()
    {
        if(_weaponData)
        {
            EquipmentManager.instance.UpdateEquippedWeapon(this, true);
            transform.GetChild(1).GetComponent<Text>().text = "E";
        }
    }
}
