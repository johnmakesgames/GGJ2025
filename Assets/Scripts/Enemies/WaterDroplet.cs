using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D col;
    EnemyStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CapsuleCollider2D>();
        stats = this.GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.gravityScale = 1;

        if (!stats.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < this.transform.position.y)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var stats = collision.gameObject.GetComponent<PlayerStats>();
                if (stats)
                {
                    stats.Health -= this.stats.GetDamageFromEnemy();
                }

            }

            Destroy(this.gameObject);

        }
    }
}
