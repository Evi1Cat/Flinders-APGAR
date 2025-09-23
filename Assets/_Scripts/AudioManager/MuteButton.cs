using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] Image line;
    private void Start()
    {
        line.gameObject.SetActive(AudioManager.Instance.IsMuted());
    }
    public void ToggleMute()
    {
        if(line.gameObject.activeSelf)
        {
            Toggle(false);
        }
        else
        {
            Toggle(true);
        }
    }
    private void Toggle(bool x)
    {
        line.gameObject.SetActive(x);
        AudioManager.Instance.MuteMaster(x);
    }
}
