using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour {

    [SerializeField]
    private AudioUI audioUI;

    [SerializeField]
    private KeybindUI keybindUI;

    [SerializeField]
    private SaveUI saveUI;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SelectTab(int index)
    {
        audioUI.Close();
        keybindUI.Close();
        saveUI.Close();

        if (index == 0)
        {
            audioUI.Open();
        }
        else if (index == 1)
        {
            keybindUI.Open();
        }
        else if (index == 2)
        {
            saveUI.Open();
        }
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
