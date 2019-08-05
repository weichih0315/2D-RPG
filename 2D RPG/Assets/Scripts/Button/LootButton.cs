using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image icon;
    public Image Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    [SerializeField]
    private Text description;
    public Text Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    [SerializeField]
    private Text stackText;
    public Text StackText
    {
        get
        {
            return stackText;
        }
    }

    [SerializeField]
    private Item item;
    public Item Item
    {
        get
        {
            return item;
        }

        set
        {
            if (value != null)
            {
                Icon.sprite = value.Icon;
                Description.text = value.Description;

                if (value.Count > 1)
                    StackText.text = value.Count.ToString();
            }

            item = value;
        }
    }

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnClick()
    {
        if (Item.StackSize > 1)
        {
            Item tempItem = Inventory.Instance.StackItem(Item);
            if (tempItem == null)
            {
                LootWindowUI.Instance.RemoveLootItem(this);
            }
            else
            {
                Item = tempItem;
            }
        }
        else if (Inventory.Instance.AddItem(Item))
        {
            LootWindowUI.Instance.RemoveLootItem(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
            TooltipUI.Instance.ShowItemTooltip(rectTransform, Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideItemTooltip();
    }
}
