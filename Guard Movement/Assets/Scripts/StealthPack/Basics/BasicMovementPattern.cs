using System.Collections.Generic;
using System.Linq;
using Assets.Script.MonoBehaviourExtensions;
using UnityEngine;

namespace Assets.Script.Basics
{
    /// <summary>
    /// Provides a very basic movement pattern easily implemented by provided
    /// <see cref="Vector3"/> objects as a position.
    /// </summary>
    public class BasicMovementPattern : MonoMovementPattern
    {
        /// <summary>
        /// The points that the guard will move to.
        /// </summary>
        public List<Vector3> Pattern;

        /// <summary>
        /// Defines if the Gizmos should draw where the guard is looking at.
        /// </summary>
        public bool DrawGizmos;

        /// <summary>
        /// The current state in which the guard is within the array.
        /// </summary>
        protected int CurrentState;

        /// <summary>
        /// A backup of the original pattern that is loaded at the start.
        /// Used for resetting the pattern if needed.
        /// </summary>
        protected List<Vector3> PatternBackup;

        protected int MaxRotations = 2;

        protected int Rotations;

        protected void Start()
        {
            PatternBackup = Pattern;
        }

        /// <inheritdoc />
        public override Vector3 GetCurrentTarget()
        {
            return Pattern[CurrentState];
        }

        /// <inheritdoc />
        public override Vector3 GetNextTarget()
        {
            if (++CurrentState >= Pattern.Count)
            {
                Rotations++;
                CurrentState = 0;
                if (Rotations >= MaxRotations)
                {
                    Pattern = PatternBackup;
                    Rotations = 0;
                }
            }

            return Pattern[CurrentState];
        }

        public override void SetNewPattern(IEnumerable<Vector3> pattern)
        {
            if (pattern == null)
            {
                ResetPattern();
                return;
            }

            Rotations = 0;
            CurrentState = 0;

            Pattern = pattern.ToList();
        }

        public override void ResetPattern()
        {
            Pattern = PatternBackup;
        }

        protected void OnDrawGizmos()
        {
            if (!DrawGizmos)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            foreach (var position in Pattern)
            {
                Gizmos.DrawSphere(position, 1);
            }
            
        }
    }
}
