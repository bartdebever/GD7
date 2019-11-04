using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class PatternGenerator : MonoBehaviour
{
    public List<GameObject> PlayFields;
    private List<BoxCollider> _obstacles;

    private void Start()
    {
        _obstacles = GetComponentsInChildren<BoxCollider>().ToList();

        Game.PatternGenerator = this;
    }

    public IEnumerable<Vector3> GeneratePattern(Vector3 origin)
    {
        const long patternWidth = 15;
        const long patternHeight = 15;

        // Origin is the middle points of the pattern that the guard should search
        // around.
        // The structure would look something like this:
        // x...x
        // ..o..
        // x...x
        // With the o being the player and the x being the pattern we want to create
        // To make this we simple need to get the maximum and minimum x and z coordinates.
        // With these variables we can make a square around the target by taking
        // (min, min), (min, max), (max,max), (max, min)
        // This will perfectly loop into a pattern that the guard can walk.
        var minX = origin.x - (patternWidth / 2f);
        var maxX = origin.x + (patternWidth / 2f);
        var minZ = origin.z - (patternHeight / 2f);
        var maxZ = origin.z + (patternHeight / 2f);

        var positionList = new Vector3[4];
        positionList[0] = new Vector3(minX, origin.y, minZ);
        positionList[1] = new Vector3(minX, origin.y, maxZ);
        positionList[2] = new Vector3(maxX, origin.y, maxZ);
        positionList[3] = new Vector3(maxX, origin.y, minZ);

        // To make sure the guard doesn't go out of bounds, the floor and obstacles
        // are used.
        // Any points that fall out of bounds or points that are inside walls are
        // neglected and removed from the final list.
        // These could be moved to a different place to keep the 4 wide searching
        // pattern, yet in the scope of this game and for the scripts to work,
        // this is not needed.
        return positionList.Where(vector3 =>
            IsInBounds(vector3) && !IsInObstacle(vector3)
            ).ToList();
    }

    private bool IsInObstacle(Vector3 point)
    {
        return _obstacles.Any(meshCollider => meshCollider.bounds.Contains(point));
    }

    private bool IsInBounds(Vector3 point)
    {
        return PlayFields.Any(playField =>
        {
            var meshCollider = playField.GetComponent<MeshCollider>();
            return meshCollider.bounds.Contains(new Vector3(point.x, playField.transform.position.y, point.z));
        });
    }
}
