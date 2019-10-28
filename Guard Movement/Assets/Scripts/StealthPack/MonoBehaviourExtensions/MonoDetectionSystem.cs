using Assets.Script.Interface;
using Assets.Script.Suspicious;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script.MonoBehaviourExtensions
{
    /// <summary>
    /// Implements the <see cref="IDetectionSystem"/> interface while extending
    /// <see cref="MonoBehaviour"/>.
    /// </summary>
    public abstract class MonoDetectionSystem : MonoBehaviour, IDetectionSystem
    {
        /// <inheritdoc />
        public abstract void AddListener(UnityAction<SuspiciousObject> action);
    }
}
