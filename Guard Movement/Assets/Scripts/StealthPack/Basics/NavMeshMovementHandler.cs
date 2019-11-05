using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Guards;
using Assets.Script.MonoBehaviourExtensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.StealthPack.Basics
{
    public class NavMeshMovementHandler : MonoMovementHandler
    {
        protected float MinDistance = 1.5f;
        protected NavMeshAgent NavMeshAgent;
        protected Vector3? Target;
        public bool DrawGizmos;

        protected void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override void SetTarget(Vector3 target, GuardData guardData)
        {
            Target = target;
        }

        private void OnDrawGizmos()
        {
            if (Target == null || !DrawGizmos)
            {
                return;
            }

            Gizmos.DrawWireSphere(Target.Value, 2f);

            for (int i = 0; i < NavMeshAgent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(NavMeshAgent.path.corners[i], NavMeshAgent.path.corners[i + 1], Color.red);
            }
        }

        public override void Tick(GameObject gameObject)
        {
            if (Target.HasValue && !(NavMeshAgent.destination == Target.Value))
            {
                NavMeshAgent.destination = Target.Value;
            }

            // Currently at the target move towards it.
            if (Target.HasValue && Vector3.Distance(gameObject.transform.position, Target.Value) <= MinDistance)
            {
                // Set the position to be perfect so we know the same path will be followed.
                gameObject.transform.position = Target.Value;

                // We are at the target, time to pick the next.
                SkipTarget();
            }
            else if (!Target.HasValue)
            {
                SkipTarget();
            }
        }

        public override void SkipTarget()
        {
            Target = MovementPattern.GetNextTarget();
            NavMeshAgent.destination = Target.Value;
        }
    }
}
