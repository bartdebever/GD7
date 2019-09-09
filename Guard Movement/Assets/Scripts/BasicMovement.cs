using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField]private List<Vector3> _targets = new List<Vector3>();
    [SerializeField]private float _speed = 5f;
    [SerializeField]private float _waitingTime = 0;

    private float _currentWaiting;
    private int _targetCounter;
    private Vector3? _target;

    private Rigidbody _rigidbody = null;
    // Start is called before the first frame update
    public void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();

        if (_rigidbody == null)
        {
            Debug.LogError("No RigidBody found on the object to be moved.");
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
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

    public void OnDrawGizmos()
    {
        if (_target == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(_target.Value, 2f);
    }
}
