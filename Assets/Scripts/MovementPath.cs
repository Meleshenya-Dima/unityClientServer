using System.Collections.Generic;
using UnityEngine;

public class MovementPath : MonoBehaviour
{
    public enum PathTypes
    {
        linear
    }

    public PathTypes PathType;
    public int movementDirection = 1;
    public int moveingTo = 0;
    public Transform[] PathElements;

    public void OnDrawGizmos()
    {
        if (PathElements == null || PathElements.Length < 2)
            return;
        for (int i = 1; i < PathElements.Length; i++)
            Gizmos.DrawLine(PathElements[i - 1].position, PathElements[i].position);
    }

    public IEnumerator<Transform> GetNextPathPoints()
    {
        if (PathElements == null || PathElements.Length < 1)
            yield break;
        while (true)
        {
            yield return PathElements[moveingTo];

            if (PathElements.Length == 1)
            {
                continue;
            }

            if (PathType == PathTypes.linear)
            {
                if (moveingTo <= 0)
                {
                    movementDirection = 1;
                }
                else if (moveingTo >= PathElements.Length - 1)
                {
                    movementDirection = -1;
                }
            }
            moveingTo += movementDirection;
        }
    }
}
