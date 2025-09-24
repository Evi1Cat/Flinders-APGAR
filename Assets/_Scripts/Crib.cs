using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crib : Nursepathnode
{
    public bool nurseOnTheWay = false;
    public delegate void TimerTick(int time);
    public TimerTick timerTick;
    [SerializeField] Sprite empty, full;
    [SerializeField] Opencrib openCrib;
    private int checkIndex = 0, healthy = -1;
    public float babyTimer = 0f;
    private SpriteRenderer cribSprite;
    private Baby baby = null;
    private List<CheckTime> APGAR_Check_Times = new List<CheckTime>();
    void Start()
    {
        cribSprite = gameObject.GetComponent<SpriteRenderer>();
        cribSprite.sprite = empty;
        timerTick = TickUpdate;

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
            timerTick((int)babyTimer);
            if (checkIndex < APGAR_Check_Times.Count) // if there are more APGAR checks to do
            {
                if (APGAR_Check_Times[checkIndex].checkTime + (GameManager.instance.babySettings.lastCHeckGraceTime * GameManager.instance.timeSpeedModifier) < babyTimer) //if the current check time has passed
                {
                    if (BabyNeedsUrgentCare())
                    {
                        healthy = 1;
                        Nursemanager.instance.SendNurse(this, false);
                        Debug.Log("Baby was remove due to needing urgent care");
                    }
                    APGAR_Check_Times[checkIndex].checkedBySystem = true;
                    checkIndex++;
                }
                else if (((checkIndex > 0 && ((APGAR_Check_Times[checkIndex].checkTime - APGAR_Check_Times[checkIndex - 1].checkTime) / 2) + APGAR_Check_Times[checkIndex - 1].checkTime < babyTimer)
                || (checkIndex == 0 && APGAR_Check_Times[checkIndex].checkTime / 2 < babyTimer))
                && !APGAR_Check_Times[checkIndex].updatedVitals) //if its half way to the next check time and it hasnt already been updated, then update the babys vitals
                {
                    UpdateHealth();
                    //Debug.Log("Vitals updated");
                    APGAR_Check_Times[checkIndex].updatedVitals = true;
                    //Debug.Log("Check baby " + gameObject.name);
                }
            }
            else if (APGAR_Check_Times[^1].checkTime + (30 * GameManager.instance.timeSpeedModifier) < babyTimer)
            {
                //add code for baby to be taken away
                if (BabyNeedsUrgentCare())
                {
                    healthy = 1;
                    Nursemanager.instance.SendNurse(this, false);
                }
                else
                {
                    Nursemanager.instance.SendNurse(this, false);
                }
            }
        }
    }

    void TickUpdate(int x)
    {

    }

    public void OpenCrib()
    {
        openCrib.Open(baby, this);
    }
    public void CloseCrib()
    {
        openCrib.Close();
    }

    public void NewBaby()
    {
        float x = 0;
        babyTimer = 0;
        checkIndex = 0;
        healthy = -1;
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

    public void ReleaseBaby()
    {
        //Logic for a baby getting taken away
        if (healthy == 1)//If baby was deemed healthy
        {
            if (APGAR_Check_Times[1].checkTime < babyTimer && !BabyNeedsUrgentCare()) // if the baby has an APGAR of 7+ after the first two checks
            {
                GameManager.instance.Increase();
            }
            else
            {
                GameManager.instance.Decrease();
            }
        }
        if (healthy == -1)//If baby was deemed unhealthy 
        {
            if (BabyNeedsUrgentCare()) // if the baby has an apgar of 6 or less after all the tests, or if the baby has an apgar of 3 or below
            {
                GameManager.instance.Increase();
            }
            else
            {
                GameManager.instance.Decrease();
            }
        }
        openCrib.Close();
        baby = null;
        cribSprite.sprite = empty;
    }

    public void SetHealthy()
    {
        healthy = 1;
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
            if (Random.Range(0, 101) > GameManager.instance.babySettings.initialGoodHealthChance && outNum > 0)
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
    
    public void UpdateHealth()
    {
        if (Random.Range(0f, 100f) < GameManager.instance.babySettings.healthChangeChance)
        {
            int change = -1;
            if (Random.Range(0f, 100f) < GameManager.instance.babySettings.goodHealthChangeChance)//Baby gets healthier
            {
                change = 1;
            }

            int[] x = new int[5];
            x[0] = baby.Check_Apgar();
            switch (baby.Check_aPgar())
            {
                case > 100:
                    x[1] = 2;
                    break;
                case > 0:
                    x[1] = 1;
                    break;
                case 0:
                    x[1] = 0;
                    break;
            }
            x[2] = baby.Check_apGar();
            x[3] = baby.Check_apgAr();
            x[4] = baby.Check_apgaR();

            for (int i = 0; i < x.Length; i++)
            {
                bool loop = true;
                while (loop)
                {
                    if (Random.Range(0f, 100f) < GameManager.instance.babySettings.symptomChangeChance && x[i]+change <= 2 && x[i]+change >= 0)
                    {
                        x[i] += change;
                    }
                    else
                    {
                        loop = false;
                    }
                }
            }

            baby.UpdateStats(x[0], HeartRateConversion(x[1]), x[2], x[3], x[4]);
        }
    }
    private float HeartRateConversion(int x)
    {
        float hr = 0f;
        switch (x)
        {
            case 2:
                hr = Random.Range(100f, 160f);
                break;
            case 1:
                hr = Random.Range(1f, 99f);
                break;
        }
        return hr;
    }

    private bool BabyNeedsUrgentCare()
    {
        if((baby.CheckAPGAR() <= 6 && APGAR_Check_Times[^1].checkTime < babyTimer) || baby.CheckAPGAR() <= 3 || baby.Check_aPgar() <= 50 || baby.Check_apgaR() == 0)
        {
            return true;
        }
        /*if (baby.CheckAPGAR() <= 6 && APGAR_Check_Times[^1].checkTime < babyTimer)
        {
            Debug.Log("baby.CheckAPGAR() <= 6 && APGAR_Check_Times[APGAR_Check_Times.Count].checkTime < babyTimer");
            return true;
        }
        if (baby.CheckAPGAR() <= 3)
        {
            Debug.Log("baby.CheckAPGAR() <= 3");
            return true;
        }
        if (baby.Check_apgaR() <= 50)
        {
            Debug.Log("baby.Check_apgaR() <= 50");
            return true;
        }
        if (baby.Check_aPgar() == 0)
        {
            Debug.Log("baby.Check_aPgar() == 0");
            return true;
        }*/
        return false;
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


