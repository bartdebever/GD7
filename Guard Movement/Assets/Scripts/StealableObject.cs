using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class StealableObject : MonoBehaviourExtensions
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void StealObject()
    {
        KillSelf();
    }
}
