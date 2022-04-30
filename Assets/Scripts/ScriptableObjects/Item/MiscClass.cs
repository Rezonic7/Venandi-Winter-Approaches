using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Misc Item", menuName = "Create Data/Item/Misc Data")]

public class MiscClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override MiscClass GetMisc() { return this; }
    public override ConsumableClass GetConsumable() { return null; }
}
