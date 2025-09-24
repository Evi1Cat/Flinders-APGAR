using UnityEngine;
using UnityEngine.UI;

public class SliderVolumeChange : MonoBehaviour
{
    [SerializeField] string mixerTarget;
    public void ChangeVol()
    {
        AudioManager.Instance.ChangeMixerVol(mixerTarget, GetComponent<Slider>().value);
    }
}
