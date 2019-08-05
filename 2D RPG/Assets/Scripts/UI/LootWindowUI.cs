using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootWindowUI : MonoBehaviour {

    private static LootWindowUI instance;
    public static LootWindowUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LootWindowUI>();
            }

            return instance;
        }
    }

    [SerializeField]
    private LootButton lootButtonPrefab;

    [SerializeField]
    private Transform area;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private LootTable currentLootTable;
    private List<LootButton> lootItems = new List<LootButton>();
    
    public void AddLootItem(Item item)
    {
        LootButton lootButton = Instantiate(lootButtonPrefab, area);
        lootButton.Item = item;
        lootItems.Add(lootButton);
    }

    public void RemoveLootItem(LootButton lootButton)
    {
        currentLootTable.LootItems.Remove(lootButton.Item);
        lootItems.Remove(lootButton);
        Destroy(lootButton.gameObject);
    }

    public void RemoveAllLootItemUI()
    {
        foreach (LootButton lootItem in lootItems)
        {
            Destroy(lootItem.gameObject);
        }

        lootItems.Clear();
    }

    public void TakeAllLootItem()
    {
        foreach (LootButton lootItem in lootItems.ToList())
        {
            lootItem.OnClick();  
        }
    }

    public void Show(LootTable lootTable)
    {
        currentLootTable = lootTable;
        RemoveAllLootItemUI();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        foreach (Item lootItem in lootTable.LootItems)
        {
            AddLootItem(lootItem);
        }
    }

    public void Hide()
    {
        currentLootTable = null;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
