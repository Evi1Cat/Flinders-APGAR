using Pixelplacement;
using Pixelplacement.TweenSystem;
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Babycontroller : MonoBehaviour
{
    [SerializeField] float breatheHeldFor = 0.5f;
    [SerializeField][Range(1f, 1.5f)] float chestExpansion = 10f;
    [SerializeField] SpriteRenderer babyChest;
    [SerializeField] TweenVars breathTween;
    [SerializeField][Range(0f, 1f)] float midpoint = 0.1f, gap = 0.95f;
    [SerializeField] SkinGradient[] gradientList;
    [SerializeField] SpriteRenderer[] babyBody;
    [SerializeField] SkinVariants[] skinVariations;
    private int breathingRate = 0;
    private bool currentlyBreathing = false;
    private Vector2 initSize = new(0, 0);
    private TweenBase currentTween;
    void Start()
    {
        //SetSkinBlue(1, "white");
    }
    private void Update()
    {

    }
    private IEnumerator BreatheIn()
    {
        float breatheWait = breatheHeldFor, chestSize = chestExpansion;
        switch (breathingRate)
        {
            case 0:
                chestSize = 1;
                breatheWait = 0f;
                break;
            case 1:
                breatheWait = Random.Range(breatheHeldFor - (breatheHeldFor * 0.9f), breatheHeldFor + (breatheHeldFor * 2f));
                break;
        }
        yield return new WaitForSeconds(breatheWait);
        currentTween = Tween.Value(1, chestSize, SetWidth, breathTween.duration, 0f, breathTween.easeCurve, completeCallback: () => CallbackIntermediate(false));
    }
    private IEnumerator BreatheOut()
    {
        float breatheWait = breatheHeldFor, chestSize = chestExpansion;
        if (breathingRate == 0)
        {
            chestSize = 1;
        }
        yield return new WaitForSeconds(breatheWait);
        currentTween = Tween.Value(chestSize, 1, SetWidth, breathTween.duration, 0f, breathTween.easeCurve, completeCallback: () => CallbackIntermediate(true));
    }
    public void StartBreathing(int breathingIntensity)
    {
        if (!currentlyBreathing)
        {
            currentlyBreathing = true;
            CallbackIntermediate(true);
        }
        breathingRate = breathingIntensity;
    }
    public void StopBreathing()
    {
        babyChest.transform.localScale = initSize;
        currentTween.Cancel();
        currentlyBreathing = false;
    }
    private void CallbackIntermediate(bool x)
    {
        if (x)
        {
            StartCoroutine(BreatheIn());
        }
        else
        {
            StartCoroutine(BreatheOut());
        }
    }
    private void SetWidth(float w)
    {
        babyChest.transform.localScale = new Vector2(w, 1);
        //Debug.Log(babyChest.size);
    }

    public void SetSkinColour(string colourName)
    {
        foreach (SkinVariants x in skinVariations)
        {
            if (x.colour == colourName)
            {
                SetColour(x);
            }
        }
    }

    private void SetColour(SkinVariants colour)
    {
        if (colour.bodyParts.Count() == babyBody.Count())
        {
            for (int i = 0; i < babyBody.Count(); i++)
            {
                babyBody[i].sprite = colour.bodyParts[i];
            }
        }
    }

    public void SetSkinBlue(int blueLevel, string skinColour)
    {
        SkinGradient matchingGradient = gradientList[0];
        Color output = matchingGradient.bestSkin;

        if (Opencrib.Instance.GetBabyHue() == Color.black)
        {
            foreach (SkinGradient x in gradientList)
            {
                if (x.name == skinColour)
                {
                    matchingGradient = x;
                }
            }

            Vector3 scaleMidpoint = (ColourToVector3(matchingGradient.bestSkin) - ColourToVector3(matchingGradient.worstSkin)) * midpoint;
            //Debug.Log("Best: " + ColourToVector3(bestSkin) + " | Worst: " + ColourToVector3(worstSkin) + " | Diff: " + diff);
            switch (blueLevel)
            {
                case 0:
                    output = ColourRandomRange(Vector3ToColour(ColourToVector3(matchingGradient.bestSkin) - (scaleMidpoint*gap)), matchingGradient.bestSkin);
                    break;
                case > 0:
                    output = ColourRandomRange(matchingGradient.worstSkin, Vector3ToColour(ColourToVector3(matchingGradient.bestSkin) - (scaleMidpoint*(2-gap))));
                    break;
            }
            Opencrib.Instance.SetBabyHue(output);
        }
        else
        {
            output = Opencrib.Instance.GetBabyHue();
        }

        foreach (SpriteRenderer x in babyBody)
        {
            string objName = x.gameObject.name;
            objName = objName[0] + objName[1] + objName[2] + "";
            //Debug.Log(objName);
            switch (blueLevel)
            {
                case 0:
                    x.color = output;
                    break;
                case 1:
                    if (objName == "337")
                    {
                        x.color = output;
                    }
                    else
                    {
                        x.color = matchingGradient.bestSkin;
                    }
                    break;
                case 2:
                    x.color = output;
                    break;
            }
        }
    }
    private Vector3 ColourToVector3(Color x)
    {
        Vector3 output = new()
        {
            x = x.r,
            y = x.g,
            z = x.b
        };
        return output;
    }
    private Color Vector3ToColour(Vector3 x)
    {
        Color output = Color.white;
        output.r = x.x;
        output.g = x.y;
        output.b = x.z;
        //Debug.Log(output + ": " + output.r + " - " + output.g + " - " + output.b + " | (" + x + ")");
        return output;
    }

    private Color ColourRandomRange(Color min, Color max)
    {
        Vector3 diff = ColourToVector3(max) - ColourToVector3(min);
        Vector3  output = (diff * Random.Range(0f,1f)) + ColourToVector3(min);
        return Vector3ToColour(output);
    }
}

[Serializable]
public class SkinGradient
{
    [SerializeField] public string name;
    [SerializeField] public Color bestSkin, worstSkin;
}

[Serializable]
public class SkinVariants
{
    [SerializeField] public string colour;
    [SerializeField] public Sprite[] bodyParts;
}
