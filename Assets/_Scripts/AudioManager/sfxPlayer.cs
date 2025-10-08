using UnityEngine;
using UnityEngine.Audio;

public class sfxPlayer : MonoBehaviour
{
    public bool isUI;
    [SerializeField] AudioMixerGroup ui, gamesfx;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isUI)
        {
            GetComponent<AudioSource>().outputAudioMixerGroup = ui;
        }
        else
        {
            GetComponent<AudioSource>().outputAudioMixerGroup = gamesfx;
        }
        if (!GetComponent<AudioSource>().isPlaying)
        {
            gameObject.SetActive(false);
            isUI = false;
        }
    }
}
