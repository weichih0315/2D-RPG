using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

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
    private Text stackSize;
    public Text StackText
    {
        get
        {
            return stackSize;
        }
    }
       
    private Slot slot;
    public Slot Slot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
            UpdateUI();
        }
    }

    public Item Item
    {
        get
        {
            return Slot.Item;
        }
        set
        {
            Slot.Item = value;
        }
    }

    public int Count
    {
        get
        {
            return Slot.Count;
        }
        set
        {
            Slot.Count = value;
        }
    }

    public bool IsEmpty { get { return Item == null; } }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandPickup.Instance.IsEmpty && !IsEmpty)
            {
                Slot.GiveItemHandPickup();
            }
            else if (!HandPickup.Instance.IsEmpty)
            {
                if (IsEmpty) //Put
                {
                    Item = HandPickup.Instance.Item;                    
                    HandPickup.Instance.Item = null;                                                        
                }
                else
                {
                    if (Item.Name == HandPickup.Instance.Item.Name && !Item.IsFull)     //Stack
                    {
                        HandPickup.Instance.Item = Slot.StackItem(HandPickup.Instance.Item);
                    }
                    else                                                                //Swap
                    {
                        Slot.SwapItemHandPickup();                        
                    }
                }                
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right && !IsEmpty)
        {
            Slot.UseItem();
        }
    }
        
    public void UpdateUI()
    {
        int Count = Item == null? 0 : Item.Count;
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

    private RectTransform rectTransform;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
            TooltipUI.Instance.ShowItemTooltip(rectTransform,Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideItemTooltip();
    }
}
