using System;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class Opencrib : MonoBehaviour
{
    public static Opencrib Instance;
    [SerializeField] private TMP_Text pulse, irritability, muscleTone, respiratory, timerText;
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
        if (openedCrib != null)
        {
            //Debug.Log("test");
            openedCrib.timerTick -= UpdateText;
            baby.StopBreathing();
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

    private void UpdateText(int timeInSeconds)
    {
        int mins = (int)Mathf.Floor((float)timeInSeconds / 60f);
        String seconds = timeInSeconds - (mins * 60) + "";
        if (int.Parse(seconds) < 10)
        {
            seconds = 0 + seconds;
        }
        timerText.text = mins + ":" + seconds;


        if (openedCrib != null)
        {
            baby.StartBreathing();
            baby.SetSkinColour(currentBaby.CheckSkinColour());
            baby.SetSkinBlue(currentBaby.Check_Apgar(), "white");
            pulse.text = "" + Mathf.Floor(currentBaby.Check_aPgar());
            irritability.text = "" + currentBaby.Check_apGar();
            muscleTone.text = "" + currentBaby.Check_apgAr();
            respiratory.text = "" + currentBaby.Check_apgaR();
        }

    }
}
