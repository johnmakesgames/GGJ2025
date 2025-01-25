using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health = 3;
    public int Money = 0;

    public int MaxBubbleCountMod = 0;
    public float MaxBubbleSizeMod = 0;
    public float BubbleRechargeTimeMod = 0;
    public float InflatingSpeedMod = 0;


    public bool appliesMaterialChange = false;
    public Material DefaultMaterial;
    public Material ChangedMaterial;

    public void IncrementFromStats(PlayerStats otherStats)
    {
        Health += otherStats.Health;
        Money += otherStats.Money;
        MaxBubbleCountMod += otherStats.MaxBubbleCountMod;
        MaxBubbleSizeMod += otherStats.MaxBubbleSizeMod;
        BubbleRechargeTimeMod += otherStats.BubbleRechargeTimeMod;
        InflatingSpeedMod += otherStats.InflatingSpeedMod;

        if (otherStats.appliesMaterialChange && !appliesMaterialChange)
        {
            this.GetComponent<SpriteRenderer>().material = otherStats.ChangedMaterial;
        }

        if (!otherStats.appliesMaterialChange && appliesMaterialChange)
        {
            this.GetComponent<SpriteRenderer>().material = DefaultMaterial;
        }
    }

    public void DecrementFromStats(PlayerStats otherStats)
    {
        Health -= otherStats.Health;
        Money -= otherStats.Money;
        MaxBubbleCountMod -= otherStats.MaxBubbleCountMod;
        MaxBubbleSizeMod -= otherStats.MaxBubbleSizeMod;
        BubbleRechargeTimeMod -= otherStats.BubbleRechargeTimeMod;
        InflatingSpeedMod -= otherStats.InflatingSpeedMod;

        if (otherStats.appliesMaterialChange)
        {
            this.GetComponent<SpriteRenderer>().material = DefaultMaterial;
        }

        if (!otherStats.appliesMaterialChange)
        {
            this.GetComponent<SpriteRenderer>().material = ChangedMaterial;
        }
    }

    public void CopyFromStats(PlayerStats otherStats)
    {
        Health = otherStats.Health;
        Money = otherStats.Money;
        MaxBubbleCountMod = otherStats.MaxBubbleCountMod;
        MaxBubbleSizeMod = otherStats.MaxBubbleSizeMod;
        BubbleRechargeTimeMod = otherStats.BubbleRechargeTimeMod;
        InflatingSpeedMod = otherStats.InflatingSpeedMod;
        appliesMaterialChange = otherStats.appliesMaterialChange;
        DefaultMaterial = otherStats.DefaultMaterial;
        ChangedMaterial = otherStats.ChangedMaterial;
    }
}
