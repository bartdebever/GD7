using System;
using Assets.Script.Guards;
using Assets.Script.MonoBehaviourExtensions;
using UnityEngine;

namespace Assets.Scripts.Guarding
{
    public class GuardMovement : MonoMovementHandler
    {
        private Rigidbody _rigidbody;
        private MovementHelper _movementHelper;
        private Vector3? _target;
        [SerializeField] private float _speed;
        private float _waitingTime;
        private float _currentWaiting;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movementHelper = new MovementHelper(gameObject);
        }

        public override void SetTarget(Vector3 target, GuardData guardData)
        {
            throw new System.NotImplementedException();
        }
        
        private void OnDrawGizmos()
        {
            if (_target == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(_target.Value, 2f);
        }

        public override void Tick(GameObject gameObject)
        {
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
                    _target = MovementPattern.GetNextTarget();
                    _currentWaiting = 0f;
                }
                else
                {
                    _currentWaiting += Time.deltaTime;
                }
            }
        }
    }
}