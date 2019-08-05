using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    public SlotUI SlotUI { get; set; }

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
            item = value;
            if (SlotUI != null)
                SlotUI.UpdateUI();

            if (value != null)
            {
                value.transform.SetParent(transform);
                Inventory.Instance.OnItemCountChanged(Item);
            }               
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
            SlotUI.UpdateUI();
            Inventory.Instance.OnItemCountChanged(Item);
        }
    }

    public bool IsEmpty
    {
        get
        {
            return Item == null || Item.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || item.Count < Item.StackSize)
            {
                return false;
            }
            return true;
        }
    }

    public Item StackItem(Item item)
    {
        int startCount = item.Count;

        for (int i = 0; i < startCount; i++)
        {
            Count++;
            item.Count--;
            if (Item.IsFull)
            {
                break;
            }
        }
        return item;
    }

    public void UseItem()
    {
        if (Item is Bag)
        {
            if (Inventory.Instance.CanAddBag)
            {
                if (Inventory.Instance.AddBag(Item as Bag))
                {
                    Item = null;
                }
                else
                    Debug.Log("Add Bag is fail,maybe inventory is full");
            }
            else
                Debug.Log("BagButton is full");
        }
        else
        {
            ItemSetting itemSetting = Item.ItemSetting;
            if (itemSetting is IUseable)
            {
                if (itemSetting is EquipItemSetting)
                {
                    Item = CharacterPanelUI.Instance.SwapItem(Item);
                }
                else if (itemSetting is SkillSetting)
                {
                    itemSetting.Use();
                }
                else
                    Item.Use();
            }
            else
                Debug.Log(Item.ItemSetting.Name + " isn't useable");
        }
        SlotUI.UpdateUI();
        Inventory.Instance.OnItemCountChanged(Item);
    }

    public void GiveItemHandPickup()
    {
        HandPickup.Instance.Item = Item;
        Item = null;

        Inventory.Instance.OnItemCountChanged(HandPickup.Instance.Item);
    }

    public void SwapItemHandPickup()
    {
        Item tempItem = HandPickup.Instance.Item;
        GiveItemHandPickup();
        Item = tempItem;
    }
}
