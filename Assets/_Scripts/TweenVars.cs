using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;

[Serializable]
public class TweenVars
{
    [SerializeField] public float duration = 0f, delay = 0f;
    [SerializeField] public AnimationCurve easeCurve = new AnimationCurve();
}
