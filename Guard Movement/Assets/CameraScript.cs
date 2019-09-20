using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject ParentObject;
    private bool _introMove = true;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_introMove)
        {
            if (MovementHelper.IsNotInRange(gameObject, ParentObject.gameObject.transform.position, 1f))
            {
                MovementHelper.Move(_rigidbody, ParentObject.gameObject.transform.position, 10f);
            }
            else
            {
                _introMove = false;
                var rotation = gameObject.transform.localEulerAngles;
                rotation.y = 0;
                gameObject.transform.localEulerAngles = rotation;

                gameObject.transform.SetParent(ParentObject.transform);
                Destroy(_rigidbody);
                Game.IsPaused = false;
                Debug.Log("Starting game.");
            }
        }
    }
}
