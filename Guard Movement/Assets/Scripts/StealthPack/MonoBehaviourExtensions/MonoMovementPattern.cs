﻿using System.Collections.Generic;
using Assets.Script.Interface;
using UnityEngine;

namespace Assets.Script.MonoBehaviourExtensions
{
    /// <summary>
    /// Basic abstract class that implements <see cref="MonoBehaviour"/> and
    /// <see cref="IMovementPattern"/>.
    /// </summary>
    public abstract class MonoMovementPattern : MonoBehaviour, IMovementPattern
    {
        /// <inheritdoc />
        public abstract Vector3 GetCurrentTarget();

        /// <inheritdoc />
        public abstract Vector3 GetNextTarget();

        public abstract void SetNewPattern(IEnumerable<Vector3> pattern);

        public abstract void ResetPattern();
    }
}
