using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternGenerator : MonoBehaviour
{
    public List<GameObject> PlayFields;
    private List<BoxCollider> _obstacles;

    private void Start()
    {
        _obstacles = GetComponentsInChildren<BoxCollider>().ToList();
        Debug.Log(_obstacles.Count);
    }

    public IEnumerable<Vector3> GeneratePattern(Vector3 origin)
    {
        const long patternWidth = 15;
        const long patternHeight = 15;

        var minX = origin.x - (patternWidth / 2f);
        var maxX = origin.x + (patternWidth / 2f);
        var minZ = origin.z - (patternHeight / 2f);
        var maxZ = origin.z + (patternHeight / 2f);

        var positionList = new Vector3[4];
        positionList[0] = new Vector3(minX, origin.y, minZ);
        positionList[1] = new Vector3(minX, origin.y, maxZ);
        positionList[2] = new Vector3(maxX, origin.y, maxZ);
        positionList[3] = new Vector3(maxX, origin.y, minZ);


        var finalList = new List<Vector3>();
        foreach (var vector3 in positionList)
        {
            if (!IsInBounds(vector3))
            {
                continue;
            }

            if (IsInObstacle(vector3))
            {
                continue;
            }

            finalList.Add(vector3);
        }

        return finalList;
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
