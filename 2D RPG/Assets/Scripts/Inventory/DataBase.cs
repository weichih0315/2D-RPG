using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "DataBase", order = 1)]
public class DataBase : ScriptableObject
{
    public ItemSetting[] itemSettings;

    public ItemSetting GetItem(string itemName)
    {
        foreach (ItemSetting itemSetting in itemSettings)
        {
            if (itemSetting != null && itemSetting.Name == itemName) return itemSetting;
        }
        return null;
    }

}