using TMPro;
using UnityEngine;

public class Clickscounterscript : MonoBehaviour
{
    [SerializeField] public TMP_Text counter;
    void Start()
    {
        counter.text = "0";
    }

    // Update is called once per frame
    public void Incriment()
    {
        counter.text = "" + (1 + int.Parse(counter.text));
    }
}
