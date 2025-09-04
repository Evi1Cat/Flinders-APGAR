using Unity.VisualScripting;
using UnityEngine;

public class BabyInputStream : MonoBehaviour
{
    public static BabyInputStream instance;

    [SerializeField] private float babyTimer = 60;
    [SerializeField] private Crib[] cribList;
    private int babiesSaved = 0;
    private float timer = 0;
    public bool activeGame = false;
    void Start()
    {
        instance = this;
        timer = babyTimer;
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
                timer = babyTimer;

                
                int index = Random.Range(0, cribList.Length);
                bool roomFound = false;
                int i = index;
                while (!roomFound)
                {
                    //Debug.Log(cribList[i].Occupied());
                    if (!cribList[i].Occupied() && !cribList[i].nurseOnTheWay)
                    {
                        roomFound = true; //If there is room in one of the cribs
                        cribList[i].nurseOnTheWay = true;
                        Nursemanager.instance.MakePath(cribList[i]);
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

    public void Score(int x)
    {
        babiesSaved += x;
    }
}
