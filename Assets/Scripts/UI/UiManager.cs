using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    GameObject HealthText;

    [SerializeField]
    GameObject MoneyText;

    PlayerStats player;
    PlayerController playerController;

    [SerializeField]
    GameObject BubbleParentUIElement;

    [SerializeField]
    GameObject BubbleUIElementToSpawn;

    [SerializeField]
    GameObject AmmoParentUIElement;

    [SerializeField]
    GameObject AmmoUIElementToSpawn;

    int lastUpdatedBubbleCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.GetComponent<TextMeshProUGUI>().SetText(player.Health.ToString());
        MoneyText.GetComponent<TextMeshProUGUI>().SetText(player.Money.ToString());
    }

    public void AddBubble(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newBubble = Instantiate(BubbleUIElementToSpawn, BubbleParentUIElement.transform);
            newBubble.transform.parent = BubbleParentUIElement.transform;
        }
    }

    public void RemoveBubble(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject go = BubbleParentUIElement.transform.GetChild(0).gameObject;
            if (go)
            {
                Destroy(go);
            }
        }
    }

    public void AddAmmo(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newAmmo = Instantiate(AmmoUIElementToSpawn, AmmoParentUIElement.transform);
            newAmmo.transform.parent = AmmoParentUIElement.transform;
        }
    }

    public void RemoveAmmo(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject go = AmmoParentUIElement.transform.GetChild(0).gameObject;
            if (go)
            {
                Destroy(go);
            }
        }
    }
}
