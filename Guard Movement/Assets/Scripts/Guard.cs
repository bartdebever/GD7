using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Guarding;
using UnityEngine;

[RequireComponent(typeof(AlertBehavior))]
[RequireComponent(typeof(Rigidbody))]
public class Guard : SimpleStateMachine
{
    [SerializeField] private List<Vector3> _targets = new List<Vector3>();
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _waitingTime = 0;

    private readonly Dictionary<GameObject, Vector3> _spottedObjects = new Dictionary<GameObject, Vector3>();
    private Rigidbody _rigidbody = null;
    private GameObject _overrideTarget = null;

    private float _currentWaiting;
    private int _targetCounter;
    private Vector3? _target;

    private GuardModes _state;

    public void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        _state = GuardModes.Route;

        StateActions = new Dictionary<int, Action<GameObject>>()
        {
            {0, (spottedObject) =>
            {
                ToggleSearching(true);
                _overrideTarget = null;
                _state = GuardModes.Route;
            }},
            {20, (spottedObject) =>
            {
                ToggleSearching(false);

                // If the dictionary already contains the object found, update it's position in the list.
                if (_spottedObjects.ContainsKey(spottedObject))
                {
                    _spottedObjects[spottedObject] = spottedObject.transform.position;
                }
                else
                {
                    _spottedObjects.Add(spottedObject, spottedObject.transform.position);
                }

                var guardManager = GetComponentInParent<GuardManager>();
                guardManager.AlertObject(spottedObject);

                _overrideTarget = spottedObject;
                _state = GuardModes.Searching;
            }}
        };
    }

    public void FixedUpdate()
    {
        if (Game.IsPaused)
        {
            return;
        }

        switch (_state)
        {
            case GuardModes.Route:
                ExecuteBasicMovement();
                break;
            case GuardModes.Searching:
                ChaseTarget();
                break;
            default:
            case GuardModes.Attacking:
                // TODO Implement method.
                break;

        }
    }

    public void GetInformed(GameObject toInspectLocation)
    {
        _overrideTarget = toInspectLocation;

        ToggleSearching(false);
    }

    public void OnDrawGizmos()
    {
        if (_target == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(_target.Value, 2f);
    }

    private void ToggleSearching(bool state)
    {
        var detectionScript = this.GetComponentInChildren<AlertBehavior>();
        detectionScript.Detecting = state;
    }

    private void ExecuteBasicMovement()
    {
        if (!_targets.Any())
        {
            // If there are no targets, don't execute anything.
            return;
        }

        // Currently not at the target move towards it.
        if (_target.HasValue && MovementHelper.IsNotInRange(gameObject, _target.Value, 0.3f))
        {
            MovementHelper.Move(_rigidbody, _target.Value, _speed);
        }
        else
        {
            // Set the position to be perfect so we know the same path will be followed.
            if (_target.HasValue)
            {
                _rigidbody.MovePosition(_target.Value);
            }

            // We are at the target, time to pick the next.
            _target = null;

            if (_currentWaiting >= _waitingTime)
            {
                if (_targetCounter >= _targets.Count)
                {
                    _targetCounter = 0;
                }

                _target = _targets[_targetCounter++];
                _currentWaiting = 0f;
            }
            else
            {
                _currentWaiting += Time.deltaTime;
            }
        }
    }

    private void ChaseTarget()
    {
        if (_overrideTarget == null)
        {
            ExecuteBasicMovement();
            return;
        }

        if (MovementHelper.IsNotInRange(gameObject, _overrideTarget.transform.position, 5f))
        {
            MovementHelper.Move(_rigidbody, _overrideTarget.transform.position, 7f);
        }
        else
        {
            // Inspect the target and decrease or increase the alert.
            if (_currentWaiting >= _waitingTime)
            {
                ChangeState(null, -15);
                _currentWaiting = 0f;
            }
            else
            {
                _currentWaiting += Time.deltaTime;
            }
            
        }
    }
}
