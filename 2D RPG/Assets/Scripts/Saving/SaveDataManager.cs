using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour {

    public static SaveDataManager Instance;

    [SerializeField]
    private DataBase[] dataBases;

    [SerializeField]
    private SaveData saveData;
    public SaveData SaveData
    {
        get
        {
            return saveData;
        }
    }
    
    private int loadIndex;
    public int LoadIndex
    {
        get
        {
            return loadIndex;
        }

        set
        {
            loadIndex = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        saveData.Load();
    }

    public void Save(int index)
    {
        SavePlayer(index);
        SaveInventory(index);
        SaveCharacterPanel(index);
        SaveActionBar(index);
        SaveQuestLog(index);
        SaveQuestGiver(index);
        saveData.GameDatas[index].DateTime = DateTime.Now.ToFileTimeUtc();
        saveData.GameDatas[index].IsEmpty = false;

        saveData.GameDatas[index].Save();
    }

    public void SavePlayer(int index)
    {
        Player player = Player.Instance;
        saveData.GameDatas[index].PlayerData.Set(player);
    }

    public void SaveInventory(int index)
    {
        Inventory inventory = Inventory.Instance;
        List<BagData> bagDatas = new List<BagData>();

        for (int i = 0; i < inventory.BagButtons.Length; i++)
        {
            if (inventory.BagButtons[i].Bag != null)
            {
                Bag bag = inventory.BagButtons[i].Bag;
                BagData bagData = new BagData(bag.Name, i, new List<ItemData>());

                bagDatas.Add(GetBagDataFromBag(bag, bagData));
            }
        }

        saveData.GameDatas[index].InventoryData.SetBagDatas(bagDatas);
    }

    

    public void SaveCharacterPanel(int index)
    {
        CharacterPanelUI characterPanel = CharacterPanelUI.Instance;
        List<string> equipNames = new List<string>();

        foreach (CharacterPanelButton characterPanelButton in characterPanel.CharacterPanelButtons)
        {
            if (characterPanelButton.EquipItem != null)
            {
                string equipName = characterPanelButton.EquipItem.Name;
                equipNames.Add(equipName);
            }
        }

        saveData.GameDatas[index].CharacterPanelData.SetEquipNames(equipNames);
    }

    public void SaveActionBar(int index)
    {
        ActionButton[] actionButtons = ActionBarUI.Instance.ActionButtons;
        List<ItemData> itemDatas = new List<ItemData>();

        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].Item != null)
            {
                ItemData itemData = new ItemData();
                itemData.Set(actionButtons[i].Item.Name, actionButtons[i].Item.Count, i);

                itemDatas.Add(itemData);
            }
        }

        saveData.GameDatas[index].ActionBarData.SetItemDatas(itemDatas);
    }

    public void SaveQuestLog(int index)
    {
        List<QuestButton> questButtons = Questlog.Instance.QuestButtons;
        List<QuestData> questDatas = new List<QuestData>();

        for (int i = 0; i < questButtons.Count; i++)
        {
            QuestData questData = new QuestData();
            questData.QuestGiverName = questButtons[i].Quest.QuestGiver.Name;
            questData.Title = questButtons[i].Quest.Title;
            questData.CollectObjectives = questButtons[i].Quest.CollectObjectives;
            questData.KillObjectives = questButtons[i].Quest.KillObjectives;

            questDatas.Add(questData);
        }

        saveData.GameDatas[index].QuestLogData.QuestDatas = questDatas;
    }

    public void SaveQuestGiver(int index)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        List<QuestGiverData> questGiverDatas = new List<QuestGiverData>();

        foreach (QuestGiver questGiver in questGivers)
        {
            QuestGiverData questGiverData = new QuestGiverData();
            questGiverData.QuestGiverName = questGiver.Name;
            for (int i = 0; i < questGiver.CompltedQuests.Count; i++)
            {
                questGiverData.CompletedQuestNames.Add(questGiver.CompltedQuests[i].Title);
            }
            questGiverDatas.Add(questGiverData);
        }

        saveData.GameDatas[index].QuestGiverDatas = questGiverDatas;
    }

    public void Load()
    {
        Load(loadIndex);
    }

    public void Load(int index)
    {
        if (!saveData.GameDatas[index].Load())
            return;
        LoadPlayer(index);
        LoadInventory(index);
        LoadCharacterPanel(index);
        LoadActionBar(index);
        LoadQuestLog(index);
        LoadQuestGiver(index);
    }

    public void LoadPlayer(int index)
    {
        Player player = Player.Instance;
        PlayerData playerData = saveData.GameDatas[index].PlayerData;

        player.Level = playerData.Level;
        player.Hp = playerData.Hp;
        player.MaxHp = playerData.MaxHp;
        player.Mp = playerData.Mp;
        player.MaxMp = playerData.MaxMp;
        player.Exp = playerData.Exp;
        player.MaxMp = playerData.MaxMp;
        player.transform.position = playerData.Position;
    }

    public void LoadInventory(int index)
    {
        Inventory inventory =  Inventory.Instance;
        InventoryData inventoryData = saveData.GameDatas[index].InventoryData;

        foreach (BagData bagData in inventoryData.BagDatas)
        {
            Bag bag = Instantiate(inventory.bagItemPrefab, inventory.transform).GetComponent<Bag>();
            bag.ItemSetting = GetItemFromDataBase(bagData.Name);

            bag = GetBagFromBagData(bag, bagData);
            inventory.AddBag(bag, inventory.BagButtons[bagData.Index]);
        }
    }

    public void LoadCharacterPanel(int index)
    {
        CharacterPanelUI characterPanelUI = CharacterPanelUI.Instance;
        CharacterPanelData characterPanelData = saveData.GameDatas[index].CharacterPanelData;

        foreach (string equipName in characterPanelData.EquipNames)
        {
            Item item = Instantiate(Inventory.Instance.itemPrefab).GetComponent<Item>();
            item.ItemSetting = GetItemFromDataBase(equipName);
            characterPanelUI.SwapItem(item);
        }
    }

    public void LoadActionBar(int index)
    {
        ActionBarUI actionBarUI = ActionBarUI.Instance;
        ActionBarData actionBarData = saveData.GameDatas[index].ActionBarData;

        foreach (ItemData itemData in actionBarData.ItemDatas)
        {
            Item item = Instantiate(Inventory.Instance.itemPrefab).GetComponent<Item>();
            item.ItemSetting = GetItemFromDataBase(itemData.Name);
            item.Count = itemData.Count;
            actionBarUI.ActionButtons[itemData.Index].Item = item;
        }
    }

    public void LoadQuestLog(int index)
    {
        Questlog questlog = Questlog.Instance;
        List<QuestData> questDatas = saveData.GameDatas[index].QuestLogData.QuestDatas;
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestData questData in questDatas)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.Name == questData.QuestGiverName);

            foreach (Quest quest in questGiver.Quests)
            {
                if (quest.Title == questData.Title)
                {
                    quest.QuestGiver = questGiver;
                    quest.KillObjectives = questData.KillObjectives;
                    Questlog.Instance.AcceptQuest(quest);
                }
            }            
        }
    }

    public void LoadQuestGiver(int index)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        List<QuestGiverData> questGiverDatas = saveData.GameDatas[index].QuestGiverDatas;

        foreach (QuestGiverData questGiverData in questGiverDatas)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.Name == questGiverData.QuestGiverName);

            foreach (string completedQuestName in questGiverData.CompletedQuestNames)
            {
                questGiver.CompleteQuest(questGiver.GetQuest(completedQuestName));
            }
        }
    }

    public BagData GetBagDataFromBag(Bag bag, BagData bagData)
    {
        for (int i = 0; i < bag.Slots.Count; i++)
        {
            if (!bag.Slots[i].IsEmpty)
            {
                Item item = bag.Slots[i].Item;
                ItemData itemData = new ItemData();

                itemData.Set(item.Name, item.Count, i);         //名子數量位置

                if (item is Bag)
                {
                    Bag itemBag = item as Bag;
                    itemData.SetBagData(new BagData(itemBag.Name, 0, new List<ItemData>()));

                    BagData itemBagData = GetBagDataFromBag(itemBag, itemData.BagData);
                }

                bagData.ItemDatas.Add(itemData);
            }
        }

        return bagData;
    }

    public Bag GetBagFromBagData(Bag bag,BagData bagData)
    {
        Inventory inventory = Inventory.Instance;
        BagSetting bagSetting = bag.ItemSetting as BagSetting;

        for (int i = 0; i < bagSetting.SlotCount; i++)
        {
            Slot slot = Instantiate(inventory.slotItmePrefab, bag.transform).GetComponent<Slot>();
            bag.Slots.Add(slot);
        }

        foreach (ItemData itemData in bagData.ItemDatas)
        {
            ItemSetting itemSetting = GetItemFromDataBase(itemData.Name);

            if (itemSetting is BagSetting)
            {
                Bag itemBag = Instantiate(inventory.bagItemPrefab).GetComponent<Bag>();
                itemBag.ItemSetting = GetItemFromDataBase(itemData.BagData.Name);
                itemBag = GetBagFromBagData(itemBag, itemData.BagData);
                bag.Slots[itemData.Index].Item = itemBag;
                itemBag.transform.SetParent(bag.Slots[itemData.Index].transform);
            }
            else
            {
                Item item = Instantiate(inventory.itemPrefab).GetComponent<Item>();
                item.ItemSetting = itemSetting;
                item.Count = itemData.Count;
                bag.Slots[itemData.Index].Item = item;
                item.transform.SetParent(bag.Slots[itemData.Index].transform);
            }
        }

        return bag;
    }

    public ItemSetting GetItemFromDataBase(string name)
    {
        foreach (DataBase dataBase in dataBases)
        {
            ItemSetting itemSetting = dataBase.GetItem(name);
            if (itemSetting != null)
                return itemSetting;
        }

        return null;
    }
}
