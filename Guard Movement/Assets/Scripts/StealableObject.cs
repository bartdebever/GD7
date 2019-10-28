using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts;
using Assets.Scripts.QuickLoading;
using UnityEditor;
using UnityEngine;

public class StealableObject : MonoBehaviourExtensions, ISaveableScript
{
    /// <summary>
    /// The material to highlight the object with to indicate it can be interacted with.
    /// </summary>
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
        QuickSaveStorage.Get.AddScript(this);
        _movementHelper = new MovementHelper(gameObject);
        _renderer = GetComponent<Renderer>();
        _initialMaterial = _renderer.material;
    }

    private void Update()
    {
        if (Game.IsPaused || _isTaken)
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
            Game.UI.SetBottomText("Press E to steal");
        }
        // Only perform this if the first check fails and the player is in range
        else if (_movementHelper.IsNotInRange(Game.PlayerObject.transform.position, 2.5f) && _isInRange)
        {
            _renderer.material = _initialMaterial;
            _isInRange = false;
            Game.UI.HideBottom();
        }
    }

    public void StealObject()
    {
        // If the object is already taken, nothing should be executed.
        if (_isTaken)
        {
            return;
        }

        // Get the render and deactivate it.
        // This hides the material but still makes it visible.
        var otherRenderer = gameObject.GetComponent<Renderer>();
        otherRenderer.enabled = false;

        Game.UI.HideBottom();

        _isTaken = true;

    }

    #region QuickSaving
    public Dictionary<string, object> Save()
    {
        var saveState = new Dictionary<string, object>
        {
            [nameof(_isTaken)] = _isTaken
        };

        return saveState;
    }

    public void Load(Dictionary<string, object> saveState)
    {
        _isTaken = (bool)saveState[nameof(_isTaken)];
        var otherRenderer = gameObject.GetComponent<Renderer>();
        otherRenderer.enabled = !_isTaken;
    }

    public GUID UniqueId { get; private set; }
    #endregion
}
