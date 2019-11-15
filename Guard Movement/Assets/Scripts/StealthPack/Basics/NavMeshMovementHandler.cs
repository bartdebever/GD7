using System.Collections.Generic;
using Assets.Script.Guards;
using Assets.Script.MonoBehaviourExtensions;
using Assets.Scripts.QuickLoading;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.StealthPack.Basics
{
    public class NavMeshMovementHandler : MonoMovementHandler, ISaveableScript
    {
        protected NavMeshAgent NavMeshAgent;
        protected Vector3? Target;

        [Tooltip("The minimum amount of distance to register as on the point.")]
        public float MinDistance = 1.5f;

        [Header("Debugging")]
        [Tooltip("Draws a WireSphere at the current target and a ray for the path towards that target.")]
        public bool DrawGizmos;
        public Color PathDebugColor = Color.red;

        protected void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            UniqueId = GUID.Generate();
            QuickSaveStorage.Get.AddScript(this);
        }

        /// <inheritdoc />
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

            // Go over all corners of the path from the NavMesh agent and draw
            // a line between them to visualize the path that the Agent will take.
            for (var i = 0; i < NavMeshAgent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(NavMeshAgent.path.corners[i], NavMeshAgent.path.corners[i + 1], PathDebugColor);
            }
        }

        /// <inheritdoc />
        public override void Tick(GameObject gameObject)
        {
            // Check if the NavMesh Agent isn't behind on the target for some reason.
            // If there is a mismatch in the target and the destination.
            // Set it again.
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
                // For some reason there is no target yet, could be on startup.
                // Gets the next target.
                SkipTarget();
            }
        }

        /// <inheritdoc />
        public override void SkipTarget()
        {
            // Set the target to be the next in the movement pattern.
            // Replaces the current target no matter the situation.
            Target = MovementPattern.GetNextTarget();

            // Set the new destination for the NavMesh Agent.
            NavMeshAgent.destination = Target.Value;
        }

        public virtual Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
                {nameof(Target), Target}
            };
        }

        public virtual void Load(Dictionary<string, object> saveState)
        {
            Target = (Vector3?) saveState[nameof(Target)];

            if (Target.HasValue)
            {
                NavMeshAgent.destination = Target.Value;
            }
        }

        public GUID UniqueId { get; protected set; }
    }
}
