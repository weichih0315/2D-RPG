using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : Item {

    [SerializeField]
    private List<Slot> slots = new List<Slot>();
    public List<Slot> Slots
    {
        get
        {
            return slots;
        }

        set
        {
            slots = value;
        }
    }
    
    public BagUI BagUI { get; set; }
    
    public bool AddItem(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.Item = item;
                return true;
            }
        }
        return false;
    }
}
