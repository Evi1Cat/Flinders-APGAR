using System;
using TMPro;
using UnityEngine;

public class Opencrib : MonoBehaviour
{

    [SerializeField] private TMP_Text blueSkin, pulse, irritability, muscleTone, respiratory, timerText;

    private Crib openedCrib = null;
    private Baby currentBaby = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open(Baby x, Crib refer)
    {
        if (x != null && !refer.nurseOnTheWay)
        {
            gameObject.SetActive(true);
            openedCrib = refer;
            currentBaby = x;
            openedCrib.timerTick += UpdateText;
        }
    }

    public void Close()
    {
        if(openedCrib != null)
        {
            openedCrib.timerTick -= UpdateText;
        }
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

    private void UpdateText(int timeInSeconds)
    {
        int mins = (int)Mathf.Floor((float)timeInSeconds / 60f);
        String seconds = timeInSeconds - (mins * 60)+"";
        if (int.Parse(seconds) < 10)
        {
            seconds = 0 + seconds;
        }
        timerText.text = mins + ":" + seconds;

        blueSkin.text = "" + currentBaby.Check_Apgar();
        pulse.text = "" + Mathf.Floor(currentBaby.Check_aPgar());
        irritability.text = "" + currentBaby.Check_apGar();
        muscleTone.text = "" + currentBaby.Check_apgAr();
        respiratory.text = "" + currentBaby.Check_apgaR();

    }
}
