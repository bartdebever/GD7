﻿using System;
using System.Collections.Generic;
using Assets.Script.Guards;
using Assets.Script.Suspicious;
using Assets.Scripts;
using Assets.Scripts.Guarding;
using Assets.Scripts.QuickLoading;
using Assets.Scripts.Statistics;
using Assets.Scripts.StealthPack.Basics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(BasicTriggerDetection))]
public class CustomGuard : Guard, ISaveableScript
{
    public Material HighlightMaterial;

    [SerializeField] private float _waitingTime = 0;

    protected readonly Dictionary<GameObject, Vector3> SpottedObjects = new Dictionary<GameObject, Vector3>();
    private IEnumerable<BasicTriggerDetection> _alertBehaviors;
    private MovementHelper _movementHelper;
    private NavMeshAgent _navMeshAgent;
    private GameObject _overrideTarget;
    private Material _initialMaterial;
    private Renderer _renderer;

    private float _currentWaiting;
    private Vector3? _target;
    private Vector3? _lastSpotted;

    private GuardModes _state = GuardModes.Route;

    protected override void Start()
    {
        base.Start();
        UniqueId = GUID.Generate();
        QuickSaveStorage.Get.AddScript(this);
        _movementHelper = new MovementHelper(gameObject);
        _navMeshAgent = GetComponentInParent<NavMeshAgent>();
        _alertBehaviors = GetComponentsInChildren<BasicTriggerDetection>();
        _renderer = GetComponent<Renderer>();
        _initialMaterial = _renderer.material;
    }

    private new void FixedUpdate()
    {
        if (Game.IsPaused)
        {
            return;
        }

        switch (_state)
        {
            case GuardModes.Route:
                MovementHandler.Tick(gameObject);
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

    private void OnDrawGizmos()
    {
        if (_lastSpotted.HasValue)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_lastSpotted.Value, 2);
        }
    }

    #region  Suspicious Target code

    public void GetInformed(GameObject toInspectLocation)
    {
        _overrideTarget = toInspectLocation;
        _state = GuardModes.Searching;

        ToggleSearching();
    }
    
    private void ToggleSearching()
    {
        var localRenderer = GetComponent<Renderer>();
        var calmState = _state == GuardModes.Route;

        localRenderer.material = calmState ? _initialMaterial : HighlightMaterial;

        _navMeshAgent.speed = calmState ? 5 : 10;
        _navMeshAgent.acceleration = calmState ? 8 : 15;

        foreach (var detectionScript in _alertBehaviors)
        {
            // If the guard is on a route, it should be trying to detect enemies.
            detectionScript.Detecting = calmState;
        }
    }
    
    private void ChaseTarget()
    {
        // If there is no override target, there is no chase going on.
        // Execute the basic movement and stop the execution of this method.
        if (_overrideTarget == null)
        {
            return;
        }

        // If we aren't next to the target.
        if (_movementHelper.IsNotInRange(_overrideTarget.transform.position, 5f))
        {
            // If it is not within the max distance of the guards "vision"
            if (_movementHelper.IsNotInRange(_overrideTarget.transform.position, 25f))
            {
                // Stop chasing the target and go back to the normal route.
                _lastSpotted = _overrideTarget.transform.position;
                GenerateNewPattern();
                _overrideTarget = null;
                _state = GuardModes.Route;
                ChangeState(GuardVariables.MaximumAlert / 2f);
                ToggleSearching();
                return;
            }

            _navMeshAgent.destination = _overrideTarget.transform.position;
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
        ToggleSearching();
    }

    #endregion

    private void GenerateNewPattern()
    {
        if (!_lastSpotted.HasValue)
        {
            return;
        }

        var lastSpotted = _lastSpotted.Value;
        var positionList = Game.PatternGenerator.GeneratePattern(lastSpotted, gameObject);

        MovementPattern.SetNewPattern(positionList);

        // Skip ahead to the next target.
        // This is to prevent the guard from walking to the last pattern.
        MovementHandler.SkipTarget();
    }

    #region QuickSave
    public override Dictionary<string, object> Save()
    {
        var saveState = base.Save();

        saveState.Add("position", gameObject.transform.position);
        saveState.Add("target", _target);
        saveState.Add("state", _state);

        return saveState;
    }

    public override void Load(Dictionary<string, object> saveState)
    {
        base.Load(saveState);
        gameObject.transform.position = (Vector3) saveState["position"];
        _target = (Vector3?) saveState["target"];
        _state = (GuardModes) saveState["state"];

        ToggleSearching();
    }
    #endregion

    #region StateMachineActions

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
            // If the dictionary already contains the object found, update it's position in the list.
            if (SpottedObjects.ContainsKey(spottedObject))
            {
                // Update the objects position with a new position.
                SpottedObjects[spottedObject] = spottedObject.transform.position;
            }
            else
            {
                // Add the object to the dictionary.
                SpottedObjects.Add(spottedObject, spottedObject.transform.position);
            }

            var guardManager = GetComponentInParent<GuardManager>();
            guardManager.AlertObject(spottedObject);

            _overrideTarget = spottedObject;
            _state = GuardModes.Searching;
            ToggleSearching();
        }}
    };

    #endregion

}
