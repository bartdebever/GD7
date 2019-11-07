using System.Collections;
using System.Collections.Generic;
using Assets.Script.Suspicious;
using UnityEngine;

public class AlertBehavior : MonoBehaviour
{
    [SerializeField]
    public bool Detecting { get; set; } = true;

    public bool DrawRay;

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

        var guard = GetComponentInParent<CustomGuard>();

        // Set up the target and origin for the raycast.
        // The raycast goes from the player to the guard because that works
        // and the other way around it doesn't. No clue why.
        var origin = suspiciousObject.gameObject.transform.position;

        // Subtracting the origin from the target will provide the direction
        // to shoot the raycast in.
        var target = transform.parent.position - origin;

        // Raycast from the origin towards the target.
        Physics.Raycast(origin, target, out var keyCastOut);

        // If nothing was hit, no action is required.
        if (keyCastOut.transform == null)
        {
            return;
        }

        if (DrawRay)
        {
            Debug.DrawRay(origin, target, Color.cyan, 2f);
            Debug.Log($"Vision Raycast hit \"{keyCastOut.transform.gameObject.name}");
        }

        // If something is hit, check if it's the guard.
        // If it is, alert the guard, if not do nothing. 
        if (keyCastOut.transform.gameObject == guard.gameObject)
        {
            guard.ChangeState(suspiciousObject);
        }
    }
}
