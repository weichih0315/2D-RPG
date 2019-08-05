using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    private static Shop instance;
    public static Shop Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Shop>();
            }

            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private ShopButton shopButtonPrefab;

    [SerializeField]
    private Transform area;

    [SerializeField]
    private Text moneyText;
    public int Money
    {
        set
        {
            moneyText.text = "Money : " + value.ToString();
        }
    }

    private List<ShopButton> shopButtons = new List<ShopButton>();

    private void Awake()
    {
        Shop.Instance.Money = GameManager.Instance.money;
    }

    public void RemoveAllShopButton()
    {
        foreach (ShopButton shopButton in shopButtons)
        {
            Destroy(shopButton.gameObject);
        }

        shopButtons.Clear();
    }

    public void Show(Seller shopTable)
    {
        RemoveAllShopButton();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        foreach (ShopItem shopItem in shopTable.ShopItems)
        {
            ShopButton shopButton = Instantiate(shopButtonPrefab, area);
            shopButton.ShopItem = shopItem;
            shopButtons.Add(shopButton);
        }
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
