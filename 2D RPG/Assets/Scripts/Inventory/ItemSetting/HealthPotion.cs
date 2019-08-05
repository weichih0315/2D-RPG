using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion",menuName ="Items/Potion", order =1)]
public class HealthPotion : ItemSetting, IUseable
{
    [SerializeField]
    private int health;

    public override bool Use()
    {
        if (!Player.Instance.Dead && Player.Instance.Hp < Player.Instance.MaxHp)
        {
            base.Use();
            Player.Instance.AddHealth(health);
            return true;
        }
        return false;
    }
}
