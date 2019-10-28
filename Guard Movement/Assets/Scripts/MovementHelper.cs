using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovementHelper
    {
        /// <summary>
        /// The origin object to move and calculate things for.
        /// </summary>
        private readonly GameObject _origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementHelper"/> class.
        /// </summary>
        /// <param name="origin">The <see cref="GameObject"/> that will be moved.</param>
        public MovementHelper(GameObject origin)
        {
            _origin = origin;
        }

        /// <summary>
        /// Moves the previously provided <see cref="GameObject"/>
        /// towards the provided <paramref name="toMoveTo"/> using the <paramref name="origin"/>
        /// and <paramref name="speed"/>.
        /// </summary>
        /// <param name="origin">The rigidbody to use to move towards the target.</param>
        /// <param name="toMoveTo">The target to move towards.</param>
        /// <param name="speed">The speed at which the origin moves.</param>
        public void Move(Rigidbody origin, Vector3 toMoveTo, float speed)
        {
            // There must be a better way to do this but this is testing code so who cares.
            var movePosition = origin.transform.position;

            movePosition.x = Mathf.MoveTowards(origin.transform.position.x, toMoveTo.x, speed * Time.fixedDeltaTime);
            movePosition.z = Mathf.MoveTowards(origin.transform.position.z, toMoveTo.z, speed * Time.fixedDeltaTime);
            movePosition.y = origin.transform.position.y;

            origin.MovePosition(movePosition);
            origin.transform.LookAt(toMoveTo);
        }

        /// <summary>
        /// Checks if the <paramref name="target"/> is within the provided
        /// <paramref name="maxDistance"/> of the previously given <see cref="GameObject"/>.
        /// This only checks for X and Z.
        /// </summary>
        /// <param name="target">The target which to check for.</param>
        /// <param name="maxDistance">
        /// The maximum amount of distance the <paramref name="target"/> can
        /// be away from <see cref="GameObject"/>/
        /// </param>
        /// <returns>
        /// A bool to indicate if the <paramref name="target"/> and the previously
        /// given <see cref="GameObject"/> are in the <paramref name="maxDistance"/>.
        /// </returns>
        public bool IsInRange(Vector3 target, float maxDistance)
        {
            return GetDistance(target) <= maxDistance;
        }

        /// <summary>
        /// Checks if the <paramref name="target"/> is within the provided
        /// <paramref name="maxDistance"/> of the previously given <see cref="GameObject"/>.
        /// This only checks for X and Z.
        /// </summary>
        /// <param name="target">The target which to check for.</param>
        /// <param name="maxDistance">
        /// The maximum amount of distance the <paramref name="target"/> can
        /// be away from <see cref="GameObject"/>/
        /// </param>
        /// <returns>
        /// A bool to indicate if the <paramref name="target"/> and the previously
        /// given <see cref="GameObject"/> are not in the <paramref name="maxDistance"/>.
        /// </returns>
        public bool IsNotInRange(Vector3 target, float maxDistance)
        {
            return GetDistance(target) > maxDistance;
        }

        /// <summary>
        /// Gets the distance between the <paramref name="target"/> and the
        /// previously given <see cref="GameObject"/>.
        /// This will only calculate the distance of the X and Z.
        /// </summary>
        /// <param name="target">
        /// The target to check the distance to.
        /// </param>
        /// <returns>The calculated distance.</returns>
        public float GetDistance(Vector3 target)
        {
            var originPosition = _origin.transform.position;

            // Fake the y position
            target.y = originPosition.y;
            return Vector3.Distance(originPosition, target);
        }
    }
}
