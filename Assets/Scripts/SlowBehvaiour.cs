using UnityEngine;

public class SlowBehvaiour : MonoBehaviour
{
    public int damage = 4;
    public float speed = 4f;
    private Vector2 direction;

    private Rigidbody2D rb;
    private Collider2D coll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        rb.velocity = direction * speed;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // When the projectile hits the player, we call PlayerHit() method from PlayerBehaviour
            PlayerBehaviour playerBehaviour = other.gameObject.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                playerBehaviour.PlayerHit(); // This calls the PlayerHit method which handles damage with a delay
                Destroy(gameObject); // Destroy the projectile after it hits the player
            }
        }

        // If the projectile collides with any other object (e.g., ground, walls), destroy it
        if (other.CompareTag("Tilemap"))
        {
            Destroy(gameObject);
        }
    }
}
