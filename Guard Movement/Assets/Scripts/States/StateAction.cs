using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class to invoke actions with a state machine.
/// </summary>
public abstract class StateAction
{
    /// <summary>
    /// Executes the action that this state may belong to.
    /// </summary>
    public abstract void Invoke(GameObject invokedObject);
}
