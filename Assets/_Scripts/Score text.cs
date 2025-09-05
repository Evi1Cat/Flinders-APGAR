using TMPro;
using UnityEngine;

public class Scoretext : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    void Start()
    {
        GameManager.instance.Increase += UpdateText;
        GameManager.instance.Decrease += UpdateText;
    }

    // Update is called once per frame
    private void UpdateText()
    {
        score.text = GameManager.instance.points + "";
    }
}
