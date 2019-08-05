using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour {
    
    [SerializeField]
    private List<ShopItem> shopItems = new List<ShopItem>();
    public List<ShopItem> ShopItems
    {
        get
        {
            return shopItems;
        }

        set
        {
            shopItems = value;
        }
    }

    private Coroutine coroutine;

    public void OpenShop()
    {
        Shop.Instance.Show(this);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(StopInteractHandle());
    }

    IEnumerator StopInteractHandle()
    {
        while (Player.Instance.IsCanInteract(transform.position))
        {
            yield return null;
        }
        HideShop();
    }

    public void HideShop()
    {
        Shop.Instance.Hide();
    }
}

[System.Serializable]
public class ShopItem
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
    private int price;
    public int Price
    {
        get
        {
            return price;
        }

        set
        {
            price = value;
        }
    }   

    [SerializeField]
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

    public ShopItem(Item newItem, int newPrice, int newCount)
    {
        Item = newItem;
        Price = newPrice;
        Count = newCount;
    }
}
