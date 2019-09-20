using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MovementHelper
    {
        public static void Move(Rigidbody origin, Vector3 toMoveTo, float speed)
        {
            // There must be a better way to do this but this is testing code so who cares.
            var movePosition = origin.transform.position;

            movePosition.x = Mathf.MoveTowards(origin.transform.position.x, toMoveTo.x, speed * Time.fixedDeltaTime);
            movePosition.z = Mathf.MoveTowards(origin.transform.position.z, toMoveTo.z, speed * Time.fixedDeltaTime);
            movePosition.y = origin.transform.position.y;

            origin.MovePosition(movePosition);
            origin.transform.LookAt(toMoveTo);
        }

        public static bool IsNotInRange(GameObject origin, Vector3 target, float maxDistance)
        {
            var originPosition = origin.transform.position;

            // Fake the y position
            target.y = originPosition.y;
            return Vector3.Distance(originPosition, target) > maxDistance;
        }
    }
}
