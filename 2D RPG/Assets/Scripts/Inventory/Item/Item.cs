using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    [SerializeField]
    private ItemSetting itemSetting;
    public ItemSetting ItemSetting
    {
        get
        {
            return itemSetting;
        }

        set
        {
            itemSetting = value;
        }
    }

    [SerializeField]
    private int count = 1;
    public int Count
    {
        get
        {
            return count;
        }

        set
        {            
            count = value;
            if (value == 0)
                Destroy(gameObject);
        }
    }

    public virtual Sprite Icon { get { return ItemSetting.Icon; } }

    public virtual string Name { get { return ItemSetting.Name; } }

    public virtual int StackSize { get { return ItemSetting.StackSize; } }       

    public virtual bool IsFull { get { return Count == StackSize; } }

    public virtual string Description { get { return ItemSetting.Description; } }

    public virtual Quality Quality { get { return ItemSetting.Quality; } }

    public virtual bool Use()
    {
        if (itemSetting is IUseable)
        {
            if (itemSetting.Use())
            {
                Count--;
                return true;
            }
            Debug.Log(ItemSetting.Name + " isn't use");
            return false;
        }
        else
        {
            Debug.Log(ItemSetting.Name + " isn't useable");
            return false;
        }            
    }
}
