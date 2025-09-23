using Pixelplacement;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    [SerializeField] int sfxPoolSize = 20;
    [SerializeField] GameObject sfxObject;
    [SerializeField] TweenVars fadeSettings;
    [SerializeField] AudioSource BGMusic;
    [SerializeField] AudioSetting[] soundEffectList, backgroundMusicList;
    [SerializeField] AudioMixer masterMixer;
    private float masterVol;
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
    private void Start()
    {
        ChangeTrack("MenuMusic");
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
        if(BGMusic.clip != null)
        {
            FadeOut(newClip);
        }
        else
        {
            ChangeTrackIn(newClip);
        }
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
    public void MuteMaster(bool x)
    {
        if (!x)
        {
            ChangeMixerVol("masterVolume", masterVol);
        }
        else
        {
            masterMixer.SetFloat("masterVolume", -80f);
        }
    }
    public void ChangeMixerVol(string exposedName, float vol)
    {
        if(exposedName == "masterVolume")
        {
            masterMixer.SetFloat("masterVolume", masterVol);
            masterVol = vol;
        }
        else
        {
            masterMixer.SetFloat(exposedName, vol);
        }
    }
    public bool IsMuted()
    {
        float currentVol;
        masterMixer.GetFloat("masterVolume", out currentVol);
        if (currentVol == -80f)
        {
            return true;
        }
        return false;
    }
    private void FadeOut()
    {
        Tween.Value(BGMusic.volume, 0f, SetVolume, fadeSettings.duration, 0f, fadeSettings.easeCurve, completeCallback: BGMusic.Pause);
    }
    private void FadeOut(AudioClip newClip)
    {
        Tween.Value(BGMusic.volume, 0f, SetVolume, fadeSettings.duration, 0f, fadeSettings.easeCurve, completeCallback: ()=> ChangeTrackIn(newClip));
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
    private void ChangeTrackIn(AudioClip newClip)
    {
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
}

[Serializable]
public class AudioSetting 
{
    [SerializeField] public string clipName;
    [SerializeField][Range(0f,1f)] public float normalizedVolume;
    [SerializeField] public float stoppedAtPosition;
    [SerializeField] public AudioClip clip;
}
