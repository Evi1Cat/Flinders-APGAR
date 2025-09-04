using UnityEngine;

public class Crib : MonoBehaviour
{
    [SerializeField] Sprite empty, full;
    [SerializeField] Opencrib openCrib;
    private SpriteRenderer cribSprite;
    private Baby baby = null;
    void Start()
    {
        cribSprite = gameObject.GetComponent<SpriteRenderer>();
        cribSprite.sprite = empty;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCrib()
    {
        openCrib.Open(baby, this);
    }

    public void NewBaby()
    {
        float x = 0;
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

    public void ReleaseBaby(int x)
    {
        //Logic for 
        baby = null;
        cribSprite.sprite = empty;
    }

    private int OTT()
    {
        return Random.Range(0, 3);
    }
}
