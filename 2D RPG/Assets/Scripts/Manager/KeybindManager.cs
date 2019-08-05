using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance;
    public static KeybindManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private KeybindUI keybindUI;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    public Dictionary<string, KeyCode> Keybind { get; private set; }

    void Start()
    {
        Keybind = new Dictionary<string, KeyCode>();

        BindKey("Up", KeyCode.W);
        BindKey("Left", KeyCode.A);
        BindKey("Down", KeyCode.S);
        BindKey("Right", KeyCode.D);

        BindKey("Act1", KeyCode.Alpha1);
        BindKey("Act2", KeyCode.Alpha2);
        BindKey("Act3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)
    {        
        if (!Keybind.ContainsKey(key))
        {
            Keybind.Add(key, keyBind);
            keybindUI.UpdateKeyText(key, keyBind);
        }
        else if (Keybind.ContainsValue(keyBind)) //If we already have the keybind, then we need to change it to the new bind
        {
            string oldKey = Keybind.FirstOrDefault(x => x.Value == keyBind).Key;

            Keybind[oldKey] = Keybind[key];
            keybindUI.UpdateKeyText(oldKey, Keybind[key]);
        }

        Keybind[key] = keyBind;
        keybindUI.UpdateKeyText(key, keyBind);
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).OnClick();
    }
}