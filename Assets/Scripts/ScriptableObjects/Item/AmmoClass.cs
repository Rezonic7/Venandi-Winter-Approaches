using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Create Data/Item/Ammo Data")]

public class AmmoClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return null; }
    public override AmmoClass GetAmmo() { return this; }

}
