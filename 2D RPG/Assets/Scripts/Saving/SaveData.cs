using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    [SerializeField]
    private List<GameData> gameDatas;    
    public List<GameData> GameDatas
    {
        get
        {
            return gameDatas;
        }
    }

    [SerializeField]
    private OptionData optionData;
    public OptionData OptionData
    {
        get
        {
            return optionData;
        }
    }
    
    public void Save()
    {
        for (int i = 0; i < gameDatas.Count; i++)
        {            
            gameDatas[i].Save();
        }

        optionData.Save();
    }

    public void Load()
    {
        for (int i = 0; i < gameDatas.Count; i++)
        {
            gameDatas[i].Index = i;
            gameDatas[i].Load();
        }

        optionData.Load();
    }
}

[Serializable]
public class GameData
{
    [SerializeField]
    private PlayerData playerData;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }

    [SerializeField]
    private InventoryData inventoryData;
    public InventoryData InventoryData
    {
        get
        {
            return inventoryData;
        }
    }

    [SerializeField]
    private CharacterPanelData characterPanelData;
    public CharacterPanelData CharacterPanelData
    {
        get
        {
            return characterPanelData;
        }
    }

    [SerializeField]
    private ActionBarData actionBarData;
    public ActionBarData ActionBarData
    {
        get
        {
            return actionBarData;
        }
    }

    [SerializeField]
    private QuestLogData questLogData;
    public QuestLogData QuestLogData
    {
        get
        {
            return questLogData;
        }
    }

    [SerializeField]
    private List<QuestGiverData> questGiverDatas;
    public List<QuestGiverData> QuestGiverDatas
    {
        get
        {
            return questGiverDatas;
        }

        set
        {
            questGiverDatas = value;
        }
    }

    [SerializeField]
    private long dateTime;
    public long DateTime
    {
        get
        {
            return dateTime;
        }

        set
        {
            dateTime = value;
        }
    }

    [SerializeField]
    private int index;
    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    [SerializeField]
    private bool isEmpty = true;
    public bool IsEmpty
    {
        get
        {
            return isEmpty;
        }

        set
        {
            isEmpty = value;
        }
    }

    public void Save()
    {
        string filePath = Application.persistentDataPath + "/GameData" + index + ".dat";
        string saveString = JsonUtility.ToJson(this);
        byte[] serializedData = Encoding.UTF8.GetBytes(saveString);
        File.WriteAllBytes(filePath, serializedData);        
    }

    public bool Load()
    {
        string filePath = Application.persistentDataPath + "/GameData" + index + ".dat";
        byte[] serializedData;        
        string jsonData = "";

        try
        {
            serializedData = File.ReadAllBytes(filePath);
            jsonData = Encoding.UTF8.GetString(serializedData);
            SaveDataManager.Instance.SaveData.GameDatas[index] = JsonUtility.FromJson<GameData>(jsonData);
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("can't find file : " + filePath);
            return false;
        }
        return true;
    }

    public void Delete()
    {
        File.Delete(Application.persistentDataPath + "/GameData" + index + ".dat");
    }
}

[Serializable]
public class PlayerData
{
    [SerializeField]
    private int level;
    public int Level
    {
        get
        {
            return level;
        }
    }    

    [SerializeField]
    private float hp;
    public float Hp
    {
        get
        {
            return hp;
        }
    }

    [SerializeField]
    private float maxHp;
    public float MaxHp
    {
        get
        {
            return maxHp;
        }
    }
        
    [SerializeField]
    private float mp;
    public float Mp
    {
        get
        {
            return mp;
        }
    }

    [SerializeField]
    private float maxMp;
    public float MaxMp
    {
        get
        {
            return maxMp;
        }
    }
        
    [SerializeField]
    private float exp;
    public float Exp
    {
        get
        {
            return exp;
        }
    }
    
    [SerializeField]
    private float maxExp;
    public float MaxExp
    {
        get
        {
            return maxExp;
        }
    }

    [SerializeField]
    private Vector2 position;
    public Vector2 Position
    {
        get
        {
            return position;
        }
    }

    public void Set(Player player)
    {
        level = player.Level;
        hp = player.Hp;
        maxHp = player.MaxHp;
        mp = player.Mp;
        maxMp = player.MaxMp;
        exp = player.Exp;
        maxExp = player.MaxExp;
        position = player.transform.position;
    }
}

[Serializable]
public class InventoryData
{
    [SerializeField]
    private List<BagData> bagDatas;
    public List<BagData> BagDatas
    {
        get
        {
            return bagDatas;
        }
    }

    public void SetBagDatas(List<BagData> bagDatas)
    {
        this.bagDatas = bagDatas;
    }
}

[Serializable]
public class BagData
{    
    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
    }

    [SerializeField]
    private int index;
    public int Index
    {
        get
        {
            return index;
        }
    }

    [SerializeField]
    private List<ItemData> itemDatas;
    public List<ItemData> ItemDatas
    {
        get
        {
            return itemDatas;
        }
    }

    public BagData(string name, int index, List<ItemData> itemDatas)
    {
        this.name = name;
        this.index = index;
        this.itemDatas = itemDatas;
    }
}

