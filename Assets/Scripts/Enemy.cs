using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    [Header("Combat")]
    public int damage = 1;
    public int maxHealth = 3;

    // Cached components and data
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;

    // Runtime state
    private int currentHealth;
    private bool isGrounded;
    private bool shouldJump;

    void Awake()
    {
        // Cache components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogColor = spriteRenderer.color;

        // Find player if not assigned
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void OnEnable()
    {
        // Reset health and color whenever enabled
        currentHealth = maxHealth;
        spriteRenderer.color = ogColor;
    }

    void Update()
    {
        // Check if on ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        if (isGrounded && player != null)
        {
            // Move towards player horizontally
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

            // Raycasts for jumping logic
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
            bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 6f, 1 << player.gameObject.layer);
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 6f, groundLayer);

            if ((!groundInFront.collider && !gapAhead.collider) || (isPlayerAbove && platformAbove.collider))
            {
                shouldJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 dir = (player.position - transform.position).normalized;
            rb.AddForce(new Vector2(dir.x * jumpForce, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        StartCoroutine(FlashWhite());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
