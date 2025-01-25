using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    GameObject shopKeeperUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopKeeperUI = GameObject.FindGameObjectWithTag("ShopUI");
        shopKeeperUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shopKeeperUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shopKeeperUI.SetActive(false);
        }
    }
}
