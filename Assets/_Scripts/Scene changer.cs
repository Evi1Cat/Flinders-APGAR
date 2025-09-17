using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechanger : MonoBehaviour
{
    [SerializeField] private String sceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
