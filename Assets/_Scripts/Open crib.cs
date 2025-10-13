using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class Opencrib : MonoBehaviour
{
    public static Opencrib Instance;
    [SerializeField] private TMP_Text pulse, irritability, muscleTone, respiratory, timerText;
    
    [SerializeField] private VideoPlayer pulsePlayer;
    [SerializeField] private Babycontroller baby;

    private Crib openedCrib = null;
    private Baby currentBaby = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy && openedCrib != null)
        {
            int mins = (int)Mathf.Floor(openedCrib.babyTimer / 60f);
            string seconds = (int)openedCrib.babyTimer - (mins * 60) + "";
            //Debug.Log(mins + ": " + seconds);
            if (int.Parse(seconds) < 10)
            {
                seconds = 0 + seconds;
            }
            timerText.text = mins + ":" + seconds;
        }
    }

    public void Open(Baby x, Crib refer)
    {
        if (x != null && !refer.nurseOnTheWay)
        {
            gameObject.SetActive(true);
            openedCrib = refer;
            currentBaby = x;
            openedCrib.timerTick += UpdateText;
            Babycontroller.Instance.CancellGrimaceTween();
        }
    }

    public void Close()
    {
        if (openedCrib != null)
        {
            //Debug.Log("test");
            openedCrib.timerTick -= UpdateText;
        }
        baby.StopBreathing();
        openedCrib = null;
        currentBaby = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Tells the crib to remove the baby
    /// </summary>
    /// <param name="x"> 1 for a healthy baby and -1 for an urgent care baby</param>
    public void RemoveBaby(int x)
    {
        if (x == 1)
        {
            openedCrib.SetHealthy();
        }
        Nursemanager.instance.SendNurse(openedCrib, false);
        Close();
    }
    public Crib GetCurrentlyOpenCrib()
    {
        return openedCrib;
    }
    public void SetBabyHue(Color x)
    {
        currentBaby.setHue = x;
    }
    public Color GetBabyHue()
    {
        return currentBaby.setHue;
    }
    public int GetbabyGrimace()
    {
        return currentBaby.Check_apGar();
    }

    private void UpdateText(int timeInSeconds, bool alreadyOpen)
    {
        if (openedCrib != null)
        {
            baby.SetSkinColour(currentBaby.CheckSkinColour());
            baby.SetSkinBlue(currentBaby.Check_Apgar(), currentBaby.CheckSkinColour(), alreadyOpen);
            pulse.text = "" + Mathf.Floor(currentBaby.Check_aPgar());
            pulsePlayer.playbackSpeed = currentBaby.Check_aPgar()/60f;
            baby.UpdateLimbMovement(currentBaby.Check_apgAr());
            baby.StartBreathing(currentBaby.Check_apgaR());
        }

    }
}
