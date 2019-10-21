using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class StealableObject : MonoBehaviourExtensions
{
    public Material HightlightMaterial;

    private Material _initialMaterial;
    private Renderer _renderer;
    private bool _isInRange;

    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _initialMaterial = _renderer.material;
    }

    private void Update()
    {
        // Check if the player is in range of the object.
        // Make sure that the player was not in the range already to not 
        // keep setting the material.
        if (!MovementHelper.IsNotInRange(gameObject, Game.PlayerObject.transform.position, 2.5f) && !_isInRange)
        {
            _renderer.material = HightlightMaterial;
            _isInRange = true;
        }
        // Only perform this if the first check fails and the player is in range
        else if (MovementHelper.IsNotInRange(gameObject, Game.PlayerObject.transform.position, 2.5f) && _isInRange)
        {
            _renderer.material = _initialMaterial;
            _isInRange = false;
        }
    }

    public void StealObject()
    {
        KillSelf();
    }
}
