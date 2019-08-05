using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour {

    [SerializeField]
    private Text title;

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

    private bool markedComplete = false;

    public void Select()
    {
        title.color = Color.red;
        Questlog.Instance.ShowDescription(Quest);
    }

    public void DeSelect()
    {
        title.color = Color.white;
    }

    public void UpdateTitle()
    {
        if (Quest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            title.text = "[" + Quest.Level + "] " + Quest.Title + "(C)";

            MessageFeedManager.Instance.WriteMessage(string.Format("{0} (C)", Quest.Title));
        }
        else if (!Quest.IsComplete)
        {
            markedComplete = false;
            title.text = "[" + Quest.Level + "] " + Quest.Title;
        }
    }
}
