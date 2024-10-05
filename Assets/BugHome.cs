using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugHome : MonoBehaviour
{
    public Globals Globals;
    public List<Bug> Bugs;

    void Start()
    {
        Globals = GameObject.FindObjectOfType<Globals>();
        foreach (Bug bug in GameObject.FindObjectsOfType<Bug>())
        {
            Bugs.Add(bug);
        }
    }

    void Update()
    {
        foreach (Bug bug in Bugs)
        {
            bug.HomeUpdate();
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(Globals.BugHomeWidth, Globals.BugHomeHeight, 1));
        
    }
}
