using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Nursemanager : MonoBehaviour
{
    public static Nursemanager instance;

    [SerializeField] private Nursepathnode startNode;
    private Nursepathnode[] path;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakePath(Crib targetCrib)
    {
        List<Nursepathnode> queue = new List<Nursepathnode>();
        List<Nursepathnode> searched = new List<Nursepathnode>();
        List<Nursepathnode> path = new List<Nursepathnode>();
        Nursepathnode currentNode;
        bool pathfound = false;
        queue.Add(startNode);
        while (queue.Count > 0 && !pathfound)
        {
            currentNode = queue[0];
            //Debug.Log(currentNode);
            //Debug.Log(searched.Count +" searched - " + currentNode.accessibleCribs.Count() + " cribs surrounding");
            if (currentNode.accessibleCribs.Count() > 0 && AtDestination(currentNode, targetCrib))
            {
                while (!pathfound)
                {
                    //Debug.Log("Looking for path " + currentNode);
                    path.Add(currentNode);
                    if (currentNode.lastNode == null)
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
                queue.RemoveAt(0);
                searched.Add(currentNode);
            }
        }
        DebugPath(path);
    }

    private bool AtDestination(Nursepathnode x, Crib y)
    {
        bool z = false;
        if (x.accessibleCribs.Contains(y))
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