[Serializable]
public class ItemData
{
    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
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
    }

    [SerializeField]
    private int index;
    public int Index
    {
        get
        {
            return index;
        }
    }

    [SerializeField]
    private BagData bagData;
    public BagData BagData
    {
        get
        {
            return bagData;
        }
    }

    public void Set(string name, int count, int index)
    {
        this.name = name;
        this.count = count;
        this.index = index;
    }

    public void SetBagData(BagData bagData)
    {
        this.bagData = bagData;
    }
}

[Serializable]
public class CharacterPanelData
{
    [SerializeField]
    private List<string> equipNames;
    public List<string> EquipNames
    {
        get
        {
            return equipNames;
        }
    }

    public void SetEquipNames(List<string> equipNames)
    {
        this.equipNames = equipNames;
    }
}

[Serializable]
public class ActionBarData
{
    [SerializeField]
    private List<ItemData> itemDatas;
    public List<ItemData> ItemDatas
    {
        get
        {
            return itemDatas;
        }
    }

    public void SetItemDatas(List<ItemData> itemDatas)
    {
        this.itemDatas = itemDatas;
    }
}

[Serializable]
public class QuestLogData
{
    [SerializeField]
    private List<QuestData> questDatas;
    public List<QuestData> QuestDatas
    {
        get
        {
            return questDatas;
        }

        set
        {
            questDatas = value;
        }
    }
}

[Serializable]
public class QuestData
{
    [SerializeField]
    private string questGiverName;
    public string QuestGiverName
    {
        get
        {
            return questGiverName;
        }

        set
        {
            questGiverName = value;
        }
    }

    [SerializeField]
    private string title;
    public string Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    [SerializeField]
    private CollectObjective[] collectObjectives;
    public CollectObjective[] CollectObjectives
    {
        get
        {
            return collectObjectives;
        }

        set
        {
            collectObjectives = value;
        }
    }

    [SerializeField]
    private KillObjective[] killObjectives;
    public KillObjective[] KillObjectives
    {
        get
        {
            return killObjectives;
        }

        set
        {
            killObjectives = value;
        }
    }

}

[Serializable]
public class QuestGiverData
{   
    [SerializeField]
    private string questGiverName;
    public string QuestGiverName
    {
        get
        {
            return questGiverName;
        }

        set
        {
            questGiverName = value;
        }
    }

    [SerializeField]
    private List<string> completedQuestNames = new List<string>();
    public List<string> CompletedQuestNames
    {
        get
        {
            return completedQuestNames;
        }

        set
        {
            completedQuestNames = value;
        }
    }
}

[Serializable]
public class OptionData
{
    [SerializeField]
    private AudioSetting audioSetting;
    public AudioSetting AudioSetting
    {
        get
        {
            return audioSetting;
        }

        set
        {
            audioSetting = value;
        }
    }

    public OptionData()
    {
        audioSetting = new AudioSetting();
    }

    public void Save()
    {
        audioSetting.Save();
    }

    public void Load()
    {
        audioSetting.Load();
    }
}

[Serializable]
public class AudioSetting
{
    [SerializeField]
    private float masterVolumePercent;
    public float MasterVolumePercent
    {
        get
        {
            return masterVolumePercent;
        }

        set
        {
            masterVolumePercent = value;
        }
    }

    [SerializeField]
    private float musicVolumePercent;
    public float MusicVolumePercent
    {
        get
        {
            return musicVolumePercent;
        }

        set
        {
            musicVolumePercent = value;
        }
    }
    
    [SerializeField]
    private float sfxVolumePercent;
    public float SfxVolumePercent
    {
        get
        {
            return sfxVolumePercent;
        }

        set
        {
            sfxVolumePercent = value;
        }
    }

    public void Save()
    {
        string filePath = Application.persistentDataPath + "/AudioSetting.dat";
        string saveString = JsonUtility.ToJson(this);
        byte[] serializedData = Encoding.UTF8.GetBytes(saveString);
        File.WriteAllBytes(filePath, serializedData);
    }

    public bool Load()
    {
        string filePath = Application.persistentDataPath + "/AudioSetting.dat";
        byte[] serializedData;
        string jsonData = "";

        try
        {
            serializedData = File.ReadAllBytes(filePath);
            jsonData = Encoding.UTF8.GetString(serializedData);
            SaveDataManager.Instance.SaveData.OptionData.AudioSetting = JsonUtility.FromJson<AudioSetting>(jsonData);
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("can't find file : " + filePath);
            return false;
        }

        return true;
    }

    public void Delete()
    {
        File.Delete(Application.persistentDataPath + "/AudioSetting.dat");
    }

    public void Set(float masterVolumePercent, float musicVolumePercent, float sfxVolumePercent)
    {
        this.masterVolumePercent = masterVolumePercent;
        this.musicVolumePercent = musicVolumePercent;
        this.sfxVolumePercent = sfxVolumePercent;
    }
}