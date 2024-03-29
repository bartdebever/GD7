﻿using UnityEngine;

namespace Assets.Script.Suspicious
{
    /// <summary>
    /// Defines an object as being suspicious and a guard being able to see it.
    /// </summary>
    public class SuspiciousObject : MonoBehaviour
    {
        /// <summary>
        /// The amount of alert that the guard's alert stage should increase with.
        /// </summary>
        public float AlertIncrease;

        /// <summary>
        /// The total amount of time it takes for a guard to interact with this entity.
        /// </summary>
        public float TimeToInteract;
    }
}
