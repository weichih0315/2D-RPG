using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text count;

    [SerializeField]
    private Text name;
    
    [SerializeField]
    private Text price;

    [SerializeField]
    private ShopItem shopItem;
    public ShopItem ShopItem
    {
        get
        {
            return shopItem;
        }

        set
        {
            icon.sprite = value.Item.Icon;
            count.text = value.Count > 1? value.Count + "" : "";
            name.text = value.Item.Name;
            price.text = "$ : " + value.Price;
            shopItem = value;
        }
    }

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Buy()
    {
        if (GameManager.Instance.money - ShopItem.Price >= 0)
        {
            Item tempShopItem = Instantiate(ShopItem.Item);
            tempShopItem.Count = ShopItem.Count;

            if (Inventory.Instance.IsCanAddItem(tempShopItem))
            {
                Inventory.Instance.StackItem(tempShopItem);     
                GameManager.Instance.money -= ShopItem.Price;
                Shop.Instance.Money = GameManager.Instance.money;
                Debug.Log("Add " + tempShopItem.Name + " to bag");
            }
            else
            {
                Debug.Log("Inventory is full");
                Destroy(tempShopItem);
            }
        }
        else
        {
            Debug.Log("need more money");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (shopItem != null)
            TooltipUI.Instance.ShowItemTooltip(rectTransform, shopItem.Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideItemTooltip();
    }
}
