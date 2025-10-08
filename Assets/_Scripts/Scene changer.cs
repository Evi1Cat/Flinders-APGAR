
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechanger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string songName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
        if (songName != null & songName == "Main")
        {
            AudioManager.Instance.ChangeTrack("GameMusic"+Random.Range(1,3));
        }
        else if (songName != null)
        {
            AudioManager.Instance.ChangeTrack(songName);
        }
    }
}
