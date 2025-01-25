using UnityEngine;

public class SimpleDoDamage : MonoBehaviour
{
    EnemyStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = this.GetComponent<EnemyStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats)
            {
                stats.Health -= this.stats.GetDamageFromEnemy();
            }
        }
    }
}
