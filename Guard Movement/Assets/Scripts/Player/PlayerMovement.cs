using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    private Rigidbody _rigidbody;

    private Dictionary<KeyCode, Action> _actions;

    [SerializeField]
    private float _interactRadius = 2.5f;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _actions = new Dictionary<KeyCode, Action>
        {
            {KeyCode.E, StealObjects}
        };
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Game.IsPaused)
        {
            return;
        }

        ExecuteMovement();

        foreach (var keyValuePair in _actions)
        {
            if (Input.GetKeyDown(keyValuePair.Key))
            {
                keyValuePair.Value.Invoke();
            }
        }
    }

    private void StealObjects()
    {
        var hitColliders = Physics.OverlapSphere(gameObject.transform.position, _interactRadius);

        var stealableObjects = hitColliders
            .Where(hitCollider => hitCollider.gameObject.GetComponent<StealableObject>() != null)
            .Select(hitCollider => hitCollider.GetComponent<StealableObject>());

        foreach (var stealableObject in stealableObjects)
        {
            stealableObject.StealObject();
        }
    }

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
