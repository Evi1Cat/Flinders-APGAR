using UnityEngine;

public class Nursepathnode : MonoBehaviour
{
    public Crib[] accessibleCribs = new Crib[0];
    public Nursepathnode[] neighbours = new Nursepathnode[0];
    public Nursepathnode lastNode = null;
}
