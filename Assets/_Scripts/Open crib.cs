using TMPro;
using UnityEngine;

public class Opencrib : MonoBehaviour
{

    [SerializeField] private TMP_Text blueSkin;
    [SerializeField] private TMP_Text pulse;
    [SerializeField] private TMP_Text irritability;
    [SerializeField] private TMP_Text muscleTone;
    [SerializeField] private TMP_Text respiratory;

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
        blueSkin.text = "" + x.Check_Apgar();
        pulse.text = "" + Mathf.Floor(x.Check_aPgar());
        irritability.text = "" + x.Check_apGar();
        muscleTone.text = "" + x.Check_apgAr();
        respiratory.text = "" + x.Check_apgaR();
        gameObject.SetActive(true);
        openedCrib = refer;
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
}
