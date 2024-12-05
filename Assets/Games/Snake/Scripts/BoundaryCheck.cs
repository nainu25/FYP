using UnityEngine;

public class BoundaryCheck : MonoBehaviour
{
    [Header("Boundary Settings")]
    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = -5f;
    public float yMax = 5f;

    private void LateUpdate()
    {
        WrapPosition();
    }

    /// <summary>
    /// Ensures the object stays within the defined boundaries by wrapping its position.
    /// </summary>
    private void WrapPosition()
    {
        Vector3 position = transform.position;

        // Wrap position along X-axis
        if (position.x < xMin)
            position.x = xMax;
        else if (position.x > xMax)
            position.x = xMin;

        // Wrap position along Y-axis
        if (position.y < yMin)
            position.y = yMax;
        else if (position.y > yMax)
            position.y = yMin;

        // Apply the wrapped position
        transform.position = position;
    }

    /// <summary>
    /// Dynamically updates boundary settings if needed (optional).
    /// </summary>
    /// <param name="newXMin">New minimum X boundary</param>
    /// <param name="newXMax">New maximum X boundary</param>
    /// <param name="newYMin">New minimum Y boundary</param>
    /// <param name="newYMax">New maximum Y boundary</param>
    public void UpdateBounds(float newXMin, float newXMax, float newYMin, float newYMax)
    {
        xMin = newXMin;
        xMax = newXMax;
        yMin = newYMin;
        yMax = newYMax;
    }
}
