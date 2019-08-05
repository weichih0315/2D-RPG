using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionBarUI : MonoBehaviour {

    private static ActionBarUI instance;
    public static ActionBarUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ActionBarUI>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;
    public ActionButton[] ActionButtons
    {
        get
        {
            return actionButtons;
        }

        set
        {
            actionButtons = value;
        }
    }

}
