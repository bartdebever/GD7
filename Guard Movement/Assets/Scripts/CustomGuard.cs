using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Basics;
using Assets.Script.Guards;
using Assets.Script.Suspicious;
using Assets.Scripts;
using Assets.Scripts.Guarding;
using Assets.Scripts.QuickLoading;
using Assets.Scripts.Statistics;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AlertBehavior))]
[RequireComponent(typeof(Rigidbody))]
public class CustomGuard : Guard, ISaveableScript
{
    [SerializeField] private List<Vector3> _targets = new List<Vector3>();
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _waitingTime = 0;

    private readonly Dictionary<GameObject, Vector3> _spottedObjects = new Dictionary<GameObject, Vector3>();
    private MovementHelper _movementHelper;
    private Rigidbody _rigidbody;
    private GameObject _overrideTarget;

    private float _currentWaiting;
    private int _targetCounter;
    private Vector3? _target;

    private GuardModes _state;

    public void Start()
    {
        UniqueId = GUID.Generate();
        QuickSaveStorage.Get.AddScript(this);
        _movementHelper = new MovementHelper(gameObject);
        _rigidbody = GetComponentInParent<Rigidbody>();
        _state = GuardModes.Route;
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
        _state = GuardModes.Searching;

        ToggleSearching();
    }

    private void OnDrawGizmos()
    {
        if (_target == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(_target.Value, 2f);
    }

    private void ToggleSearching()
    {
        var detectionScript = this.GetComponentInChildren<AlertBehavior>();
        detectionScript.Detecting = _state != GuardModes.Searching;
    }

    private void ExecuteBasicMovement()
    {
        if (!_targets.Any())
        {
            // If there are no targets, don't execute anything.
            return;
        }

        // Currently not at the target move towards it.
        if (_target.HasValue && _movementHelper.IsNotInRange(_target.Value, 0.3f))
        {
            _movementHelper.Move(_rigidbody, _target.Value, _speed);
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
        // If there is no override target, there is no chase going on.
        // Execute the basic movement and stop the execution of this method.
        if (_overrideTarget == null)
        {
            ExecuteBasicMovement();
            return;
        }

        // If we aren't next to the target.
        if (_movementHelper.IsNotInRange(_overrideTarget.transform.position, 5f))
        {
            // If it is not within the max distance of the guards "vision"
            if (_movementHelper.IsNotInRange(_overrideTarget.transform.position, 20f))
            {
                // Stop chasing the target and go back to the normal route.
                _overrideTarget = null;
                _state = GuardModes.Route;
                ChangeState(GuardVariables.MinimumAlert);
                ToggleSearching();
                return;
            }

            // Move towards the target.
            _movementHelper.Move(_rigidbody, _overrideTarget.transform.position, 7f);
        }
        else
        {
            // Inspect the target and decrease or increase the alert.
            if (_currentWaiting >= _waitingTime)
            {
                HandleSuspiciousTarget();
                _currentWaiting = 0f;
            }
            else
            {
                _currentWaiting += Time.deltaTime;
            }
            
        }
    }

    private void HandleSuspiciousTarget()
    {
        var suspiciousObject = _overrideTarget.GetComponent<SuspiciousObject>();
        suspiciousObject.AlertIncrease = -suspiciousObject.AlertIncrease;

        Destroy(_overrideTarget);
        _overrideTarget = null;

        ChangeState(suspiciousObject);
    }

    public Dictionary<string, object> Save()
    {
        var saveState = new Dictionary<string, object>();

        saveState.Add("position", gameObject.transform.position);
        saveState.Add("targetCounter", _targetCounter);
        saveState.Add("target", _target);
        saveState.Add("state", _state);

        return saveState;
    }

    public void Load(Dictionary<string, object> saveState)
    {
        gameObject.transform.position = (Vector3) saveState["position"];
        _targetCounter = (int) saveState["targetCounter"];
        _target = (Vector3?) saveState["target"];
        _state = (GuardModes) saveState["state"];

        ToggleSearching();
    }

    public GUID UniqueId { get; private set; }
    public override Dictionary<float, Action<GameObject>> StateActions => new Dictionary<float, Action<GameObject>>()
    {
        {GuardVariables.MinimumAlert, (spottedObject) =>
        {
            ToggleSearching();
            _overrideTarget = null;
            _state = GuardModes.Route;
        }},
        {GuardVariables.MaximumAlert, (spottedObject) =>
        {
            ToggleSearching();

            // If the dictionary already contains the object found, update it's position in the list.
            if (_spottedObjects.ContainsKey(spottedObject))
            {
                // Update the objects position with a new position.
                _spottedObjects[spottedObject] = spottedObject.transform.position;
            }
            else
            {
                // Add the object to the dictionary.
                _spottedObjects.Add(spottedObject, spottedObject.transform.position);
            }

            var guardManager = GetComponentInParent<GuardManager>();
            guardManager.AlertObject(spottedObject);

            _overrideTarget = spottedObject;
            _state = GuardModes.Searching;
        }}
    };
}
