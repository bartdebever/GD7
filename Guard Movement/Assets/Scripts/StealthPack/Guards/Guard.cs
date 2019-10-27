using Assets.Script.Basics;
using Assets.Script.MonoBehaviourExtensions;
using UnityEngine;

namespace Assets.Script.Guards
{
    public abstract class Guard : BasicStateMachine
    {
        #region Publics
        public GuardData GuardData;
        public MonoMovementHandler MovementHandler;
        public MonoMovementPattern MovementPattern;
        public MonoDetectionSystem DetectionSystem;
        #endregion

        #region Unity Methods
        protected virtual void FixedUpdate()
        {
            MovementHandler.Tick(gameObject);
        }
        #endregion

    }
}
