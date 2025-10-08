using Pixelplacement;
using Pixelplacement.TweenSystem;
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Babycontroller : MonoBehaviour
{
    public static Babycontroller Instance;
    [Header("Baby Breathing Variables")]
    [SerializeField] float breatheHeldFor = 0.5f;
    [SerializeField][Range(1f, 1.5f)] float chestExpansion = 10f;
    [SerializeField] SpriteRenderer babyChest;
    [SerializeField] TweenVars breathTween;
    private int breathingRate = 0;
    private bool currentlyBreathing = false;
    private Vector2 initSize = new(1, 1);
    private TweenBase currentTween;
    [Header("Baby Blue Skin Variables")]
    [SerializeField][Range(0f, 1f)] float midpoint;
    [SerializeField][Range(0f, 1f)] float gap;
    [SerializeField] SkinGradient[] gradientList;
    [SerializeField] SpriteRenderer[] babyBody;
    [SerializeField] TweenVars blueChangeTween;
    [Header("Baby Limb Activity Variables")]
    [Range(0f, 1f)] public float moveMult;
    [Range(0f, 10f)] public float limbMovementSpeed;
    [SerializeField] AnimationCurve[] movementCurves;
    [SerializeField] PivotNode[] joints;
    [Header("Baby Grimace Variables")]
    [SerializeField] SpriteRenderer[] faceRenderers;
    [SerializeField] TweenVars faceFadeVars;
    [SerializeField] TweenVars handFadeVars;
    [SerializeField] TweenVars handMoveVars;
    [SerializeField] Vector3 starPos, endPos;
    [SerializeField] GameObject hand;
    private TweenBase faceFaceTween;
    [Header("Baby Variables")]
    [SerializeField] public SkinVariants[] skinVariations;
    void Awake()
    {
        Instance = this;
        //SetSkinBlue(1, "white");
        foreach (PivotNode x in joints)
        {
            x.SetManager(this);
        }
    }
    private void Update()
    {

    }
    private void BreatheIn()
    {
        //Debug.Log("In");
        if (gameObject.activeInHierarchy)
        {
            float breatheWait = breatheHeldFor, chestSize = chestExpansion;
            switch (breathingRate)
            {
                case 0:
                    chestSize = 1;
                    breatheWait = 0f;
                    break;
                case 1:
                    breatheWait = Random.Range(breatheHeldFor - (breatheHeldFor * 0.7f), breatheHeldFor + (breatheHeldFor * 2f));
                    StartCoroutine(BreatheNoise(breatheWait, false));
                    break;
                case 2:
                    StartCoroutine(BreatheNoise(breatheWait, true));
                    break;
            }
            currentTween = Tween.Value(1, chestSize, SetWidth, breathTween.duration, breatheWait, breathTween.easeCurve, completeCallback: BreatheOut);
        }
    }
    private void BreatheOut()
    {
        //Debug.Log("Out");
        float chestSize = chestExpansion;
        if (breathingRate == 0)
        {
            chestSize = 1;
        }
        currentTween = Tween.Value(chestSize, 1, SetWidth, breathTween.duration, breatheHeldFor, breathTween.easeCurve, completeCallback: BreatheIn);
    }
    private IEnumerator BreatheNoise(float wait, bool healthy)
    {
        yield return new WaitForSeconds(wait);
        //Debug.Log("PlayingBreathingNoise");
        if (healthy)
        {
            AudioManager.Instance.PlaySoundEffect("HealthyBreath" + Random.Range(1, 5));
        }
        else
        {
            AudioManager.Instance.PlaySoundEffect("UnhealthyBreath" + Random.Range(1, 5));
        }
    }
    public void StartBreathing(int breathingIntensity)
    {
        breathingRate = breathingIntensity;
        if (!currentlyBreathing)
        {
            currentlyBreathing = true;
            currentTween?.Cancel();
            babyChest.transform.localScale = initSize;
            BreatheIn();
        }
    }
    public void StopBreathing()
    {
        //currentTween.Cancel();
        //babyChest.transform.localScale = initSize;
        currentlyBreathing = false;
    }
    private void SetWidth(float w)
    {
        //Debug.Log(w);
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

    public void SetSkinBlue(int blueLevel, string skinColour, bool alreadyOpen)
    {
        SkinGradient matchingGradient = gradientList[0];
        Color output = matchingGradient.bestSkin;

        if (Opencrib.Instance.GetBabyHue() == Color.black)
        {
            foreach (SkinGradient x in gradientList)
            {
                if (x.name == skinColour)
                {
                    //Debug.Log(x.name + " matches with " + skinColour);
                    matchingGradient = x;
                }
            }

            Vector3 scaleMidpoint = (ColourToVector3(matchingGradient.bestSkin) - ColourToVector3(matchingGradient.worstSkin)) * midpoint;
            //Debug.Log("Best: " + ColourToVector3(bestSkin) + " | Worst: " + ColourToVector3(worstSkin) + " | Diff: " + diff);
            switch (blueLevel)
            {
                case 2:
                    output = ColourRandomRange(Vector3ToColour(ColourToVector3(matchingGradient.bestSkin) - (scaleMidpoint * gap)), matchingGradient.bestSkin);
                    break;
                case < 2:
                    output = ColourRandomRange(matchingGradient.worstSkin, Vector3ToColour(ColourToVector3(matchingGradient.bestSkin) - (scaleMidpoint * (2 - gap))));
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
                    BlueFade(x, output, alreadyOpen);
                    break;
                case 1:
                    if (objName == "337")
                    {
                        BlueFade(x, output, alreadyOpen);
                    }
                    else
                    {
                        BlueFade(x, matchingGradient.bestSkin, alreadyOpen);
                    }
                    break;
                case 2:
                    BlueFade(x, output, alreadyOpen);
                    break;
            }
        }
    }
    private void BlueFade(SpriteRenderer x, Color y, bool alreadyOpen)
    {
        if (alreadyOpen)
        {
            Tween.Color(x, y, blueChangeTween.duration, blueChangeTween.delay, blueChangeTween.easeCurve);
        }
        else
        {
            x.color = y;
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
        Vector3 output = (diff * Random.Range(0f, 1f)) + ColourToVector3(min);
        return Vector3ToColour(output);
    }

    public AnimationCurve getTweenCurve()
    {
        return movementCurves[Random.Range(0, movementCurves.Count())];
    }
    public void UpdateLimbMovement(int y)
    {
        foreach (PivotNode x in joints)
        {
            x.Set(y);
        }
    }
    public void TweenHandToChest(int grimmace)
    {
        Tween.LocalPosition(hand.transform, starPos, endPos, handMoveVars.duration, handMoveVars.delay, handMoveVars.easeCurve, completeCallback: () => SetFace(grimmace));
        Tween.Value(0f, 1f, (x) => SetFaceAlpha(x, hand.GetComponent<SpriteRenderer>(), true), handFadeVars.duration, handFadeVars.delay, handFadeVars.easeCurve);
    }
    private IEnumerator TweenHandAwayFromChest(float wait)
    {
        yield return new WaitForSeconds(wait);
        Tween.LocalPosition(hand.transform,endPos, starPos, handMoveVars.duration, handMoveVars.delay, handMoveVars.easeCurve);
        Tween.Value(0f, 1f, (x) => SetFaceAlpha(x, hand.GetComponent<SpriteRenderer>(), false), handFadeVars.duration, handFadeVars.delay, handFadeVars.easeCurve);
    }

    private void SetFace(int babyGrimace)
    {
        
        switch (babyGrimace)
        {
            case 0:
                StartCoroutine(TweenHandAwayFromChest(faceFadeVars.duration * 2));
                break;
            case 1:
                ChooseFace(false, true, false, 0);
                break;
            case 2:
                ChooseFace(false, false, true, 0);
                AudioManager.Instance.PlaySoundEffect("BabyCry"+Random.Range(1,5));
                break;
        }
    }
    private void ChooseFace(bool nuetral, bool peeved, bool upset, float replay)
    {
        if (replay < 2)
        {
            Tween.Value(faceRenderers[0].color.a, 1f, (x)=> SetFaceAlpha(x, faceRenderers[0], nuetral), faceFadeVars.duration, faceFadeVars.delay, faceFadeVars.easeCurve, completeCallback: ()=> ChooseFace(true, false, false, replay+1));
            
            Tween.Value(faceRenderers[1].color.a, 1f, (x)=> SetFaceAlpha(x, faceRenderers[1], peeved), faceFadeVars.duration, faceFadeVars.delay, faceFadeVars.easeCurve);
            Tween.Value(faceRenderers[2].color.a, 1f, (x)=> SetFaceAlpha(x, faceRenderers[2], upset), faceFadeVars.duration, faceFadeVars.delay, faceFadeVars.easeCurve);
        }
        else
        {
            StartCoroutine(TweenHandAwayFromChest(0));
        }
    }
    private void SetFaceAlpha(float x, SpriteRenderer face, bool setActive)
    {
        Color temp = face.color;
        if (setActive)
        {
            temp.a = x;
        }
        else
        {
            temp.a = 1f - x;
        }
        face.color = temp;
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
