using System.Diagnostics;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PivotNode : MonoBehaviour
{
    [Range(0, 2)] private int activityScore = 2;
    private int currentlyPerforming = -1;
    [SerializeField]private float movementDuration = 1f;
    [SerializeField] private Quaternion pivotMin, pivotMax;
    private TweenBase currentTween;
    private Babycontroller manager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activityScore != currentlyPerforming || currentTween?.Status == Tween.TweenStatus.Finished || currentTween?.Status == Tween.TweenStatus.Canceled)
        {
            //Debug.Log("changing behaviour");
            switch (activityScore)
            {
                case 0:
                    currentlyPerforming = 0;
                    currentTween = null;
                    RandomizeRot();
                    break;
                case 1:
                    currentlyPerforming = 1;
                    //Debug.Log(currentTween?.Status);
                    if (currentTween == null || currentTween.Status == Tween.TweenStatus.Finished || currentTween.Status == Tween.TweenStatus.Canceled)
                    {
                        RandomizeRot();
                        Move();
                    }
                    movementDuration = manager.limbMovementSpeed;
                    break;
                case 2:
                    currentlyPerforming = 2;
                    if (currentTween == null || currentTween.Status == Tween.TweenStatus.Finished || currentTween.Status == Tween.TweenStatus.Canceled)
                    {
                        RandomizeRot();
                        Move();
                    }
                    movementDuration = manager.limbMovementSpeed * manager.moveMult;
                    break;
            }
        }
    }
    private void Move()
    {
        if (gameObject.activeInHierarchy)
        {
            //Debug.Log("rotting");
            currentTween = Tween.Rotation(transform, NewPivot(), movementDuration, 0f, manager.getTweenCurve(), completeCallback: Move);
        }
        else
        {
            activityScore = -1;
        }
    }
    private void RandomizeRot()
    {
        transform.localRotation = NewPivot();
    }
    private Quaternion NewPivot()
    {
        Quaternion rot = Quaternion.Slerp(pivotMin, pivotMax, Random.Range(0f,1f));
        //Debug.Log(rot);
        
        //Debug.Log(pivotMin + " - " + pivotMax);
        return rot;
    }
    [ContextMenu("Set pivots max rotation")]
    public void SetPivotMax()
    {
        pivotMax = transform.localRotation;
        Debug.Log("Set the max pivot to: " + pivotMax);
    }
    [ContextMenu("Set pivots min rotation")]
    public void SetPivotMin()
    {
        pivotMin = transform.localRotation;
        Debug.Log("Set the min pivot to: " + pivotMin);
    }
    public void SetManager(Babycontroller m)
    {
        manager = m;
    }
    public void Set(int score)
    {
        activityScore = score;
    }
}
