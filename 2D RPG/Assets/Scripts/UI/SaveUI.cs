using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour {

    private static SaveUI instance;
    public static SaveUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveUI>();
            }

            return instance;
        }
    }

    [SerializeField]
    private SaveButton[] saveButtons;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        foreach (SaveButton saveButton in saveButtons)
        {
            saveButton.Show();
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
