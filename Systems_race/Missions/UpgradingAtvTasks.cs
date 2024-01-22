using System;
using UnityEngine;
using GameManager;

public class UpgradingAtvTasks : MonoBehaviour
{
    [SerializeField] private PlayerTask[] _tasks = new PlayerTask[0];

    private void OnEnable() => StateChange();

    private void StateChange()
    {
        int maxValue = 0;
        int current = 0;

        int[] UpgradesChain = UWorld.playerSaveData.UpgradesChain;
        int[] UpgradesEngine = UWorld.playerSaveData.UpgradesEngine;
        int[] UpgradesClutch = UWorld.playerSaveData.UpgradesClutch;
        int[] UpgradesSprings = UWorld.playerSaveData.UpgradesSprings;

        for (int i = 0; i < UpgradesChain.Length; i++)
        {
            current = UpgradesChain[i] + UpgradesEngine[i] + UpgradesClutch[i] + UpgradesSprings[i];

            if (current >= maxValue)
                maxValue = current;
        }

        Debug.Log("ATV purchasing " + maxValue);
        foreach (var task in _tasks)
        {
            task.AddProgressValueWithBorderTargetValue(maxValue);
        }
    }
}