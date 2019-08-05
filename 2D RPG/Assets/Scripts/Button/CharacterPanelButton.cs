using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterPanelButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private EquipmentType equipType;
    public EquipmentType EquipType
    {
        get
        {
            return equipType;
        }

        set
        {
            equipType = value;
        }
    }

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Item equipItem;
    public Item EquipItem
    {
        get
        {
            return equipItem;
        }

        set
        {
            if (value == null)
            {
                icon.sprite = null;
                icon.color = Color.clear;
                equipAnimator.Dequip();
            }
            else
            {
                icon.sprite = value.ItemSetting.Icon;
                icon.color = Color.white;
                equipAnimator.Equip((value.ItemSetting as EquipItemSetting).AnimationClips);
            }
            equipItem = value;
        }
    }

    [SerializeField]
    private EquipSocket equipAnimator;

    public bool IsEmpty { get { return EquipItem == null; } }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandPickup.Instance.IsEmpty && !IsEmpty)       //Take
            {
                HandPickup.Instance.Item = EquipItem;
                RemoveItem();
            }
            else if (!HandPickup.Instance.IsEmpty && 
                HandPickup.Instance.Item.ItemSetting is EquipItemSetting &&
                (HandPickup.Instance.Item.ItemSetting as EquipItemSetting).EquipType == equipType)
            {
                if (IsEmpty)
                {
                    PutItem(HandPickup.Instance.Item);
                    HandPickup.Instance.Item = null;
                }
                else
                {
                    HandPickup.Instance.Item = SwapItem(HandPickup.Instance.Item);
                    if (HandPickup.Instance.Item != null)
                        HandPickup.Instance.Item.transform.SetParent(HandPickup.Instance.transform);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Inventory.Instance.AddItem(EquipItem))
            {
                EquipItem = null;
            }
        }
    }

    public void PutItem(Item newItem)
    {
        EquipItem = newItem;
        EquipItem.transform.SetParent(transform);        
    }

    public Item SwapItem(Item newItem)
    {
        Item tempItem = newItem;
        newItem = EquipItem;
        EquipItem = tempItem;
        return newItem;
    }

    public void RemoveItem()
    {
        EquipItem = null;
    }

    RectTransform rectTransform;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EquipItem != null)
            TooltipUI.Instance.ShowItemTooltip(rectTransform, EquipItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideItemTooltip();
    }
}
