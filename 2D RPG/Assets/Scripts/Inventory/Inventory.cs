using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
            }

            return instance;
        }
    }

    public GameObject bagUIPrefab,bagItemPrefab,slotItmePrefab,itemPrefab;

    private List<Bag> bags = new List<Bag>();
    public List<Bag> Bags
    {
        get
        {
            return bags;
        }
    }

    [SerializeField]
    private BagButton[] bagButtons;
    public BagButton[] BagButtons
    {
        get
        {
            return bagButtons;
        }
    }

    public event System.Action<Item> itemCountChangedEvent;

    public bool CanAddBag
    {
        get { return bags.Count < bagButtons.Length; }
    }

    //物品欄使用  不指定位置
    public bool AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                BagUI bagUI = Instantiate(bagUIPrefab, transform).GetComponent<BagUI>();
                bag.BagUI = bagUI;
                bag.BagUI.Bag = bag;
                bag.BagUI.AddSlots();

                bagButton.Bag = bag;
                bags.Add(bag);
                SortBag();
                bag.transform.SetParent(transform.parent);

                OnBagChanged(bag);
                return true;
            }
        }
        return false;
    }

    //指定位置
    public void AddBag(Bag bag, BagButton bagButton)
    {
        BagSetting bagSetting = (BagSetting)bag.ItemSetting;
        BagUI bagUI = Instantiate(bagUIPrefab, transform).GetComponent<BagUI>();
        bag.BagUI = bagUI;
        bag.BagUI.Bag = bag;
        bag.BagUI.AddSlots();

        bagButton.Bag = bag;
        bags.Add(bag);
        SortBag();
        bag.transform.SetParent(transform.parent);

        OnBagChanged(bag);
    }

    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.BagUI.gameObject);
        SortBag();

        OnBagChanged(bag);
    }

    public void SortBag()
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag != null)
            {
                bagButton.Bag.BagUI.transform.SetParent(null);
                bagButton.Bag.BagUI.transform.SetParent(transform);
            }
        }
    }

    public bool AddItem(Item item)
    {
        if (item is Bag && AddBag(item as Bag))
        {
            return true;
        }

        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag != null && bagButton.Bag.AddItem(item))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsCanAddItem(Item item)
    {
        int emptyCount = 0;

        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag != null)
            {
                foreach (Slot slot in bagButton.Bag.Slots)
                {
                    if (!slot.IsEmpty)
                    {
                        if (item.Name == slot.Item.Name)
                            emptyCount += (slot.Item.StackSize - slot.Item.Count);
                        if (emptyCount >= item.Count)       //確認是否有足夠空間存放
                            return true;
                    }           
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Item StackItem(Item item)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag != null)
            {
                foreach (Slot slot in bagButton.Bag.Slots)
                {
                    if (!slot.IsEmpty && slot.Item.Name == item.Name && !slot.IsFull)
                    {
                        item = slot.StackItem(item);
                        if (item.Count == 0)
                            return null;
                    }
                }
            }
        }

        if (AddItem(item))
            return null;
        else
            return item;
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;

        foreach (Bag bag in bags)
        {
            foreach (Slot slot in bag.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Name == type)
                {
                    itemCount += slot.Item.Count;
                }
            }
        }

        return itemCount;

    }

    public bool RemoveItem(string name, int count)
    {
        foreach (Bag bag in bags)
        {
            foreach (Slot slot in bag.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Name == name)
                {
                    int tempCount = slot.Item.Count;

                    for (int i = 0; i < tempCount; i++)
                    {
                        count--;
                        slot.Count--;
                        if (count == 0)
                            return true;
                    }                    
                }
            }
        }
        return false;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

    public void OnBagChanged(Bag bag)
    {
        foreach (Slot slot in bag.Slots)
        {
            if(!slot.IsEmpty && itemCountChangedEvent != null)
                itemCountChangedEvent.Invoke(slot.Item);
        }
    }
}
