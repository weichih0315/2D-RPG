using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillItemSetting", menuName = "Items/Skill", order = 1)]
public class SkillSetting : ItemSetting, IUseable
{

    [SerializeField]
    private Skill skillPrefab;
    public Skill SkillPrefab
    {
        get
        {
            return skillPrefab;
        }

        set
        {
            skillPrefab = value;
        }
    }

    public override Sprite Icon
    {
        get
        {
            return skillPrefab.Icon;
        }
    }

    public override string Name
    {
        get
        {
            return skillPrefab.Name;
        }

        set
        {
            base.Name = value;
        }
    }

    public override Quality Quality
    {
        get
        {
            return skillPrefab.Quality;
        }

        set
        {
            base.Quality = value;
        }
    }

    public override int StackSize
    {
        get
        {
            return 1;
        }
    }

    public override string Description
    {
        get
        {
            return skillPrefab.Description;
        }
    }

    public override bool Use()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.StartAttack(SkillPrefab);
        return base.Use();
    }
}
