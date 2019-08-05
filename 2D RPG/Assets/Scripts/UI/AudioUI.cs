using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour {

    [SerializeField]
    private Slider[] volumeSliders;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        AudioSetting audioSetting = SaveDataManager.Instance.SaveData.OptionData.AudioSetting;
        volumeSliders[0].value = audioSetting.MasterVolumePercent;
        volumeSliders[1].value = audioSetting.MusicVolumePercent;
        volumeSliders[2].value = audioSetting.SfxVolumePercent;
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
