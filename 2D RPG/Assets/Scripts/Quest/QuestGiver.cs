using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {

    [SerializeField]
    private SpriteRenderer statusRenderer,minimapIndicator;

    [SerializeField]
    private Sprite question, questionSilver, exclamation;

    [SerializeField]
    private Sprite minimapQuestion, minimapQuestionSilver, minimapExclamation;

    [SerializeField]
    private List<Quest> quests = new List<Quest>();
    public List<Quest> Quests
    {
        get
        {
            return quests;
        }

        set
        {
            quests = value;
        }
    }

    [SerializeField]
    private List<Quest> completedQuests = new List<Quest>();
    public List<Quest> CompltedQuests
    {
        get
        {
            return completedQuests;
        }

        set
        {
            completedQuests = value;
        }
    }

    private Coroutine coroutine;

    protected override void Start()
    {
        base.Start();

        foreach (Quest quest in quests)
        {
            quest.QuestGiver = this;
        }
    }

    public Quest GetQuest(string questName)
    {
        foreach (Quest quest in quests)
        {
            if (quest.Title == questName)
            {
                return quest;
            }
        }

        return null;
    }

    public void CompleteQuest(Quest quest)
    {
        completedQuests.Add(quest);
        quests.Remove(quest);
        UpdateQuestStatus();
        if (QuestGiverWindow.Instance.IsOpen)
            QuestGiverWindow.Instance.UpdateButton();
    }

    public void UpdateQuestStatus()
    {
        if (quests.Count == 0)
        {
            statusRenderer.sprite = null;
            minimapIndicator.sprite = null;
            return;
        }

        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && Questlog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    minimapIndicator.sprite = minimapQuestion;
                    break;
                }
                else if (!Questlog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    minimapIndicator.sprite = minimapExclamation;
                    break;
                }
                else if (!quest.IsComplete && Questlog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    minimapIndicator.sprite = minimapQuestionSilver;
                }
            }
        }
    }

    public void OpenQuestWindow()
    {
        QuestGiverWindow.Instance.ShowQuests(this);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(StopInteractHandle());
    }

    IEnumerator StopInteractHandle()
    {
        while (Player.Instance.IsCanInteract(transform.position))
        {
            yield return null;
        }
        HideShop();
    }

    public void HideShop()
    {
        Shop.Instance.Hide();
    }
}
