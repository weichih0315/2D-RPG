using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Helmet, Shoulders, Chest, Gloves, Legs, Feet, MainHand, Offhand, TwoHand }

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 2)]
public class EquipItemSetting : ItemSetting,IUseable {

    [SerializeField]
    private EquipmentType equipType;
    public EquipmentType EquipType
    {
        get
        {
            return equipType;
        }

        set
        {
            equipType = value;
        }
    }

    [SerializeField]
    private int armorModifier;
    public int ArmorModifier
    {
        get
        {
            return armorModifier;
        }

        set
        {
            armorModifier = value;
        }
    }

    [SerializeField]
    private int damageModifier;
    public int DamageModifier
    {
        get
        {
            return damageModifier;
        }

        set
        {
            damageModifier = value;
        }
    }    

    [SerializeField]
    private int speedModifier;
    public int SpeedModifier
    {
        get
        {
            return speedModifier;
        }

        set
        {
            speedModifier = value;
        }
    }

    [SerializeField]
    private AnimationClip[] animationClips;
    public AnimationClip[] AnimationClips
    {
        get
        {
            return animationClips;
        }
    }

    public override string Description
    {
        get
        {
            string str = string.Empty;

            if (ArmorModifier > 0)
            {
                str += string.Format(" +{0} armor\n", ArmorModifier);
            }
            if (DamageModifier > 0)
            {
                str += string.Format(" +{0} damage\n", DamageModifier);
            }
            if (SpeedModifier > 0)
            {
                str += string.Format(" +{0} speed\n", SpeedModifier);
            }

            str = str.Remove(str.Length - 1);
            return str;
        }
    }

    public override bool Use()
    {
        return base.Use();
    }
}
