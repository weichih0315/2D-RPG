using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;
    public string Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    [SerializeField]
    [TextArea]
    private string description;
    public string Description
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

    [SerializeField]
    private int level;
    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    [SerializeField]
    private int exp;
    public int Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    [SerializeField]
    private CollectObjective[] collectObjectives;
    public CollectObjective[] CollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }

    [SerializeField]
    private KillObjective[] killObjectives;
    public KillObjective[] KillObjectives
    {
        get
        {
            return killObjectives;
        }

        set
        {
            killObjectives = value;
        }
    }

    public QuestButton QuestButton { get; set; }
    public QuestGiver QuestGiver { get; set; }

    public bool IsComplete
    {
        get
        {

            foreach (Objective objective in collectObjectives)
            {
                if (!objective.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective objective in KillObjectives)
            {
                if (!objective.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;
    public int Amount
    {
        get
        {
            return amount;
        }
    }

    private int currentAmount;
    public int CurrentAmount
    {
        get
        {
            return currentAmount;
        }

        set
        {
            currentAmount = value;
        }
    }

    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
    }

    public bool IsComplete
    {
        get
        {
            return CurrentAmount >= Amount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (item != null && Name.ToLower() == item.Name.ToLower())
        {
            CurrentAmount = Inventory.Instance.GetItemCount(item.Name);

            if (CurrentAmount < Amount)
            {
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", item.Name, CurrentAmount, Amount));
            }

            Questlog.Instance.UpdateUI();
            QuestGiverWindow.Instance.UpdateUI();
        }
    }

    public void UpdateItemCount()
    {
        CurrentAmount = Inventory.Instance.GetItemCount(Name);

        Questlog.Instance.UpdateUI();
        QuestGiverWindow.Instance.UpdateUI();
    }

    public void Complete()
    {
        Inventory.Instance.RemoveItem(Name, Amount);
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Enemy enemy)
    {
        if (enemy != null && Name == enemy.Name)
        {
            CurrentAmount++;

            if (CurrentAmount < Amount)
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", enemy.Name, CurrentAmount, Amount));

            Questlog.Instance.UpdateUI();
            QuestGiverWindow.Instance.UpdateUI();
        }
    }
}