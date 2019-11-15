using System.Collections.Generic;
using System.Linq;
using Assets.Script.MonoBehaviourExtensions;
using Assets.Scripts.QuickLoading;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Basics
{
    /// <summary>
    /// Provides a very basic movement pattern easily implemented by provided
    /// <see cref="Vector3"/> objects as a position.
    /// </summary>
    public class BasicMovementPattern : MonoMovementPattern, ISaveableScript
    {
        /// <summary>
        /// The points that the guard will move to.
        /// </summary>
        public List<Vector3> Pattern;

        /// <summary>
        /// The current state in which the guard is within the array.
        /// </summary>
        protected int CurrentState;

        /// <summary>
        /// A backup of the original pattern that is loaded at the start.
        /// Used for resetting the pattern if needed.
        /// </summary>
        protected List<Vector3> PatternBackup;

        [Tooltip("The maximum amount of rotations in the pattern to be done before it resets.")]
        public int MaxRotations = 2;

        protected int Rotations;

        /// <summary>
        /// Defines if the Gizmos should be drawn that represent the pattern.
        /// </summary>
        [Tooltip("Visualizes the Pattern using Gizmo spheres")]
        [Header("Debugging")]
        public bool DrawGizmos;

        public Color GizmoColor = Color.yellow;

        protected void Start()
        {
            PatternBackup = Pattern;
            UniqueId = GUID.Generate();
            QuickSaveStorage.Get.AddScript(this);
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
                CurrentState = 0;
                if (++Rotations >= MaxRotations)
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
            Rotations = 0;
            CurrentState = 0;
            Pattern = PatternBackup;
        }

        protected void OnDrawGizmos()
        {
            if (!DrawGizmos)
            {
                return;
            }

            Gizmos.color = GizmoColor;
            foreach (var position in Pattern)
            {
                Gizmos.DrawSphere(position, 1);
            }
            
        }

        public Dictionary<string, object> Save()
        {
            var stateDictionary = new Dictionary<string, object>()
            {
                {nameof(Pattern), Pattern},
                {nameof(Rotations), Rotations},
                {nameof(CurrentState), CurrentState}
            };

            return stateDictionary;
        }

        public void Load(Dictionary<string, object> saveState)
        {
            Pattern = (List<Vector3>) saveState[nameof(Pattern)];
            Rotations = (int) saveState[nameof(Rotations)];
            CurrentState = (int) saveState[nameof(CurrentState)];
        }

        public GUID UniqueId { get; protected set; }
    }
}
