using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QGQuestButton : MonoBehaviour {

    [SerializeField]
    private Text title;
    public Text Title
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

    private Quest quest;
    public Quest Quest
    {
        get
        {
            return quest;
        }

        set
        {
            quest = value;
        }
    }
    
    public void Select()
    {
        QuestGiverWindow.Instance.ShowQuestDescription(Quest);
    }

    public void UpdateTitle()
    {
        Title.text = "[" + Quest.Level + "] " + Quest.Title + "<color=#ffbb04> <size=10>!</size></color>";
        Color c = Title.color;
        c.a = 1f;
        Title.color = c;

        if (Questlog.Instance.HasQuest(quest) && quest.IsComplete)  //完成任務
        {
            Title.text = "[" + Quest.Level + "] " + Quest.Title + "<color=#ffbb04> <size=10>?</size></color>";
        }
        else if (Questlog.Instance.HasQuest(quest))     //未完成任務
        {
            c.a = 0.5f;
            Title.color = c;
            Title.text = "[" + Quest.Level + "] " + Quest.Title + "<color=#c0c0c0ff> <size=10>?</size></color>";
        }
    }
}
