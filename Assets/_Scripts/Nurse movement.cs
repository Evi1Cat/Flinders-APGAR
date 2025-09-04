using UnityEngine;
using System.Collections.Generic;
using Pixelplacement;

public class Nursemovement : MonoBehaviour
{
    //[Range(0, 100)] public int curvePercent = 0;
    public TweenVars[] nurseTweens = new TweenVars[3];
    private Crib targetCrib = null;
    private List<Nursepathnode> path, pathBack = new List<Nursepathnode>();
    public void AddPath(List<Nursepathnode> x, Crib y)
    {
        path = x;
        targetCrib = y;
        transform.position = path[0].transform.position;
        pathBack.Insert(0, path[0]);
        path.RemoveAt(0);
        Tween.Position(transform, path[0].transform.position, nurseTweens[0].duration, nurseTweens[0].delay, nurseTweens[0].easeCurve, completeCallback: MoveToNextPoint);
        pathBack.Insert(0, path[0]);
        path.RemoveAt(0);
    }

    private void MoveToNextPoint()
    {
        //Vector3 target = path[0].transform.position;

        if (path.Count > 1)
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: MoveToNextPoint);
            pathBack.Insert(0, path[0]);
        }
        else
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[2].duration, nurseTweens[2].delay, nurseTweens[2].easeCurve, completeCallback: PlaceBaby);
        }
        path.RemoveAt(0);
    }
    private void PlaceBaby()
    {
        targetCrib.NewBaby();
        targetCrib.nurseOnTheWay = false;
        path = pathBack;
        Tween.Position(transform, path[0].transform.position, nurseTweens[0].duration, 0.5f, nurseTweens[0].easeCurve, completeCallback: Leave);
        path.RemoveAt(0);
    }
    private void Leave()
    {
        if (path.Count > 1)
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: Leave);
        }
        else
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: Destroy);
        }
        path.RemoveAt(0);
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
