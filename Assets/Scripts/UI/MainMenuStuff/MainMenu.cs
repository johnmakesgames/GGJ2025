using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject SettingsMenu;

    [SerializeField]
    string SceneToLoadOnPlay;

    PlayerStats stats;

    [SerializeField]
    TextMeshProUGUI FurthestPercentText;

    [SerializeField]
    TextMeshProUGUI MostRecentPercentText;

    [SerializeField]
    TextMeshProUGUI MoneyText;

    [SerializeField]
    TextMeshProUGUI MaxBubbleCountModText;

    [SerializeField]
    TextMeshProUGUI MaxBubbleSizeModText;

    [SerializeField]
    TextMeshProUGUI InflatingSpeedModText;

    [SerializeField]
    TextMeshProUGUI TimeTakenToCompleteText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        stats.LoadStats();
        PopulateStatsScreen();
    }

    void PopulateStatsScreen()
    {
        FurthestPercentText.text = $"{(100 * stats.furthestDistanceThroughLevel).ToString()}%";
        MostRecentPercentText.text = $"{(100 * stats.lastDistanceThroughLevel).ToString()}%";
        MoneyText.text = stats.Money.ToString();
        MaxBubbleCountModText.text = stats.MaxBubbleCountMod.ToString();
        MaxBubbleSizeModText.text = stats.MaxBubbleSizeMod.ToString();
        InflatingSpeedModText.text = stats.InflatingSpeedMod.ToString();
        TimeTakenToCompleteText.text = (stats.FastestTimeTakenToComplete != -1) ? $"{stats.FastestTimeTakenToComplete.ToString()} Seconds" : "Never Completed";
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene(SceneToLoadOnPlay, LoadSceneMode.Single);
    }

    public void OnSettingPressed()
    {
        SettingsMenu.SetActive(!SettingsMenu.activeInHierarchy);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
