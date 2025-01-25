using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperUI : MonoBehaviour
{
    [SerializeField]
    PlayerStats ItemOneOption;

    [SerializeField]
    Button ItemOneButton;

    [SerializeField]
    PlayerStats ItemTwoOption;

    [SerializeField]
    Button ItemTwoButton;

    [SerializeField]
    PlayerStats ItemThreeOption;

    [SerializeField]
    Button ItemThreeButton;

    [SerializeField]
    PlayerStats ItemFourOption;

    [SerializeField]
    Button ItemFourButton;

    PlayerStats player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        ItemOneButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemOneOption.Money.ToString());
        ItemTwoButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemTwoOption.Money.ToString());
        ItemThreeButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemThreeOption.Money.ToString());
        ItemFourButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemFourOption.Money.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Money + ItemOneOption.Money < 0)
        {
            ItemOneButton.interactable = false;
        }

        if (player.Money + ItemTwoOption.Money < 0)
        {
            ItemTwoButton.interactable = false;
        }

        if (player.Money + ItemThreeOption.Money < 0)
        {
            ItemThreeButton.interactable = false;
        }

        if (player.Money + ItemFourOption.Money < 0)
        {
            ItemFourButton.interactable = false;
        }
    }

    public void BuyItemOne()
    {
        player.IncrementFromStats(ItemOneOption);
    }

    public void BuyItemTwo()
    {
        player.IncrementFromStats(ItemTwoOption);
    }

    public void BuyItemThree()
    {
        player.IncrementFromStats(ItemThreeOption);
    }

    public void BuyItemFour()
    {
        player.IncrementFromStats(ItemFourOption);
    }
}
