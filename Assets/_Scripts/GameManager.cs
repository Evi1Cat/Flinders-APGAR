using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeSpeedModifier = 1;
    public BabySettings babySettings = new BabySettings();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

[Serializable]
public class BabySettings
{
    [SerializeField] public float[] APGAR_Check_Times;
    [SerializeField][Range(0, 100)] public int healthChance = 60;
}


