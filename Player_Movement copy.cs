using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;      // Horizontal movement speed
    public float jumpForce = 10f;     // Force applied when jumping
    private Rigidbody2D rb;
    private bool isGrounded;

    // Advanced MonoBehaviour Lifecycle Functions
    void OnEnable()
    {
        Debug.Log("PlayerController enabled");
    }

    void OnDisable()
    {
        Debug.Log("PlayerController disabled");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement (A and D keys or Left and Right arrow keys)
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Jumping logic (W key)
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Correct y-axis velocity for jump
            Debug.Log("Player jumped!");
        }
    }

    void LateUpdate()
    {
        // Post-frame updates (e.g., clamping player position within bounds)
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -10f, 10f);
        transform.position = clampedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player touches the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Player left the ground.");
        }
    }

    // Object Instantiation
    public GameObject projectilePrefab;
    public Transform firePoint;

    void FireProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Projectile fired!");
        }
    }

    // Event System Example
    public delegate void PlayerAction();
    public static event PlayerAction OnPlayerJump;

    void TriggerJumpEvent()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            OnPlayerJump?.Invoke();
        }
    }

    // Advanced GetComponent Usage
    void RetrieveNestedComponent()
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Example of dynamic manipulation
        }
    }
}

// Custom Component Example
public class CustomMovementBehavior : MonoBehaviour
{
    public float speedMultiplier = 1.5f;

    public void ApplySpeedBoost(Rigidbody2D rb)
    {
        rb.velocity *= speedMultiplier;
        Debug.Log("Speed boost applied!");
    }
}

// ScriptableObject Example
[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float health = 100f;
    public float damage = 25f;
    public float defense = 10f;
}
