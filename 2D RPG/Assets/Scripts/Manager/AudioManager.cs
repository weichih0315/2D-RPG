using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, Sfx, Music };

    public float MasterVolumePercent { get; private set; }
    public float SfxVolumePercent { get; private set; }
    public float MusicVolumePercent { get; private set; }

    private AudioSource sfx2DSource;
    private AudioSource[] musicSources;
    private int activeMusicSourceIndex;    

    private Transform audioListener;
    private SoundLibrary library;

    public static AudioManager Instance;

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

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                musicSources[i].loop = true;
                newMusicSource.transform.parent = transform;
            }
            GameObject newSfx2Dsource = new GameObject("2D sfx source");
            sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
            newSfx2Dsource.transform.parent = transform;

            audioListener = FindObjectOfType<AudioListener>().transform;
        }
    }

    private void Start()
    {
        AudioSetting audioSetting = SaveDataManager.Instance.SaveData.OptionData.AudioSetting;
        MasterVolumePercent = audioSetting.MasterVolumePercent;
        MusicVolumePercent = audioSetting.MusicVolumePercent;
        SfxVolumePercent = audioSetting.SfxVolumePercent;
    }

    private void Update()
    {
        /*if (FindObjectOfType<Player>() != null)
        {
            audioListener.position = FindObjectOfType<Player>().transform.position;
        }*/
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        AudioSetting audioSetting = SaveDataManager.Instance.SaveData.OptionData.AudioSetting;

        if (channel == AudioChannel.Master)
        {
            MasterVolumePercent = volumePercent;
            audioSetting.MasterVolumePercent = MasterVolumePercent;
        }
        else if (channel == AudioChannel.Music)
        {
            MusicVolumePercent = volumePercent;
            audioSetting.MusicVolumePercent = MusicVolumePercent;
        }
        else if (channel == AudioChannel.Sfx)
        {
            SfxVolumePercent = volumePercent;
            audioSetting.SfxVolumePercent = SfxVolumePercent;
        }

        musicSources[0].volume = MusicVolumePercent * MasterVolumePercent;
        musicSources[1].volume = MusicVolumePercent * MasterVolumePercent;        

        audioSetting.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void Play()
    {
        for (int index = 0; index < musicSources.Length; index++)
            musicSources[index].Play();
    }

    public void Pause()
    {
        for(int index = 0; index < musicSources.Length; index++)
            musicSources[index].Pause();
    }

    public void Stop()
    {
        for (int index = 0; index < musicSources.Length; index++)
            musicSources[index].Stop();
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, SfxVolumePercent * MasterVolumePercent);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(library.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), SfxVolumePercent * MasterVolumePercent);
    }

    public void PlaySound2D(AudioClip audioClip)
    {
        sfx2DSource.PlayOneShot(audioClip, SfxVolumePercent * MasterVolumePercent);
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, MusicVolumePercent * MasterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(MusicVolumePercent * MasterVolumePercent, 0, percent);
            yield return null;
        }
        musicSources[1 - activeMusicSourceIndex].clip = null;
    }

    private void OnEnable()
    {
        //GameManager.OnGameOverStatic += GameOver;
    }

    private void OnDisable()
    {
        //GameManager.OnGameOverStatic -= GameOver;
    }

    private void GameOver()
    {
        Stop();
    }
}
