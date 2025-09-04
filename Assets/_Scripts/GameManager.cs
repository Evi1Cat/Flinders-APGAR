using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public readonly float timeSpeedModifier = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
