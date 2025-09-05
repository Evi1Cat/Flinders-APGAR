using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeSpeedModifier = 1;
    public BabySettings babySettings = new BabySettings();
    public delegate void PointChange();
    public PointChange Increase, Decrease;
    public int points = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        Increase += GainPoints;
        Decrease -= LosePoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GainPoints()
    {
        points++;
    }
    private void LosePoints()
    {
        points--;
    }
}

[Serializable]
public class BabySettings
{
    [SerializeField] public float[] APGAR_Check_Times;
    [SerializeField][Range(0, 100)] public int healthChance = 60;
}



