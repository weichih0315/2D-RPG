using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelUI : MonoBehaviour {

    private static CharacterPanelUI instance;
    public static CharacterPanelUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterPanelUI>();
            }

            return instance;
        }
    }

    [SerializeField]
    private CharacterPanelButton[] characterPanelButtons;
    public CharacterPanelButton[] CharacterPanelButtons
    {
        get
        {
            return characterPanelButtons;
        }

        set
        {
            characterPanelButtons = value;
        }
    }

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public Item SwapItem(Item newItem)
    {
        EquipItemSetting equipItemSetting = newItem.ItemSetting as EquipItemSetting;

        foreach (CharacterPanelButton characterPanelButton in characterPanelButtons)
        {
            if (equipItemSetting.EquipType == characterPanelButton.EquipType)
            {
                return characterPanelButton.SwapItem(newItem);
            }
        }

        return null;
    }
}
