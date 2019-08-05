using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour {

    private static Questlog instance;
    public static Questlog Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Questlog>();
            }
            return instance;
        }
    }

    [SerializeField]
    private QuestButton questButtonPrefab;

    [SerializeField]
    private Transform questArea;

    [SerializeField]
    private Text questDescription;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountText;

    [SerializeField]
    private int maxCount;

    private List<QuestButton> questButtons = new List<QuestButton>();
    public List<QuestButton> QuestButtons
    {
        get
        {
            return questButtons;
        }

        set
        {
            questButtons = value;
        }
    }

    private Quest selectedQuest;

    private void Start()
    {
        questCountText.text = questButtons.Count + "/" + maxCount;
    }

    public void AcceptQuest(Quest quest)
    {
        QuestButton questButton = Instantiate(questButtonPrefab, questArea);
        questButton.Quest = quest;
        questButton.Quest.QuestButton = questButton;
        questButtons.Add(questButton);

        foreach (CollectObjective collectObjective in quest.CollectObjectives)
        {
            Inventory.Instance.itemCountChangedEvent += collectObjective.UpdateItemCount;
            collectObjective.UpdateItemCount();
        }

        foreach (KillObjective killObjective in quest.KillObjectives)
        {
            GameManager.Instance.EnemyOnDeadEvent += killObjective.UpdateKillCount;
        }

        questCountText.text = questButtons.Count + "/" + maxCount;
        CheckComplete();
    }   
    
    public void ShowDescription(Quest quest)
    {
        foreach (QuestButton questButton in questButtons)
        {
            if (questButton.Quest != quest)
                questButton.DeSelect();
        }

        selectedQuest = quest;

        UpdateSelectedDescripts();
    }

    public void UpdateUI()
    {
        if (selectedQuest != null)
        {
            UpdateSelectedDescripts();
        }

        CheckComplete();
    }

    public void CheckComplete()
    {
        foreach (QuestButton questButton in questButtons)
        {
            questButton.Quest.QuestGiver.UpdateQuestStatus();
            questButton.UpdateTitle();
        }
    }

    public void UpdateSelectedDescripts()
    {
        string objectives = string.Empty;

        foreach (Objective obj in selectedQuest.CollectObjectives)
        {
            objectives += obj.Name + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
        }

        foreach (Objective obj in selectedQuest.KillObjectives)
        {
            objectives += obj.Name + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
        }

        questDescription.text = string.Format("{0}\n" +
            "<size=10>{1}</size>\n" +
            "\nObjectives\n<size=10>{2}</size>",
            selectedQuest.Title,
            selectedQuest.Description,
            objectives);
    }

    public void RemoveQuest(Quest quest)
    {
        if (selectedQuest == quest)
        {
            selectedQuest = null;
            questDescription.text = string.Empty;
        }            

        questButtons.Remove(quest.QuestButton);
        Destroy(quest.QuestButton.gameObject);

        questCountText.text = questButtons.Count + "/" + maxCount;
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AbandonQuest()
    {
        if (selectedQuest == null)
            return;

        foreach (CollectObjective collectObjective in selectedQuest.CollectObjectives)
        {
            Inventory.Instance.itemCountChangedEvent -= collectObjective.UpdateItemCount;
        }

        foreach (KillObjective collectObjective in selectedQuest.KillObjectives)
        {
            GameManager.Instance.EnemyOnDeadEvent -= collectObjective.UpdateKillCount;
        }

        QuestGiver questGiver = selectedQuest.QuestGiver;
        RemoveQuest(selectedQuest);
        questGiver.UpdateQuestStatus();
        QuestGiverWindow.Instance.UpdateUI();
    }

    public bool HasQuest(Quest quest)
    {
        foreach (QuestButton questButton in questButtons)
        {
            if (questButton.Quest.Title == quest.Title)
            {
                return true;
            }
        }

        return false;
    }
}
