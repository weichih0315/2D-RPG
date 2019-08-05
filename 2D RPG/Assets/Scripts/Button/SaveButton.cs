using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour {

    [SerializeField]
    private GameObject characterIcon;

    [SerializeField]
    private Text areaText;

    [SerializeField]
    public GameObject saveButton, loadButton;

    [SerializeField]
    private int index;

    private SaveData saveData;

    private void Start()
    {
        saveData = SaveDataManager.Instance.SaveData;
    }

    public void Save()
    {
        SaveDataManager.Instance.Save(index);
        Show();
    }

    public void Load()
    {
        SaveDataManager.Instance.LoadIndex = index;
        SceneManager.LoadScene("Game");
    }

    public void Show()
    {
        if (saveData.GameDatas[index].IsEmpty)
        {
            characterIcon.SetActive(false);
            areaText.text = "Empty";
            saveButton.SetActive(false);
            loadButton.SetActive(false);
        }
        else
        {
            DateTime dateTime = DateTime.FromFileTimeUtc(saveData.GameDatas[index].DateTime);
            characterIcon.SetActive(true);
            areaText.text = "Date: " + dateTime.ToString("dd/MM/yyy") + " - Time: " + dateTime.ToString("H:mm");
            saveButton.SetActive(true);
            loadButton.SetActive(true);
        }
    }
}
