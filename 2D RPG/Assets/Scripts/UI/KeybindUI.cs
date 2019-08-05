using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindUI : MonoBehaviour {

    private CanvasGroup canvasGroup;

    private GameObject[] keybindButtons;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
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
    }
}
