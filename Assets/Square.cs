using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public Globals Globals;
    public int row, col;


    public bool HasBug()
    {
        return transform.childCount > 0;
    }

    public int GetOwner()
    {
        if (!HasBug()) return 2;
        return (int)transform.GetComponentInChildren<Bug>().owner;
    }

    void Start()
    {
        Globals = GameObject.FindObjectOfType<Globals>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
