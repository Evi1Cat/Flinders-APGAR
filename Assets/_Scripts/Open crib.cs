using System;
using TMPro;
using UnityEngine;

public class Opencrib : MonoBehaviour
{

    [SerializeField] private TMP_Text blueSkin, pulse, irritability, muscleTone, respiratory, timerText;

    private Crib openedCrib = null;
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
        if (x != null)
        {
            blueSkin.text = "" + x.Check_Apgar();
            pulse.text = "" + Mathf.Floor(x.Check_aPgar());
            irritability.text = "" + x.Check_apGar();
            muscleTone.text = "" + x.Check_apgAr();
            respiratory.text = "" + x.Check_apgaR();
            gameObject.SetActive(true);
            openedCrib = refer;
            openedCrib.timerTick += UpdateTime;
        }
    }

    public void Close()
    {
        openedCrib.timerTick -= UpdateTime;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Tells the crib to remove the baby
    /// </summary>
    /// <param name="x"> 1 for a healthy baby and -1 for an urgent care baby</param>
    public void RemoveBaby(int x)
    {
        openedCrib.ReleaseBaby(x);
        gameObject.SetActive(false);
    }

    private void UpdateTime(int timeInSeconds)
    {
        int mins = (int)Mathf.Floor((float)timeInSeconds / 60f);
        String seconds = timeInSeconds - (mins * 60)+"";
        if (int.Parse(seconds) < 10)
        {
            seconds = 0 + seconds;
        }
        timerText.text = mins + ":" + seconds;
    }
}
