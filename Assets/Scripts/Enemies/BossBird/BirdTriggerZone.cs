using UnityEngine;

public class BirdTriggerZone : MonoBehaviour
{
    [SerializeField]
    BirdFollowPlayer bird;

    bool pendingActivation = true;

    [SerializeField]
    float cameraSizeIncreaseSpeed = 5;

    private void Update()
    {
        if (!pendingActivation)
        {
            var Maincam = GameObject.FindGameObjectWithTag("MainCamera");
            if (Maincam.GetComponent<Camera>().orthographicSize < 10)
            {
                Maincam.GetComponent<Camera>().orthographicSize += cameraSizeIncreaseSpeed * Time.deltaTime;
            }

            if (Maincam.GetComponent<FollowCamera>().Offset.y < 7)
            {
                Maincam.GetComponent<FollowCamera>().Offset.y += cameraSizeIncreaseSpeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pendingActivation)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                bird.Activate(collision.gameObject);
                pendingActivation = false;
            }
        }
    }
}
