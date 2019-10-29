using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject ParentObject;
    private bool _introMove = true;
    private MovementHelper _movementHelper;
    private Rigidbody _rigidbody;

    /// <summary>
    /// Defines if the intro should be done or the camera should be set in place instantly.
    /// </summary>
    public bool DoIntro;

    // Start is called before the first frame update
    void Start()
    {
        _movementHelper = new MovementHelper(gameObject);
        _rigidbody = GetComponent<Rigidbody>();

        if (!DoIntro)
        {
            FinishIntro();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_introMove)
        {
            return;
        }

        // Check if the camera is not in the provided range of an object.
        if (_movementHelper.IsNotInRange(ParentObject.gameObject.transform.position, 1f))
        {
            // If not, move the camera to that object.
            _movementHelper.Move(_rigidbody, ParentObject.gameObject.transform.position, 10f);
        }
        else
        {
            // The camera is close enough and the intro should end.
            FinishIntro();
        }
    }

    /// <summary>
    /// Finishes the intro camera shot by placing the camera in its final position
    /// and destroying the objects that should not exist anymore.
    /// </summary>
    private void FinishIntro()
    {
        // Set the position of the camera to be exactly right.
        var position = gameObject.transform.position;
        position.x = ParentObject.gameObject.transform.position.x;
        position.z = ParentObject.gameObject.transform.position.z;

        gameObject.transform.position = position;

        // Sets the variable to do the movement sequence to false.
        _introMove = false;

        transform.LookAt(ParentObject.gameObject.transform);

        // The camera will move towards the player and rotate to get there.
        // When the game actually starts, this rotation should not be there anymore.
        var rotation = gameObject.transform.localEulerAngles;
        rotation.y = 0;
        gameObject.transform.localEulerAngles = rotation;

        // Set the camera as the child of the object that will be the player.
        // This makes it so the camera will follow the player.
        gameObject.transform.SetParent(ParentObject.transform);

        // Destroy the rigidbody component as it will hinder the camera from moving
        // with the player.
        Destroy(_rigidbody);

        // This class isn't needed anymore, lets save memory.
        _movementHelper = null;

        Game.IsPaused = false;
    }
}
