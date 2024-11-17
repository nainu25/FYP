using UnityEngine;

public class BoundaryCheck : MonoBehaviour
{
    // Define the boundaries of the playable area
    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = -5f;
    public float yMax = 5f;

    private void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        Vector3 position = transform.position;

        // Check if the snake's head is out of bounds and teleport it to the other side
        if (position.x < xMin)
        {
            position.x = xMax;
        }
        else if (position.x > xMax)
        {
            position.x = xMin;
        }

        if (position.y < yMin)
        {
            position.y = yMax;
        }
        else if (position.y > yMax)
        {
            position.y = yMin;
        }

        transform.position = position;
    }
}
