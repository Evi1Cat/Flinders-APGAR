using UnityEngine;
using System.Collections.Generic;
using Pixelplacement;

public class Nursemovement : MonoBehaviour
{
    //[Range(0, 100)] public int curvePercent = 0;
    public TweenVars[] nurseTweens = new TweenVars[3];
    [SerializeField] AnimationCurve turnTweenCurve;
    private Crib targetCrib = null;
    private Nursepathnode home, current;
    private List<Nursepathnode> path;
    public void AddPath(List<Nursepathnode> x, Crib y)
    {
        path = x;
        targetCrib = y;
        transform.position = path[0].transform.position;
        home = path[0];
        path.RemoveAt(0);
        Tween.Position(transform, path[0].transform.position, nurseTweens[0].duration, nurseTweens[0].delay, nurseTweens[0].easeCurve, completeCallback: MoveToNextPoint);
        path.RemoveAt(0);
    }

    private void MoveToNextPoint()
    {
        Rotate();
        if (path.Count > 1)
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: MoveToNextPoint);
        }
        else
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[2].duration, nurseTweens[2].delay, nurseTweens[2].easeCurve, completeCallback: PlaceBaby);
        }
        current = path[0];
        path.RemoveAt(0);
    }
    private void PlaceBaby()
    {
        if (targetCrib.Occupied())
        {
            targetCrib.ReleaseBaby();
            BabyInputStream.instance.RecalculateTimer();
        }
        else
        {
            targetCrib.NewBaby();
            AudioManager.Instance.PlaySoundEffect("SpawnMum" + Random.Range(1, 4));
        }
        targetCrib.nurseOnTheWay = false;
        targetCrib.CloseCrib();
        //Debug.Log(current);
        path = Nursemanager.instance.MakePath(home, current);
        //Debug.Log(path);
        Tween.Position(transform, path[0].transform.position, nurseTweens[0].duration, 0.5f, nurseTweens[0].easeCurve, completeCallback: Leave);
        path.RemoveAt(0);
        
    }
    private void Leave()
    {
        if (path.Count > 1)
        {
            Rotate();
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: Leave);
        }
        else
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: Destroy);
        }
        path.RemoveAt(0);
    }
    private void Rotate()
    {
        //Debug.Log(path.Count);
        if (path.Count >= 2)
        {
            Vector3 firstDIff = new(-1, -1, -1), secondDIff = new(-1, -1, -1);
            firstDIff = transform.position - path[0].gameObject.transform.position;
            secondDIff = path[0].gameObject.transform.position - path[1].gameObject.transform.position;
            //Debug.Log(firstDIff + " --- " + secondDIff);
            if (firstDIff.x != secondDIff.x && firstDIff.y != secondDIff.y)
            {
                float lookTo = 0f;
                if (firstDIff.x == 0)
                {
                    if (secondDIff.x > 0)
                    {
                        //Debug.Log("Look up");
                        lookTo = 270f;
                    }
                    if (secondDIff.x < 0)
                    {
                        //Debug.Log("Look up");
                        lookTo = 90f;
                    }
                }
                else if (firstDIff.y == 0)
                {
                    if (secondDIff.y > 0)
                    {
                        //Debug.Log("Look right");
                        lookTo = 0f;
                    }
                    if (secondDIff.y < 0)
                    {
                        //Debug.Log("Look left");
                        lookTo = 180f;
                    }
                }
                Tween.Rotation(transform, new Vector3(0, 0, lookTo), nurseTweens[1].duration, nurseTweens[1].duration / 2, turnTweenCurve);
            }
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
