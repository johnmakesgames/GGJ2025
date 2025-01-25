using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class StatChangingPickup : MonoBehaviour
{
    PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>()?.IncrementFromStats(stats);
            Destroy(this.gameObject);
        }
    }
}
