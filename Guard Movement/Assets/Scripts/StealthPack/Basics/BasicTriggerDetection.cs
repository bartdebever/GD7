using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Basics;
using Assets.Script.Guards;
using Assets.Script.Suspicious;
using Assets.Scripts.QuickLoading;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.StealthPack.Basics
{
    public class BasicTriggerDetection : BasicDetectionSystem, ISaveableScript
    {
        [SerializeField]
        public bool Detecting { get; set; } = true;

        /// <summary>
        /// Defines if a Ray should be drawn where the raycast is drawn.
        /// </summary>
        public bool DrawRay;

        /// <summary>
        /// The color of the given ray.
        /// </summary>
        public Color RayColor = Color.cyan;

        /// <summary>
        /// The duration that the ray stays out.
        /// </summary>
        public float RayTime = 2f;

        protected virtual void Start()
        {
            UniqueId = GUID.Generate();
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!Detecting)
            {
                return;
            }

            var suspiciousObject = otherCollider.gameObject.GetComponent<SuspiciousObject>();
            if (suspiciousObject == null)
            {
                return;
            }

            var guard = GetComponentInParent<Guard>();

            // Set up the target and origin for the raycast.
            // The raycast goes from the player to the guard because that works
            // and the other way around it doesn't. No clue why.
            var origin = suspiciousObject.gameObject.transform.position;

            // Subtracting the origin from the target will provide the direction
            // to shoot the raycast in.
            var target = transform.parent.position - origin;

            // Raycast from the origin towards the target.
            Physics.Raycast(origin, target, out var keyCastOut);

            // If nothing was hit, no action is required.
            if (keyCastOut.transform == null)
            {
                return;
            }

            // If the developer wants to debug the raycast of the trigger, we
            // make a visible ray and also give a log message.
            if (DrawRay)
            {
                Debug.DrawRay(origin, target, RayColor, RayTime);
                Debug.Log($"Vision Raycast hit \"{keyCastOut.transform.gameObject.name}");
            }

            // If something is hit, check if it's the guard.
            // If it is, alert the guard, if not do nothing. 
            if (keyCastOut.transform.gameObject == guard.gameObject)
            {
                DetectionEvent?.Invoke(suspiciousObject);

                guard.ChangeState(suspiciousObject);
            }
        }

        public virtual Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
                {nameof(Detecting), Detecting}
            };
        }

        public virtual void Load(Dictionary<string, object> saveState)
        {
            Detecting = (bool) saveState[nameof(Detecting)];
        }

        public GUID UniqueId { get; protected set; }
    }
}
