using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class Crib : MonoBehaviour
{
    public bool nurseOnTheWay = false;
    [SerializeField] Sprite empty, full;
    [SerializeField] Opencrib openCrib;
    private int checkIndex = 0;
    public float babyTimer = 0f;
    private SpriteRenderer cribSprite;
    private Baby baby = null;
    private List<CheckTime> APGAR_Check_Times = new List<CheckTime>();
    void Start()
    {
        cribSprite = gameObject.GetComponent<SpriteRenderer>();
        cribSprite.sprite = empty;

        foreach (float x in GameManager.instance.babySettings.APGAR_Check_Times)
        {
            APGAR_Check_Times.Add(new CheckTime(x));
        }
    }

    void Update()
    {
        if (baby != null)
        {
            babyTimer += Time.deltaTime * GameManager.instance.timeSpeedModifier;
            if (checkIndex < APGAR_Check_Times.Count)
            {
                if (APGAR_Check_Times[checkIndex].checkTime + (30*GameManager.instance.timeSpeedModifier)< babyTimer)
                {
                    int urgentCareInt = 3;
                    if (checkIndex == APGAR_Check_Times.Count - 1)
                    {
                        urgentCareInt = 6;
                    }
                    if (baby.CheckAPGAR() <= urgentCareInt)
                    {
                        ReleaseBaby(1);
                    }
                    APGAR_Check_Times[checkIndex].checkedBySystem = true;
                    checkIndex++;
                }
                else if (((checkIndex > 0 && ((APGAR_Check_Times[checkIndex].checkTime - APGAR_Check_Times[checkIndex - 1].checkTime) / 2) + APGAR_Check_Times[checkIndex - 1].checkTime < babyTimer)
                || (checkIndex == 0 && APGAR_Check_Times[checkIndex].checkTime / 2 < babyTimer))
                && !APGAR_Check_Times[checkIndex].updatedVitals)
                {
                    //add code to change baby's vitals
                    //Debug.Log("Vitals updated");
                    APGAR_Check_Times[checkIndex].updatedVitals = true;
                    Debug.Log("Check baby " + gameObject.name);
                }
            }
        }
    }

    public void OpenCrib()
    {
        openCrib.Open(baby, this);
    }

    public void NewBaby()
    {
        float x = 0;
        babyTimer = 0;
        checkIndex = 0;
        switch (OTT())
        {
            case 2:
                x = Random.Range(100f, 160f);
                break;
            case 1:
                x = Random.Range(1f, 99f);
                break;
        }
        baby = new Baby(OTT(), x, OTT(), OTT(), OTT());

        //Debug.Log(baby);
        cribSprite.sprite = full;
    }

    public bool Occupied()
    {
        bool x = false;
        if (baby != null)
        {
            x = true;
        }
        return x;
    }

    public void ReleaseBaby(int x)
    {
        //Logic for a baby getting taken away
        if (x == 1)//If baby was deemed healthy
        {
            if (baby.CheckAPGAR() > 6 && APGAR_Check_Times[1].checkTime < babyTimer) // if the baby has an APGAR of 7+ after the first two checks
            {
                GameManager.instance.Increase();
            }
            else
            {
                GameManager.instance.Decrease();
            }
        }
        if(x == -1)//If baby was deemed unhealthy 
        {
            if ((baby.CheckAPGAR() <= 6 && APGAR_Check_Times[4].checkTime < babyTimer) || baby.CheckAPGAR() <= 3) // if the baby has an apgar of 6 or less after all the tests, or if the baby has an apgar of 3 or below
            {
                GameManager.instance.Increase();
            }
            else
            {
                GameManager.instance.Decrease();
            }
        }
        baby = null;
        cribSprite.sprite = empty;
    }

    /// <summary>
    /// One to three
    /// </summary>
    /// <returns>Returns a random number from zero to two favouring higher numbers</returns>
    private int OTT()
    {
        bool loop = true;
        int outNum = 2;
        while (loop)
        {
            if (Random.Range(0, 101) > GameManager.instance.babySettings.healthChance && outNum > 0)
            {
                outNum -= 1;
            }
            else
            {
                loop = false;
            }
        }
        return outNum;
    }
}

public class CheckTime
{
    public float checkTime = 0f;
    public bool checkedBySystem = false, updatedVitals = false;

    public CheckTime(float x)
    {
        checkTime = x;
    }
}
