using System;
using System.Collections.Generic;
using System.IO;
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

    // Normalised value of how far through the level the player is.
    public float distanceThroughLevel;
    public float furthestDistanceThroughLevel;
    public float lastDistanceThroughLevel;

    public float FastestTimeTakenToComplete = -1;
    public float TimeSinceStart = -1;

    private void Update()
    {
        TimeSinceStart += Time.deltaTime;
    }

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


    static readonly string savePath = Path.Combine(Environment.CurrentDirectory, $"GameProgressSave.frgSave");
    static readonly int loadableVersion = 2;
    public void LoadStats()
    {
        if (File.Exists(savePath))
        {
            try
            {
                List<string> saveData = new List<string>(File.ReadAllLines(savePath));
                
                if (!int.TryParse(saveData[0], out int foundVersion))
                {
                    return;
                }

                if (foundVersion != loadableVersion)
                {
                    // Save file not suitable version, quit out.
                    File.Delete(savePath);
                    return;
                }
                
                if (!float.TryParse(saveData[1], out furthestDistanceThroughLevel))
                {
                    furthestDistanceThroughLevel = 0;
                }

                if (furthestDistanceThroughLevel > 1)
                {
                    furthestDistanceThroughLevel = 0;
                }

                if (!float.TryParse(saveData[2], out lastDistanceThroughLevel))
                {
                    lastDistanceThroughLevel = 0;
                }

                if (lastDistanceThroughLevel > 0)
                {
                    lastDistanceThroughLevel = 0;
                }

                if (!int.TryParse(saveData[3], out Money))
                {
                    Money = 0;
                }

                if (!int.TryParse(saveData[4], out MaxBubbleCountMod))
                {
                    MaxBubbleCountMod = 0;
                }

                if (!float.TryParse(saveData[5], out MaxBubbleSizeMod))
                {
                    MaxBubbleSizeMod = 0;
                }

                if (!float.TryParse(saveData[6], out InflatingSpeedMod))
                {
                    InflatingSpeedMod = 0;
                }

                if (!float.TryParse(saveData[7], out FastestTimeTakenToComplete))
                {
                    FastestTimeTakenToComplete = -1;
                }

                TimeSinceStart = 0;

            }
            catch (Exception e)
            {
                Debug.LogError($"Exception thrown loading save {e.Message}");
            }
        }
    }

    public void SaveStats()
    {
        string path = Path.Combine(Environment.CurrentDirectory, $"{Environment.UserName}_Save.frgSave");
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using (FileStream fs = File.Create(path))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(loadableVersion);

                if (distanceThroughLevel > furthestDistanceThroughLevel)
                {
                    sw.WriteLine(distanceThroughLevel);
                }
                else
                {
                    sw.WriteLine(furthestDistanceThroughLevel);
                }
                sw.WriteLine(distanceThroughLevel);
                sw.WriteLine(Money);
                sw.WriteLine(MaxBubbleCountMod);
                sw.WriteLine(MaxBubbleSizeMod);
                sw.WriteLine(InflatingSpeedMod);

                if (TimeSinceStart < FastestTimeTakenToComplete && distanceThroughLevel == 1)
                {
                    sw.WriteLine(TimeSinceStart);
                }
                else
                {
                    sw.WriteLine(FastestTimeTakenToComplete);
                }

                sw.Flush();
            }
        }
    }
}
