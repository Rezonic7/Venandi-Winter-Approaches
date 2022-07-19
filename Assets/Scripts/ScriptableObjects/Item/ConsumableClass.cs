using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Create Data/Item/Consumable Data")]
public class ConsumableClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return this; }
    public override AmmoClass GetAmmo() { return null; }
    
}
