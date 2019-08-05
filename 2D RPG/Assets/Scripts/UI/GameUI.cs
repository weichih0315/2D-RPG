using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [SerializeField]
    private MenuUI menuUI;

    [SerializeField]
    private CharacterPanelUI characterPanelUI;

    [SerializeField]
    private Questlog questLogUI;

    [SerializeField]
    private CraftingUI craftingUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeybindManager.Instance.Keybind["Act1"]))
        {
            KeybindManager.Instance.ClickActionButton("Act1");
        }

        if (Input.GetKeyDown(KeybindManager.Instance.Keybind["Act2"]))
        {
            KeybindManager.Instance.ClickActionButton("Act2");
        }

        if (Input.GetKeyDown(KeybindManager.Instance.Keybind["Act3"]))
        {
            KeybindManager.Instance.ClickActionButton("Act3");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Time.timeScale = Time.timeScale > 0 ? 0 : 1;
            menuUI.OpenClose();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            characterPanelUI.OpenClose();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            questLogUI.OpenClose();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            craftingUI.OpenClose();
        }
    }
}
