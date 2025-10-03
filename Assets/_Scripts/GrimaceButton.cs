using UnityEngine;

public class GrimaceButton : MonoBehaviour
{
    public void TriggerGrimace()
    {
        Babycontroller.Instance.SetFace(Opencrib.Instance.GetbabyGrimace());
    }
}
