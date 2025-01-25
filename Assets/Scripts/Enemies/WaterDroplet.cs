using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D col;
    EnemyStats stats;

    [SerializeField]
    float timeToFall;
    float timeSinceSpawn = 0;

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
        if (timeSinceSpawn <= timeToFall)
        {
            timeSinceSpawn += Time.deltaTime;
            this.transform.localScale = new Vector3(this.transform.localScale.x, timeSinceSpawn / timeToFall, this.transform.localScale.z);
            col.size = new Vector2(timeSinceSpawn / timeToFall, (timeSinceSpawn / timeToFall) * 2);
        }
        else
        {
            rb.gravityScale = 1;
        }

        if (!stats.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < this.transform.position.y)
        {
            if (collision.gameObject.CompareTag("JohnTestGround"))
            {
                Destroy(this.gameObject);
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                var stats = collision.gameObject.GetComponent<PlayerStats>();
                if (stats)
                {
                    stats.Health -= this.stats.GetDamageFromEnemy();
                }

                Destroy(this.gameObject);
            }
        }
    }
}
