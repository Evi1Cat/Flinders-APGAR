using System;
using Pixelplacement;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeSpeedModifier = 1;
    public BabySettings babySettings = new BabySettings();
    public delegate void PointChange();
    public PointChange Increase, Decrease;
    public int points = 0;
    [SerializeField] CanvasGroup gameWinScreen;
    [SerializeField] TweenVars winScreenFadeVars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        Increase += GainPoints;
        Decrease += LosePoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GainPoints()
    {
        points++;
        if (points >= 10)
        {
            //Add win script here
            GetComponent<BabyInputStream>().activeGame = false;
            Tween.Value(gameWinScreen.alpha, 1f, SetWinScreenAlpha, winScreenFadeVars.duration, winScreenFadeVars.delay, winScreenFadeVars.easeCurve);
        }
    }
    private void SetWinScreenAlpha(float x)
    {
        gameWinScreen.alpha = x;
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
    [SerializeField] public float lastCHeckGraceTime = 10f;
    [SerializeField][Range(0, 100)] public int initialGoodHealthChance = 60, healthChangeChance = 60, goodHealthChangeChance = 50, symptomChangeChance = 35;
    [SerializeField][Range(0, 10)] public float preferredBabiesOnScreen = 4.5f;
}



