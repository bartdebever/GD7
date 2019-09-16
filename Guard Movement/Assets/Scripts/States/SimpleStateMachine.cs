using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public abstract class SimpleStateMachine : MonoBehaviour
{
    [SerializeField] private int _alertLevel;

    [SerializeField] private int _maxAlertLevel;

    public Dictionary<int, Action<GameObject>> StateActions;

    public void ChangeState(GameObject spottedObject, int alertLevel)
    {
        _alertLevel += alertLevel;
        if (_alertLevel > _maxAlertLevel)
        {
            _alertLevel = _maxAlertLevel;
        }
        var closestIndex = GetClosestIndex();
        try
        {
            StateActions[closestIndex].Invoke(spottedObject);
        }
        catch (ArgumentException)
        {
            Debug.Log($"No action found for state {closestIndex}");
        }
    }

    /// <summary>
    /// Gets the closest dictionary index to the current <see cref="_alertLevel"/>.
    /// </summary>
    /// <returns></returns>
    private int GetClosestIndex()
    {
        var keys = StateActions.Keys.OrderBy(key => key).ToList();

        for (var i = keys.Count - 1; i >= 0; i--)
        {
            if (_alertLevel >= keys[i])
            {
                Debug.Log(keys[i]);
                return keys[i];
            }
        }

        return 0;
    }
        
}
