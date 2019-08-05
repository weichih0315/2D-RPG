using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour {

    [SerializeField]
    private List<LootItem> lootItemsSetting = new List<LootItem>();

    private List<Item> lootItems = new List<Item>();
    public List<Item> LootItems
    {
        get
        {
            return lootItems;
        }

        set
        {
            lootItems = value;
        }
    }

    public bool IsEmpty { get { return lootItems.Count == 0; } }

    private void Awake()
    {
        for (int i = 0; i < lootItemsSetting.Count; i++)
        {
            if (Random.Range(0, 100) <= lootItemsSetting[i].DropChance)
            {
                Item item = Instantiate(lootItemsSetting[i].Item, transform);
                item.Count = Mathf.Clamp(lootItemsSetting[i].Count, 1, item.StackSize);
                lootItems.Add(item);
            }
        }
    }

    public void ShowLootWindow()
    {
        LootWindowUI.Instance.Show(this);
    }

    public void HideLootWindow()
    {
        LootWindowUI.Instance.Hide();
    }
}

[System.Serializable]
public class LootItem
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
            item = value;
        }
    }

    [SerializeField]
    [Range(0, 100)]
    private float dropChance;
    public float DropChance
    {
        get
        {
            return dropChance;
        }

        set
        {
            dropChance = value;
        }
    }    

    [SerializeField]
    [Range(0, 100)]
    private int count;
    public int Count
    {
        get
        {
            return count;
        }

        set
        {
            count = value;
        }
    }
}