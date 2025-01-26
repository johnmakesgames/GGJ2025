using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    static GameObject shopKeeperUI;

    [SerializeField]
    PlayerStats ItemOneOption;
    [SerializeField]
    Sprite ItemOneImage;

    [SerializeField]
    PlayerStats ItemTwoOption;
    [SerializeField]
    Sprite ItemTwoImage;

    [SerializeField]
    PlayerStats ItemThreeOption;
    [SerializeField]
    Sprite ItemThreeImage;

    [SerializeField]
    PlayerStats ItemFourOption;
    [SerializeField]
    Sprite ItemFourImage;

    GameObject crossHair;

    [SerializeField]
    float NormalizedPercentageThroughLevel = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject gobj = GameObject.FindGameObjectWithTag("ShopUI");
        if (gobj)
        {
            shopKeeperUI = gobj;
            shopKeeperUI?.SetActive(false);
        }

        crossHair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shopKeeperUI?.SetActive(true);
            shopKeeperUI.GetComponent<ShopKeeperUI>()?.ActivateShop(ItemOneOption, ItemTwoOption, ItemThreeOption, ItemFourOption);
            shopKeeperUI.GetComponent<ShopKeeperUI>()?.SetShopImages(ItemOneImage, ItemTwoImage, ItemThreeImage, ItemFourImage);

            collision.gameObject.GetComponent<PlayerStats>().distanceThroughLevel = Mathf.Max(collision.gameObject.GetComponent<PlayerStats>().distanceThroughLevel, NormalizedPercentageThroughLevel);

            crossHair?.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shopKeeperUI?.SetActive(false);
            crossHair?.SetActive(true);
        }
    }
}
