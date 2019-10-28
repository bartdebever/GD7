using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Suspicious;
using UnityEngine;

namespace Assets.Script.Basics
{
    /// <summary>
    /// Basic implementation of a state machine like behaviour.
    /// </summary>
    public abstract class BasicStateMachine : MonoBehaviour
    {
        /// <summary>
        /// The amount of alert the guard is currently in.
        /// </summary>
        [SerializeField] private float _alertLevel;

        /// <summary>
        /// The maximum amount of alert the guard can get.
        /// </summary>
        [SerializeField] private float _maxAlertLevel;

        /// <summary>
        /// The actions that are performed with some states.
        /// </summary>
        public abstract Dictionary<float, Action<GameObject>> StateActions { get; }

        /// <summary>
        /// Forced the state to be the given <paramref name="state"/>.
        /// Does not call new behaviour.
        /// </summary>
        /// <param name="state">The value that the state should now have.</param>
        protected void ChangeState(float state)
        {
            _alertLevel = state;

            // The state can not be above the maximum or bellow 0.
            if (_alertLevel > _maxAlertLevel)
            {
                _alertLevel = _maxAlertLevel;
            }
            else if (_alertLevel < 0)
            {
                _alertLevel = 0;
            }
        }

        /// <summary>
        /// Changes the state by using a <see cref="SuspiciousObject"/> that
        /// was spotted by a guard.
        /// </summary>
        /// <param name="spottedObject">
        /// The object seen by the guard.
        /// </param>
        /// <remarks>
        /// The behaviour method will be called that is closest to the provided
        /// value for <paramref name="spottedObject"/>.<see cref="SuspiciousObject.AlertIncrease"/>.
        /// This will only be rounded down.
        ///
        /// Example:
        ///
        /// Actions:
        /// 20, ActionX
        /// 10, ActionY
        /// 0, ActionZ
        ///
        /// Alert level
        /// 20 -> ActionX
        /// 19 -> ActionY
        /// 19.9 -> ActionY
        /// 9 -> ActionZ
        /// 0 -> ActionZ
        /// </remarks>
        public void ChangeState(SuspiciousObject spottedObject)
        {
            if (spottedObject == null)
            {
                _alertLevel = 0;

                return;
            }

            _alertLevel += spottedObject.AlertIncrease;
            if (_alertLevel > _maxAlertLevel)
            {
                _alertLevel = _maxAlertLevel;
            }
            var closestIndex = GetClosestIndex();
            try
            {
                StateActions[closestIndex].Invoke(spottedObject.gameObject);
            }
            catch (ArgumentException)
            {
                Debug.Log($"No action found for state {closestIndex}");
            }
        }

        /// <summary>
        /// Gets the closest dictionary index to the current <see cref="_alertLevel"/>.
        /// </summary>
        /// <returns>The closest index rounded down.</returns>
        private float GetClosestIndex()
        {
            var keys = StateActions.Keys.OrderBy(key => key).ToList();

            for (var i = keys.Count - 1; i >= 0; i--)
            {
                if (_alertLevel >= keys[i])
                {
                    return keys[i];
                }
            }

            return 0;
        }
        
    }
}