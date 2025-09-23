using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public void PlayClip(string x)
    {
        AudioManager.Instance.PlaySoundEffect(x);
    }
    public void ButtonClickSound()
    {
        AudioManager.Instance.PlaySoundEffect("Click " + Random.Range(1, 8));
    }
}
