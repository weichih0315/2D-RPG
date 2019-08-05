using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour {

    private static CraftingUI instance;
    public static CraftingUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CraftingUI>();
            }

            return instance;
        }
    }
        
    [SerializeField]
    private Text title,description, materialTitle,count;

    [SerializeField]
    private CraftSlotUI craftSlotPrefab;

    [SerializeField]
    private Transform  info,itemParents, materialParents;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Recipe selectedRecipe;

    private int craftCount = 1;
    public int CraftCount
    {
        get
        {
            return craftCount;
        }

        set
        {
            craftCount = value;
            count.text = craftCount + "";
            UpdateDescription();
        }
    }

    private List<CraftSlotUI> craftSlotUIs = new List<CraftSlotUI>();
    
    private void Start()
    {
        Inventory.Instance.itemCountChangedEvent += ItemCountChanged;
    }

    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            selectedRecipe.Deselect();
        }

        selectedRecipe = recipe;
        selectedRecipe.Select();

        UpdateDescription();
    }

    private void UpdateDescription()
    {
        if (selectedRecipe == null)
            return;

        title.text = selectedRecipe.CraftingItem.Item.Name;
        description.text = selectedRecipe.Description;
        materialTitle.text = "Materials : ";

        foreach (CraftSlotUI craftingMaterial in craftSlotUIs)
        {
            Destroy(craftingMaterial.gameObject);
        }
        craftSlotUIs.Clear();

        CraftSlotUI craftingItem = Instantiate(craftSlotPrefab, itemParents);
        craftingItem.UpdateUI(selectedRecipe.CraftingItem.Item, selectedRecipe.CraftingItem.Count * CraftCount, false);
        craftSlotUIs.Add(craftingItem);
        foreach (CraftingMaterial craftingMaterial in selectedRecipe.CraftingMaterials)
        {
            CraftSlotUI craftSlotMaterial = Instantiate(craftSlotPrefab, materialParents);
            craftSlotMaterial.UpdateUI(craftingMaterial.Item, craftingMaterial.Count * CraftCount, true);
            craftSlotUIs.Add(craftSlotMaterial);
        }

        StartCoroutine(LastOneFrameRebuildLayout());
    }

    private IEnumerator LastOneFrameRebuildLayout()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)info);
    }

    public void IncreaseCount()
    {
        if (CraftCount < 999)
            CraftCount = CraftCount + 1;
    }

    public void ReduceCount()
    {
        if (CraftCount > 1)
            CraftCount = CraftCount - 1;
    }

    public void Craft()
    {
        if (IsCanCraft() && !Player.Instance.IsAction)
            StartCoroutine(Player.Instance.StartCraft(selectedRecipe, CraftCount));
        else
            Debug.Log("Need more Materials");
    }

    public bool IsCanCraft()
    {        
        foreach (CraftingMaterial material in selectedRecipe.CraftingMaterials)
        {
            int count = Inventory.Instance.GetItemCount(material.Item.Name);

            if (count < material.Count * CraftCount)
            {
                return false;
            }
        }

        return true;
    }

    private void ItemCountChanged(Item item)
    {
        UpdateDescription();
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
