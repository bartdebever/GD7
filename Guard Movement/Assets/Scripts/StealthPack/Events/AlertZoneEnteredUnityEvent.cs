using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script.Events
{
    /// <summary>
    /// Event to be used to alert a guard when a GameObject enters its zone.
    /// </summary>
    public class AlertZoneEnteredUnityEvent : UnityEvent<GameObject>
    {
    }
}
