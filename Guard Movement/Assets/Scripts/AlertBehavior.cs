using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBehavior : MonoBehaviour
{
    [SerializeField]
    public bool Detecting { get; set; } = true;

    public void OnTriggerEnter(Collider otherCollider)
    {
        if (!Detecting)
        {
            return;
        }

        var suspiciousObject = otherCollider.gameObject.GetComponent<SuspiciousObject>();
        if (suspiciousObject == null)
        {
            return;
        }

        var guard = this.GetComponentInParent<Guard>();
        guard.EnterAlert(otherCollider.gameObject, suspiciousObject.AlertLevel);
    }
}
