using UnityEngine;
using System.Collections.Generic;

public class GreenCubeController : MonoBehaviour
{
    public Transform target; // Assign the red cube's Transform reference in the Inspector

    private GridManager gridManager;
    private bool isMoving;
    private List<Node> path;
    private int targetIndex = 0; // Index to traverse the path
    private float movementSpeed = 3.0f; // Adjust as needed

    private void Start()
    {
        gridManager = GridManager.Instance;
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.LogError("Target (Red Cube) not assigned to Green Cube Controller!");
            return;
        }

        if (!isMoving)
        {
            FindPathToTarget(target.position);
        }
        else
        {
            MoveGreenCubeAlongPath();
        }
    }

    private void FindPathToTarget(Vector3 targetPosition)
    {
        int startIndex = gridManager.GetGridIndex(transform.position);
        int targetIndex = gridManager.GetGridIndex(targetPosition);

        Node startNode = gridManager.nodes[startIndex / gridManager.numOfRows, startIndex % gridManager.numOfColumns];
        Node endNode = gridManager.nodes[targetIndex / gridManager.numOfRows, targetIndex % gridManager.numOfColumns];

        if (path != null)
        {
            path.Clear(); // Clear the existing path
        }
        else
        {
            path = new List<Node>();
        }

        path = AStar.FindPath(startNode, endNode);

        if (path != null && path.Count > 0)
        {
            isMoving = true;
            targetIndex = 0;
            Debug.Log("Path Found! Moving towards the target.");
        }
        else
        {
            Debug.LogWarning("Path not found!");
        }
    }

    private void MoveGreenCubeAlongPath()
    {
        if (path != null && path.Count > 0 && targetIndex < path.Count)
        {
            Vector3 targetPosition = path[targetIndex].position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (distance < 0.05f)
            {
                targetIndex++;

                if (targetIndex >= path.Count)
                {
                    isMoving = false;
                    targetIndex = 0;
                    Debug.Log("Reached the target!");
                    path.Clear(); // Clear the path once the destination is reached
                }
            }
        }
    }
}