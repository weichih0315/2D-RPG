using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandPickup : MonoBehaviour
{
    private static HandPickup instance;
    public static HandPickup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandPickup>();
            }

            return instance;
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
    private Text stackSize;
    public Text StackText
    {
        get
        {
            return stackSize;
        }
    }

    [SerializeField]
    private ItemPickup itemPickup;

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
            UpdateHandPickupUI();
        }
    }

    public ItemSetting ItemSetting
    {
        get
        {
            return item.ItemSetting;
        }

        set
        {
            item.ItemSetting = value;
            UpdateHandPickupUI();
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
            UpdateHandPickupUI();
        }
    }

    public bool IsEmpty { get { return Item == null; } }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !IsEmpty)
        {
            PutAtGround();
        }

        if (!IsEmpty)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void PutAtGround()
    {
        ItemPickup temp = Instantiate(itemPickup, Player.Instance.transform.position, Quaternion.identity);
        temp.Item = Item;
        Item = null;
    }

    public void UpdateHandPickupUI()
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
