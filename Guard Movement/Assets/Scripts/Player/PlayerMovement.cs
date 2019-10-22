using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// The speed at which the character moves.
    /// </summary>
    public float Speed;

    /// <summary>
    /// Reference to the Rigidbody, used more often to move the player.
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// The velocity that was applied before the player was paused.
    /// </summary>
    private Vector3? _pausedVelocity;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // If the game is paused and the velocity is already saved, stop this method.
        if (Game.IsPaused && _pausedVelocity.HasValue)
        {
            return;
        }
        // If the game is paused and there is no value stored
        // Store the velocity currently applied to the player and reset the players
        // current velocity.
        else if (Game.IsPaused)
        {
            _pausedVelocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector3();
        }
        else if (!Game.IsPaused && _pausedVelocity.HasValue)
        {
            _rigidbody.velocity = _pausedVelocity.Value;
            _pausedVelocity = null;
        }

        ExecuteMovement();
    }

    /// <summary>
    /// Executes the movement for the given key by the player.
    /// </summary>
    private void ExecuteMovement()
    {
        var force = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            force.z += Speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            force.x -= Speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            force.x += Speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            force.z -= Speed;
        }

        _rigidbody.AddForce(force);
    }
}
