using UnityEngine;

public class BulletScript : MonoBehaviour
{
    CircleCollider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collider.isTrigger = true;
            return;
        }

        EnemyStats stats = collision.gameObject.GetComponent<EnemyStats>();
        if (stats)
        {
            stats.DealDamage(1);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collider.isTrigger = false;
        }
    }
}
