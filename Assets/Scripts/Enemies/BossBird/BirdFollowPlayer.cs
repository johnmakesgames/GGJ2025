using UnityEngine;

public class BirdFollowPlayer : MonoBehaviour
{
    private GameObject player;
    private bool isActive;
    float movementSpeed;

    public AudioSource cawPlayer;
    public AudioClip cawClip;

    private void Start()
    {
        cawPlayer = GetComponent<AudioSource>();

        cawClip.LoadAudioData();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && player)
        {
            Vector3 dir = player.transform.position - this.transform.position;
            dir.Normalize();

            this.transform.position += dir * movementSpeed * Time.deltaTime;

            movementSpeed += Time.deltaTime;

            if(!cawPlayer.isPlaying)
            {
                cawPlayer.PlayOneShot(cawClip);

            }
        }
    }

    public void Activate(GameObject player)
    {
        this.player = player;
        isActive = true;

        cawPlayer.PlayOneShot(cawClip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats killingPlayerStats = new PlayerStats();
            killingPlayerStats.Health = -99999;
            collision.gameObject.GetComponent<PlayerStats>().IncrementFromStats(killingPlayerStats);
        }
    }
}
