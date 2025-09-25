using Pixelplacement;
using Pixelplacement.TweenSystem;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Babycontroller : MonoBehaviour
{
    [SerializeField] float breatheHeldFor = 0.5f, chestExpansion = 10f;
    [SerializeField] SpriteRenderer babyChest;
    [SerializeField] TweenVars breathTween;
    [SerializeField][Range(0f, 25f)] float variation = 10f;
    [SerializeField] SkinGradient[] gradientList;
    [SerializeField] SpriteRenderer[] babyBody;
    [SerializeField] SkinVariants[] skinVariations;
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
        yield return new WaitForSeconds(breatheHeldFor);
        currentTween = Tween.Value(babyChest.size.x, babyChest.size.x + (babyChest.size.x * (100 / chestExpansion)), SetWidth, breathTween.duration, 0f, breathTween.easeCurve, completeCallback:()=> CallbackIntermediate(false));
    }
    private IEnumerator BreatheOut()
    {
        yield return new WaitForSeconds(breatheHeldFor);
        currentTween = Tween.Value(babyChest.size.x, babyChest.size.x - (chestExpansion * (babyChest.size.x / chestExpansion+1)), SetWidth, breathTween.duration, 0f, breathTween.easeCurve, completeCallback: () => CallbackIntermediate(true));
    }
    private void CallbackIntermediate(bool x)
    {
        if(x)
        {
            BreatheIn();
        }
        else
        {
            BreatheOut();
        }
    }
    private void SetWidth(float w)
    {
        babyChest.size = new Vector2(w, babyChest.size.y);
    }

    public void SetSkinColour(string colourName)
    {
        foreach(SkinVariants x in skinVariations)
        {
            if(x.colour == colourName)
            {
                SetColour(x);
            }
        }
    }

    private void SetColour(SkinVariants colour)
    {
        if(colour.bodyParts.Count() == babyBody.Count())
        {
            for(int i = 0; i < babyBody.Count(); i++)
            {
                babyBody[i].sprite = colour.bodyParts[i];
            }
        }
    }

    public void SetSkinBlue(int blueLevel, string skinColour)
    {
        SkinGradient matchingGradient = gradientList[0];
        foreach(SkinGradient x in gradientList)
        {
            if(x.name == skinColour)
            {
                matchingGradient = x;
            }
        }

        Vector3 diff = ColourToVector3(matchingGradient.bestSkin) - ColourToVector3(matchingGradient.worstSkin);
        //Debug.Log("Best: " + ColourToVector3(bestSkin) + " | Worst: " + ColourToVector3(worstSkin) + " | Diff: " + diff);
        Vector3 output = ColourToVector3(matchingGradient.bestSkin);
        switch (blueLevel)
        {
            case 0: 
                output = (diff / (100 / variation)) + ColourToVector3(matchingGradient.worstSkin);
                break;
            case 1:
                output = (diff / 2) + ColourToVector3(matchingGradient.worstSkin);
                break;
            case 2:
                output = ColourToVector3(matchingGradient.bestSkin) - (diff / (100 / variation));
                break;
        }

        foreach(SpriteRenderer x in babyBody)
        {
            x.color = Vector3ToColour(output);
        }
    }
    private Vector3 ColourToVector3(Color x)
    {
        Vector3 output = new Vector3
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
