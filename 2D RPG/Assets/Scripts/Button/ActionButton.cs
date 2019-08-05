using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerDownHandler
{
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
                value.transform.SetParent(transform);
            item = value;
            UpdateActionButtonUI();
        }
    }

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
    private Text stackText;
    public Text StackText
    {
        get
        {
            return stackText;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return Item == null;
        }
    }

    public int Count
    {
        get
        {
            return Item.Count;
        }
        set
        {
            Item.Count = value;
            UpdateActionButtonUI();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandPickup.Instance.IsEmpty && !IsEmpty)
            {
                HandPickup.Instance.Item = Item;
                Item = null;
            }
            else if (!HandPickup.Instance.IsEmpty && 
                !(HandPickup.Instance.Item is Bag) && 
                !(HandPickup.Instance.Item.ItemSetting is EquipItemSetting) &&
                !(HandPickup.Instance.Item.ItemSetting is MaterialSetting))
            {
                if (IsEmpty)
                {
                    Item = HandPickup.Instance.Item;
                    HandPickup.Instance.Item = null;
                }
                else
                {
                    if (Item.Name == HandPickup.Instance.Item.Name && !Item.IsFull)     //Stack
                    {
                        StackItem();
                    }
                    else                                                                //Change
                    {
                        HandPickup.Instance.Item = SwapItem(HandPickup.Instance.Item);
                    }
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        if (IsEmpty)
            return;

        if (item.ItemSetting is SkillSetting)
        {
            item.ItemSetting.Use();
        }
        else
            Item.Use();

        UpdateActionButtonUI();
    }

    private void StackItem()
    {
        int count = HandPickup.Instance.Item.Count;

        for (int i = 0; i < count; i++)
        {
            Count++;
            HandPickup.Instance.Count--;
            if (Item.IsFull)
            {
                break;
            }
        }
    }

    private Item SwapItem(Item newItem)
    {
        Item tempItem = newItem;
        newItem = Item;
        Item = tempItem;
        return newItem;
    }

    public void UpdateActionButtonUI()
    {
        int Count = Item == null ? 0 : Item.Count;
        if (Count > 1)
        {
            Icon.sprite = Item.Icon;
            StackText.text = Count.ToString();
            StackText.color = Color.white;
            Icon.color = Color.white;
        }
        else if (Count == 1)
        {
            Icon.sprite = Item.Icon;
            StackText.color = Color.clear;
            Icon.color = Color.white;
        }
        else if (Count == 0)
        {
            StackText.color = Color.clear;
            Icon.color = Color.clear;
        }
    }
}
