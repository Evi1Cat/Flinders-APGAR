using UnityEngine;

public class GrimaceButton : MonoBehaviour
{
    public void TriggerGrimace()
    {
        Babycontroller.Instance.TweenHandToChest(Opencrib.Instance.GetbabyGrimace());
    }
}
