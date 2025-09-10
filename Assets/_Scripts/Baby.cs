using System;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public class Baby
{
    //Baby health concerns
    private String skinColour = "";
    [Range(0, 2)] private int skinBlue = 1;
    [Range(0, 2)] private int agitation = 1;
    [Range(60, 200)] private float heartRate = 130;
    [Range(0, 2)] private int muscleTone = 1;
    [Range(0, 2)] private int respiration = 1;
    //Baby health distractors

    public Baby(int A, float P, int G, int M, int R)
    {
        skinBlue = A;
        heartRate = P;
        agitation = G;
        muscleTone = M;
        respiration = R;
    }

    public int CheckAPGAR()
    {
        int score = 0;
        switch (heartRate)
        {
            case > 100:
                score += 2;
                break;
            case > 0:
                score += 1;
                break;
        }

        score += skinBlue + agitation + muscleTone + respiration;
        return score;
    }

    public int Check_Apgar()
    {
        return skinBlue;
    }
    public float Check_aPgar()
    {
        return heartRate;
    }
    public int Check_apGar()
    {
        return agitation;
    }
    public int Check_apgAr()
    {
        return muscleTone;
    }
    public int Check_apgaR()
    {
        return respiration;
    }

    public void UpdateStats(int A, float P, int G, int M, int R)
    {
        skinBlue = A;
        heartRate = P;
        agitation = G;
        muscleTone = M;
        respiration = R;
    }
}
