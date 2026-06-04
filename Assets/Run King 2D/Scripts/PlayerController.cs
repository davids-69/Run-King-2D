using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Rörelse")]
    public float moveSpeed = 6f;
    public float minJumpForce = 8f;
    public float maxJumpForce = 18f;
    public float jumpChargeSpeed = 15f; 
    public float jumpPushX = 3f;      
    public float fallMultiplier = 2.5f;

    [Header("Mark-kollision")]
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int direction = 1; 
    private float currentJumpForce;
    private bool isCharging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumpForce = minJumpForce;
    }

    void Update()
    {
        // 1. Mark-check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 2. Hantera hopp-laddning
        HandleJump();

        // 3. Automatisk rörelse (bara om vi inte laddar ett hopp)
        if (isGrounded && !isCharging)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        else if (isGrounded && isCharging)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // 4. Bättre fall-känsla
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Vänd spriten åt rätt håll
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, 1);
    }

    void HandleJump()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isCharging = true;
                currentJumpForce += jumpChargeSpeed * Time.deltaTime;
                currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);
            }
            
            if (Input.GetKeyUp(KeyCode.Space) && isCharging)
            {
                Jump();
            }
        }
        else
        {
            isCharging = false;
            currentJumpForce = minJumpForce;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2((direction * moveSpeed) + (direction * jumpPushX), currentJumpForce);
        isCharging = false;
        currentJumpForce = minJumpForce;
    }

    // VÄND BARA NÄR VI KROCKAR MED VÄGGAR
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vi kollar om objektet vi krockar med har taggen "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Vi kollar också att krocken faktiskt kom från sidan (X-axeln)
            if (Mathf.Abs(collision.contacts[0].normal.x) > 0.5f)
            {
                direction *= -1;
            }
        }
    }
}
