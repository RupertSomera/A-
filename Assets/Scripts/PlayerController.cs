using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GridManager gridManager;
    private bool isMoving;
    private Node targetNode;
    private float movementSpeed = 5.0f; // Adjust as needed

    private void Start()
    {
        gridManager = GridManager.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    Vector3 clickPosition = hit.point;
                    int targetIndex = gridManager.GetGridIndex(clickPosition);
                    targetNode = gridManager.nodes[targetIndex / gridManager.numOfRows, targetIndex % gridManager.numOfColumns];

                    // Start moving towards the target node
                    MoveToTargetNode();
                }
            }
        }

        if (isMoving)
        {
            MoveCubeTowardsTarget();
        }
    }

    private void MoveToTargetNode()
    {
        isMoving = true;
    }

    private void MoveCubeTowardsTarget()
    {
        if (targetNode != null)
        {
            Vector3 targetPosition = targetNode.position;
            Vector3 currentPosition = transform.position;

            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);

            if (transform.position == targetPosition)
            {
                // Arrived at the target node
                isMoving = false;
                targetNode = null;
            }
        }
    }
}