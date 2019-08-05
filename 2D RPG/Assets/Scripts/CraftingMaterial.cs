using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CraftingMaterial{

    [SerializeField]
    private Item item;
    public Item Item
    {
        get
        {
            return item;
        }
    }

    [SerializeField]
    private int count;
    public int Count
    {
        get
        {
            return count;
        }
    }
}
