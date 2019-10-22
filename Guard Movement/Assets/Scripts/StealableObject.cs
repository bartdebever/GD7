using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts;
using Assets.Scripts.Quickloading;
using UnityEditor;
using UnityEngine;

public class StealableObject : MonoBehaviourExtensions, ISaveableScript
{
    public Material HighlightMaterial;
    private MovementHelper _movementHelper;
    private Material _initialMaterial;
    private Renderer _renderer;
    private bool _isInRange;
    private bool _isTaken;

    // Start is called before the first frame update
    private void Start()
    {
        UniqueId = GUID.Generate();
        QuicksaveStorage.Get.AddScript(this);
        _movementHelper = new MovementHelper(gameObject);
        _renderer = GetComponent<Renderer>();
        _initialMaterial = _renderer.material;
    }

    private void Update()
    {
        if (Game.IsPaused)
        {
            return;
        }

        // Check if the player is in range of the object.
        // Make sure that the player was not in the range already to not 
        // keep setting the material.
        if (!_movementHelper.IsNotInRange(Game.PlayerObject.transform.position, 2.5f) && !_isInRange)
        {
            _renderer.material = HighlightMaterial;
            _isInRange = true;
        }
        // Only perform this if the first check fails and the player is in range
        else if (_movementHelper.IsNotInRange(Game.PlayerObject.transform.position, 2.5f) && _isInRange)
        {
            _renderer.material = _initialMaterial;
            _isInRange = false;
        }
    }

    public void StealObject()
    {
        if (_isTaken)
        {
            return;
        }

        var otherRenderer = gameObject.GetComponent<Renderer>();
        otherRenderer.enabled = false;

        _isTaken = true;

    }

    public Dictionary<string, object> Save()
    {
        var saveState = new Dictionary<string, object>
        {
            ["taken"] = _isTaken
        };

        return saveState;
    }

    public void Load(Dictionary<string, object> saveState)
    {
        _isTaken = (bool) saveState["taken"];
        var otherRenderer = gameObject.GetComponent<Renderer>();
        otherRenderer.enabled = !_isTaken;
    }

    public GUID UniqueId { get; private set; }
}
