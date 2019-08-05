using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSetting : ScriptableObject
{
    [SerializeField]
    private new string name;
    public virtual string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    [SerializeField]
    private Sprite icon;
    public virtual Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    [SerializeField]
    private int stackSize;
    public virtual int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    [SerializeField]
    private Quality quality;
    public virtual Quality Quality
    {
        get
        {
            return quality;
        }

        set
        {
            quality = value;
        }
    }

    [SerializeField]
    [TextArea]
    private string description;
    public virtual string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }    

    public virtual bool Use()
    {
        Debug.Log("Use " + name);
        return true;
    }    
}

