using UnityEngine;
using System.Collections.Generic;
using Pixelplacement;

public class Nursemovement : MonoBehaviour
{
    public TweenVars[] nurseTweens = new TweenVars[3];
    private List<Nursepathnode> path;
    public void AddPath(List<Nursepathnode> x)
    {
        path = x;
        transform.position = path[0].transform.position;
        path.RemoveAt(0);
        Tween.Position(transform, path[0].transform.position, nurseTweens[0].duration, nurseTweens[0].delay, nurseTweens[0].easeCurve, completeCallback: MoveToNextPoint);
        path.RemoveAt(0);
    }

    private void MoveToNextPoint()
    {
        if (path.Count > 1)
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[1].duration, nurseTweens[1].delay, nurseTweens[1].easeCurve, completeCallback: MoveToNextPoint);
        }
        else
        {
            Tween.Position(transform, path[0].transform.position, nurseTweens[2].duration, nurseTweens[2].delay, nurseTweens[2].easeCurve, completeCallback: PlaceBaby);
        }
        path.RemoveAt(0);
    }
    private void PlaceBaby()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
