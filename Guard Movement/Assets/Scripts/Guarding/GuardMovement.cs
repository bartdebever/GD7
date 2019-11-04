using System;
using Assets.Script.Guards;
using Assets.Script.MonoBehaviourExtensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Guarding
{
    public class GuardMovement : MonoMovementHandler
    {
        private NavMeshAgent _navMeshAgent;
        private MovementHelper _movementHelper;
        private Vector3? _target;

        [SerializeField] private float _speed;
        private float _waitingTime;
        private float _currentWaiting;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
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
            if (_target.HasValue && !(_navMeshAgent.destination == _target.Value))
            {
                _navMeshAgent.destination = _target.Value;
            }

            // Currently not at the target move towards it.
            if (_target.HasValue && _movementHelper.IsNotInRange(_target.Value, 0.3f))
            {
                
            }
            else
            {
                // Set the position to be perfect so we know the same path will be followed.
                if (_target.HasValue)
                {
                    gameObject.transform.position = _target.Value;
                }

                // We are at the target, time to pick the next.
                _target = null;

                if (_currentWaiting >= _waitingTime)
                {
                    _target = MovementPattern.GetNextTarget();
                    _navMeshAgent.destination = _target.Value;
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