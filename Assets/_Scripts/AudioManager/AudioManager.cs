using Pixelplacement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    [SerializeField] int sfxPoolSize = 20;
    [SerializeField] GameObject sfxObject;
    [SerializeField] TweenVars fadeSettings;
    [SerializeField] AudioSource BGMusic;
    [SerializeField] AudioSetting[] soundEffectList, backgroundMusicList;
    private List<AudioSource> sfxPool = new List<AudioSource>();
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);

        for(int i = 0; i < sfxPoolSize; i++)
        {
            AddToPool();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseMusic()
    {
        foreach (AudioSetting x in backgroundMusicList)
        {
            if(x.clip == BGMusic.clip)
            {
                x.stoppedAtPosition = BGMusic.time;
            }
        }
        FadeOut();
    }
    public void ResumeMusic()
    {
        foreach (AudioSetting x in backgroundMusicList)
        {
            if (x.clip == BGMusic.clip)
            {
                BGMusic.time = x.stoppedAtPosition;
            }
        }
        FadeIn();
    }
    public void ChangeTrack(AudioClip newClip)
    {
        FadeOut();
        while(BGMusic.isPlaying)
        {

        }
        BGMusic.clip = newClip;
        foreach (AudioSetting x in backgroundMusicList)
        {
            if (x.clip == BGMusic.clip)
            {
                x.stoppedAtPosition = 0;
            }
        }
        FadeIn();
    }
    public void ChangeTrack(string trackName)
    {
        foreach (AudioSetting x in backgroundMusicList)
        {
            if (x.clipName == trackName)
            {
                ChangeTrack(x.clip);
            }
        }
    }
    public void PlaySoundEffect(AudioClip newSFX)
    {
        for(int i = 0; i < sfxPool.Count; i++)
        {
            if (!sfxPool[i].gameObject.activeSelf)
            {
                sfxPool[i].clip = newSFX;
                for(int y = 0; y < soundEffectList.Length; y++)
                {
                    if(newSFX == soundEffectList[y].clip)
                    {
                        sfxPool[i].volume = soundEffectList[y].normalizedVolume;
                    }
                }
                sfxPool[i].gameObject.SetActive(true);
                sfxPool[i].Play();
                Debug.Log("Playing " + newSFX.name + " on " + sfxPool[i].gameObject.name);
                return;
            }
        }
        AddToPool();
        PlaySoundEffect(newSFX);
    }
    public void PlaySoundEffect(string newSFX)
    {
        foreach (AudioSetting x in soundEffectList)
        {
            if (x.clipName == newSFX)
            {
                PlaySoundEffect(x.clip);
            }
        }
    }
    private void FadeOut()
    {
        Tween.Value(BGMusic.volume, 0f, SetVolume, fadeSettings.duration, 0f, fadeSettings.easeCurve, completeCallback: BGMusic.Pause);
    }
    private void FadeIn()
    {
        float targetVol = 0f;
        foreach (AudioSetting x in backgroundMusicList)
        {
            if (x.clip == BGMusic.clip)
            {
                targetVol = x.normalizedVolume;
            }
        }
        BGMusic.Play();
        Tween.Value(0f, targetVol, SetVolume, fadeSettings.duration, 0f, fadeSettings.easeCurve);
    }
    private void SetVolume(float x)
    {
        BGMusic.volume = x;
    }
    private void AddToPool()
    {
        GameObject x = Instantiate(sfxObject);
        x.transform.parent = transform;
        sfxPool.Add(x.GetComponent<AudioSource>());
        x.SetActive(false);
    }
}

[Serializable]
public class AudioSetting 
{
    [SerializeField] public string clipName;
    [SerializeField][Range(0f,1f)] public float normalizedVolume;
    [SerializeField] public float stoppedAtPosition;
    [SerializeField] public AudioClip clip;
}
