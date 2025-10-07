using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Nursemanager : MonoBehaviour
{
    public static Nursemanager instance;

    [SerializeField] private GameObject nursePrefab;
    [SerializeField] private Nursepathnode deliverNode, retrieveNode;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SendNurse(Crib targetCrib, bool deliverBaby)
    {
        if (!targetCrib.nurseOnTheWay)
        {
            targetCrib.nurseOnTheWay = true;
            Nursemovement newNurse = Instantiate(nursePrefab).GetComponent<Nursemovement>();
            if (deliverBaby)
            {
                newNurse.AddPath(MakePath(targetCrib, deliverNode), targetCrib);
                AudioManager.Instance.PlaySoundEffect("EnterLeft");
            }
            else
            {
                newNurse.AddPath(MakePath(targetCrib, retrieveNode), targetCrib);
                AudioManager.Instance.PlaySoundEffect("EnterRight");
            }
        }
    }

    public List<Nursepathnode> MakePath(Nursepathnode target, Nursepathnode start)
    {
        List<Nursepathnode> queue = new List<Nursepathnode>();
        List<Nursepathnode> searched = new List<Nursepathnode>();
        List<Nursepathnode> path = new List<Nursepathnode>();
        Nursepathnode currentNode;
        bool pathfound = false;
        queue.Add(start);
        while (queue.Count > 0)
        {
            currentNode = queue[0];
            queue.RemoveAt(0);
            searched.Add(currentNode);
            //Debug.Log(currentNode);
            //Debug.Log(searched.Count +" searched - " + currentNode.accessibleCribs.Count() + " cribs surrounding");
            if (AtDestination(currentNode, target))
            {
                while (!pathfound)
                {
                    //Debug.Log("Looking for path " + currentNode);
                    path.Insert(0, currentNode);
                    if (currentNode == start)
                    {
                        pathfound = true;
                    }
                    else
                    {
                        currentNode = currentNode.lastNode;
                    }
                }
            }
            else
            {
                foreach (Nursepathnode x in currentNode.neighbours)
                {
                    if (!searched.Contains(x) && !queue.Contains(x))
                    {
                        x.lastNode = currentNode;
                        queue.Add(x);
                    }
                }
            }
        }
        if (!pathfound)
        {
            string x = "Failed to find path from " + start + " to " + target + ": ";
            foreach (Nursepathnode y in path)
            {
                x += y.gameObject.name + ", ";
            }
            Debug.Log(x);
        }
        return path;
        //Makes the nurse and sends them on their way
    }

    private bool AtDestination(Nursepathnode x, Nursepathnode y)
    {
        bool z = false;
        if ((x.accessibleCribs.Count() > 0 && x.accessibleCribs.Contains(y)) || x == y)
        {
            z = true;
        }
        return z;
    }

    private void DebugPath(List<Nursepathnode> y)
    {
        String prtnString = "";
        foreach (Nursepathnode x in y)
        {
            prtnString += x.gameObject.name + " --> ";
        }
        prtnString += "Crib";
        //Debug.Log(prtnString);
    }
}
