using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor.Android;
using UnityEngine;

[RequireComponent(typeof(BasicMovement))]
[RequireComponent(typeof(AlertBehavior))]
[RequireComponent(typeof(Rigidbody))]
public class Guard : MonoBehaviour
{
    private readonly Dictionary<GameObject, Vector3> _spottedObjects = new Dictionary<GameObject, Vector3>();
    private Rigidbody _rigidbody = null;
    private GameObject _overrideTarget = null;
    private BasicMovement _basicMovement;

    [SerializeField] private int _personalAlertLevel = 0;

    public void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (_overrideTarget == null)
        {
            return;
        }

        if (MovementHelper.IsNotInRange(gameObject, _overrideTarget.transform.position, 5f))
        {
            MovementHelper.Move(_rigidbody, _overrideTarget.transform.position, 7f);
        }
    }

    public void EnterAlert(GameObject spottedObject, int alertLevel)
    {
        _personalAlertLevel += alertLevel;

        ToggleSearching(false);
        //Debug.Log($"{gameObject} detected a suspicious item: {spottedObject}");

        // If the dictionary already contains the object found, update it's position in the list.
        if (_spottedObjects.ContainsKey(gameObject))
        {
            _spottedObjects[spottedObject] = spottedObject.transform.position;
        }
        else
        {
            _spottedObjects.Add(spottedObject, spottedObject.transform.position);
        }


        var guardManager = GetComponentInParent<GuardManager>();
        guardManager.AlertObject(spottedObject);
    }

    public void GetInformed(GameObject toInspectLocation)
    {
        _overrideTarget = toInspectLocation;

        ToggleSearching(false);
    }

    private void ToggleSearching(bool state)
    {

        var detectionScript = this.GetComponentInChildren<AlertBehavior>();
        detectionScript.Detecting = state;

        var moveComponent = gameObject.GetComponent<BasicMovement>();
        moveComponent.enabled = state;
    }
}
