﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    private List<Guard> _guards;
    [SerializeField] private int _maxAlertGuards;

    public void Start()
    {
        _guards = GetComponentsInChildren<Guard>().ToList();
    }

    public void AlertObject(GameObject objectLocation)
    {
        var guardsToAlert = _guards.OrderBy(guard =>
                Vector3.Distance(guard.gameObject.transform.position, objectLocation.transform.position))
            .Take(_maxAlertGuards);

        foreach (var guard in guardsToAlert)
        {
            guard.GetInformed(objectLocation);
        }
    }
}