using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Suspicious;
using UnityEngine;

namespace Assets.Script.Basics
{
    public abstract class BasicStateMachine : MonoBehaviour
    {
        [SerializeField] private float _alertLevel;

        [SerializeField] private float _maxAlertLevel;

        public abstract Dictionary<float, Action<GameObject>> StateActions { get; }

        protected void ChangeState(float state)
        {
            _alertLevel = state;

            if (_alertLevel > _maxAlertLevel)
            {
                _alertLevel = _maxAlertLevel;
            }
            else if (_alertLevel < 0)
            {
                _alertLevel = 0;
            }
        }

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
        /// <returns></returns>
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