using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Spelaren som kameran ska följa
    public float smoothSpeed = 0.125f; // Hur mjukt kameran följer efter
    public Vector3 offset;          // Förskjutning (t.ex. för att se lite högre upp)

    void FixedUpdate()
    {
        if (target != null)
        {
            // Vi vill bara följa spelaren i höjdled (Y)
            Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y + offset.y, transform.position.z);
            
            // Om spelaren rör sig neråt vill vi kanske inte att kameran följer med? 
            // För ett hoppspel brukar kameran bara åka uppåt.
            if (desiredPosition.y > transform.position.y)
            {
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
    }
}
