using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : MonoBehaviour
{
    private static QuestGiverWindow instance;
    public static QuestGiverWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }

            return instance;
        }
    }

    [SerializeField]
    private GameObject backBtn, acceptBtn, completeBtn, questDescription;

    [SerializeField]
    private QGQuestButton questButtonPrefab;

    [SerializeField]
    private Transform questArea;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private List<QGQuestButton> qGQuestButtons = new List<QGQuestButton>();

    private Quest selectedQuest;

    private QuestGiver selectedQuestGiver;

    public bool IsOpen { get { return canvasGroup.alpha == 1; } }
        
    public void RemoveAllButton()
    {
        foreach (QGQuestButton QGButton in qGQuestButtons)
        {
            Destroy(QGButton.gameObject);
        }

        qGQuestButtons.Clear();
    }
    
    public void ShowQuests(QuestGiver questGiver)
    {
        selectedQuestGiver = questGiver;

        UpdateButton();
        Open();
    }

    public void UpdateButton()
    {
        RemoveAllButton();

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in selectedQuestGiver.Quests)
        {
            QGQuestButton questButton = Instantiate(questButtonPrefab, questArea);
            questButton.Quest = quest;

            qGQuestButtons.Add(questButton);
        }

        UpdateButtonTitle();
    }

    public void ShowQuestDescription(Quest quest)
    {
        selectedQuest = quest;

        if (Questlog.Instance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!Questlog.Instance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
            completeBtn.SetActive(false);
        }
        else
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(false);
        }

        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        UpdateSelectedQuestDescription();
    }

    public void UpdateUI()
    {
        if (selectedQuest != null)
        {
            if (backBtn.activeSelf == true) //以返回鍵 判斷頁面
            {
                ShowQuestDescription(selectedQuest);
            }
            else
                UpdateSelectedQuestDescription();
        }

        UpdateButtonTitle();
    }

    public void UpdateButtonTitle()
    {
        foreach (QGQuestButton questButton in qGQuestButtons)
        {
            questButton.UpdateTitle();
        }
    }

    public void UpdateSelectedQuestDescription()
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

        questDescription.GetComponent<Text>().text = string.Format("{0}\n<size=10>{1}</size>\n\nObjectives\n<size=10>{2}</size>", selectedQuest.Title, selectedQuest.Description, objectives);
    }

    public void Accept()
    {
        Questlog.Instance.AcceptQuest(selectedQuest);
        Back();
    }

    public void Back()
    {
        ShowQuests(selectedQuestGiver);

        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        completeBtn.SetActive(false);
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            foreach (CollectObjective collectObjective in selectedQuest.CollectObjectives)
            {
                Inventory.Instance.itemCountChangedEvent -= collectObjective.UpdateItemCount;
                collectObjective.Complete();
            }

            foreach (KillObjective collectObjective in selectedQuest.KillObjectives)
            {
                GameManager.Instance.EnemyOnDeadEvent -= collectObjective.UpdateKillCount;
            }

            Player.Instance.AddExp(EXPManager.CalculateEXP(selectedQuest));

            selectedQuestGiver.CompleteQuest(selectedQuest);

            Questlog.Instance.RemoveQuest(selectedQuest);

            Back();
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        completeBtn.SetActive(false);
    }
}
