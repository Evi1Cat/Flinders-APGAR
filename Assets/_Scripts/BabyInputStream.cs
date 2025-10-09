using Unity.VisualScripting;
using UnityEngine;

public class BabyInputStream : MonoBehaviour
{
    public static BabyInputStream instance;

    [SerializeField] private float babyTimer = 60;
    [SerializeField][Range(0f, 1f)] private float spawnVariation;
    [SerializeField] private Crib[] cribList;
    private float timer = 0;
    private int currentBabies = 0;
    public bool activeGame = false;
    void Start()
    {
        instance = this;
        timer = babyTimer * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeGame)
        {
            timer -= Time.deltaTime * GameManager.instance.timeSpeedModifier;
            //Debug.Log(timer);
            if (timer < 0)
            {
                CheckCurrentBabies();
                timer = Mathf.Pow((currentBabies - GameManager.instance.babySettings.preferredBabiesOnScreen) / 1.2f, 3f) + (babyTimer * Random.Range(1f - spawnVariation, 1f + spawnVariation));
                //Debug.Log(timer);


                int index = Random.Range(0, cribList.Length);
                bool roomFound = false;
                int i = index;
                while (!roomFound)
                {
                    //Debug.Log(cribList[i].Occupied());
                    if (!cribList[i].Occupied())
                    {
                        roomFound = true; //If there is room in one of the cribs
                        Nursemanager.instance.SendNurse(cribList[i], true);
                        //Debug.Log("baby!");
                    }
                    else
                    {
                        i += 1;
                        if (i >= cribList.Length)
                        {
                            i = 0;
                        }
                        if (i == index)
                        {
                            roomFound = true; //If all the cribs have been searched, ends the loop
                        }
                    }
                }

            }
        }
    }
    private void CheckCurrentBabies()
    {
        currentBabies = 0;
        foreach (Crib x in cribList)
        {
            if (x.Occupied() || x.nurseOnTheWay)
            {
                currentBabies++;
            }
        }
        //Debug.Log(currentBabies + " current babies");
    }

    public void RecalculateTimer()
    {
        //Debug.Log(timer);
        float x = Mathf.Pow((currentBabies - GameManager.instance.babySettings.preferredBabiesOnScreen) / 1.2f, 3f) + (babyTimer * Random.Range(1f - spawnVariation, 1f + spawnVariation));
        float percent = timer / x;
        //Debug.Log(timer + " / " + x +" = "+ percent);
        CheckCurrentBabies();
        timer = (Mathf.Pow((currentBabies - GameManager.instance.babySettings.preferredBabiesOnScreen-1) / 1.2f, 3f) + (babyTimer * Random.Range(1f - spawnVariation, 1f + spawnVariation)))*percent;
        //Debug.Log(timer);
    }
}
