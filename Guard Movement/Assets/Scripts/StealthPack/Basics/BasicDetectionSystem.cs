using Assets.Script.Events;
using Assets.Script.MonoBehaviourExtensions;
using Assets.Script.Suspicious;
using UnityEngine.Events;

namespace Assets.Script.Basics
{
    /// <summary>
    /// A basic detection system that uses Unity Events.
    /// </summary>
    public class BasicDetectionSystem : MonoDetectionSystem
    {
        protected readonly UnityEvent<SuspiciousObject> DetectionEvent = new SuspiciousUnityEvent();

        /// <inheritdoc />
        public override void AddListener(UnityAction<SuspiciousObject> action)
        {
            DetectionEvent.AddListener(action);
        }
    }
}
