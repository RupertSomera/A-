using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    public GameObject greenCube; // Assign GreenCube GameObject in the Inspector
    private GameObject endCube;

    private float movementSpeed = 5.0f; // Adjust as needed
    private List<Node> path;
    private int targetIndex = 0;

    private void Start()
    {
        endCube = GameObject.FindGameObjectWithTag("End");
        path = new List<Node>();
        CalculatePath();
    }

    private void Update()
    {
        MoveGreenCubeAlongPath();
    }

    private void CalculatePath()
    {
        if (greenCube != null && endCube != null)
        {
            Node startNode = new Node(GridManager.Instance.GetGridCellCenter(
                GridManager.Instance.GetGridIndex(greenCube.transform.position)));

            Node endNode = new Node(GridManager.Instance.GetGridCellCenter(
                GridManager.Instance.GetGridIndex(endCube.transform.position)));

            path = AStar.FindPath(startNode, endNode);
        }
    }

    private void MoveGreenCubeAlongPath()
    {
        if (path != null && path.Count > 0 && targetIndex < path.Count)
        {
            Vector3 targetPosition = path[targetIndex].position;

            // Calculate the distance between the current and target positions
            float distance = Vector3.Distance(transform.position, targetPosition);

            // Move towards the target position at a consistent speed
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the distance has become very small, indicating reaching the target
            if (distance < 0.05f)
            {
                targetIndex++;

                if (targetIndex >= path.Count)
                {
                    // Reached the end of the path
                    path = null;
                    targetIndex = 0;

                    // Debug log for checking
                    Debug.Log("Reached the target!");
                }
            }
        }
    }
}