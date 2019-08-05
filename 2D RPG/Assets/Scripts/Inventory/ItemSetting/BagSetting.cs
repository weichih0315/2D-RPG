using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class BagSetting : ItemSetting
{
    [SerializeField]
    private int slotCount;
    public int SlotCount
    {
        get
        {
            return slotCount;
        }

        set
        {
            slotCount = value;
        }
    }
}
