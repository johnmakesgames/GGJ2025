using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health = 3;
    public int Money = 0;

    public int MaxBubbleCountMod = 0;
    public float MaxBubbleSizeMod = 0;
    public float BubbleRechargeTimeMod = 0;
    public float InflatingSpeedMod = 0;

    public void IncrementFromStats(PlayerStats otherStats)
    {
        Health += otherStats.Health;
        Money += otherStats.Money;
        MaxBubbleCountMod += otherStats.MaxBubbleCountMod;
        MaxBubbleSizeMod += otherStats.MaxBubbleSizeMod;
        BubbleRechargeTimeMod += otherStats.BubbleRechargeTimeMod;
        InflatingSpeedMod += otherStats.InflatingSpeedMod;
    }

    public void DecrementFromStats(PlayerStats otherStats)
    {
        Health -= otherStats.Health;
        Money -= otherStats.Money;
        MaxBubbleCountMod -= otherStats.MaxBubbleCountMod;
        MaxBubbleSizeMod -= otherStats.MaxBubbleSizeMod;
        BubbleRechargeTimeMod -= otherStats.BubbleRechargeTimeMod;
        InflatingSpeedMod -= otherStats.InflatingSpeedMod;
    }
}
