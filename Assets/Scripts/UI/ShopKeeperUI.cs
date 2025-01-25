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
    Image ItemOneImage;

    [SerializeField]
    PlayerStats ItemTwoOption;

    [SerializeField]
    Button ItemTwoButton;

    [SerializeField]
    Image ItemTwoImage;

    [SerializeField]
    PlayerStats ItemThreeOption;

    [SerializeField]
    Button ItemThreeButton;

    [SerializeField]
    Image ItemThreeImage;

    [SerializeField]
    PlayerStats ItemFourOption;

    [SerializeField]
    Button ItemFourButton;

    [SerializeField]
    Image ItemFourImage;

    PlayerStats player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void ActivateShop(PlayerStats I1O, PlayerStats I2O, PlayerStats I3O, PlayerStats I4O)
    {
        if (I1O)
            this.ItemOneOption.CopyFromStats(I1O);
        if (I2O)
            this.ItemTwoOption.CopyFromStats(I2O);
        if (I3O)
            this.ItemThreeOption.CopyFromStats(I3O);
        if (I4O)
            this.ItemFourOption.CopyFromStats(I4O);

        ItemOneButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemOneOption.Money.ToString());
        ItemTwoButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemTwoOption.Money.ToString());
        ItemThreeButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemThreeOption.Money.ToString());
        ItemFourButton.GetComponentInChildren<TextMeshProUGUI>().SetText(ItemFourOption.Money.ToString());
    }

    public void SetShopImages(Sprite I1I, Sprite I2I, Sprite I3I, Sprite I4I)
    {
        ItemOneImage.sprite = I1I;
        ItemTwoImage.sprite = I2I;
        ItemThreeImage.sprite = I3I;
        ItemFourImage.sprite = I4I;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Money + ItemOneOption.Money < 0)
        {
            ItemOneButton.interactable = false;
        }
        else
        {
            ItemOneButton.interactable = true;
        }

        if (player.Money + ItemTwoOption.Money < 0)
        {
            ItemTwoButton.interactable = false;
        }
        else
        {
            ItemTwoButton.interactable = true;
        }

        if (player.Money + ItemThreeOption.Money < 0)
        {
            ItemThreeButton.interactable = false;
        }
        else
        {
            ItemThreeButton.interactable = true;
        }

        if (player.Money + ItemFourOption.Money < 0)
        {
            ItemFourButton.interactable = false;
        }
        else
        {
            ItemFourButton.interactable = true;
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
