using UnityEngine;

public class Movment : MonoBehaviour
{
    public int direction;
    private Vector3 movement;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // rb.linearVelocity += new Vector2(2 * direction * Time.deltaTime, -1f);
        movement = new Vector3(2 * direction, 0f, 0f);
        transform.position = transform.position + movement * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = direction * -1;
    }
}
