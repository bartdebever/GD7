using System.Collections;
using System.Collections.Generic;
using Assets.Script.Suspicious;
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

        var guard = this.GetComponentInParent<CustomGuard>();

        var target = suspiciousObject.gameObject.transform.position;
        var origin = transform.parent.position - target;

        Physics.Raycast(target, origin, out var keyCastOut);

        if (keyCastOut.transform == null)
        {
            return;
        }

        Debug.Log(keyCastOut.transform.gameObject);
        Debug.DrawRay(target, origin, Color.cyan, 2f);

        if (keyCastOut.transform.gameObject == guard.gameObject)
        {
            guard.ChangeState(suspiciousObject);
        }
    }
}
