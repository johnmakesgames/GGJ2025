using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class StatChangingPickup : MonoBehaviour
{
    PlayerStats stats;

    [SerializeField]
    bool temporary;

    [SerializeField]
    float temporaryDuration;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>()?.IncrementFromStats(stats);

            if (!temporary)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(DisableAfterDuration(temporaryDuration, collision.gameObject.GetComponent<PlayerStats>()));
            }

        }
    }

    private IEnumerator DisableAfterDuration(float duration, PlayerStats statsToRemoveEffectFrom)
    {
        yield return new WaitForSeconds(duration);

        statsToRemoveEffectFrom?.DecrementFromStats(stats);
        Destroy(this.gameObject);
    }
}
