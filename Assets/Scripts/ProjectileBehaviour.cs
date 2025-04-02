using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5f;
    private Vector2 direction;

    private Rigidbody2D rb;
    private Collider2D coll;
    private Vector2 startingPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        startingPoint = transform.position;
    }

    void Update()
    {
        rb.velocity = direction * speed;
        if(transform.position.x > startingPoint.x + 20 || transform.position.x < startingPoint.x - 20)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // When the projectile hits the player, we call PlayerHit() method from PlayerBehaviour
            PlayerBehaviour playerBehaviour = collision.gameObject.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null)
            {
                playerBehaviour.PlayerHit(); // This calls the PlayerHit method which handles damage with a delay
                Destroy(gameObject); // Destroy the projectile after it hits the player
            }
        }

        // If the projectile collides with any other object (e.g., ground, walls), destroy it
/*        if (collision.gameObject.CompareTag("Room"))
        {
            Destroy(gameObject);
        }*/
    }

/*    void OnTriggerEnter2D(Collider2D other)
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
        if (other.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }*/
}
