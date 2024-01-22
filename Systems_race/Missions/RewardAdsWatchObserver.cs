using System;
using System.Collections;
using UnityEngine;

public class RewardAdsWatchObserver : Singleton<RewardAdsWatchObserver>
{
    [SerializeField] private PlayerTask[] _tasks = new PlayerTask[0];

    void Start()
    {
        LoadAllTask();
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardAdsWatched;
    }

    private void OnDestroy() => IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardAdsWatched;

    private void RewardAdsWatched(IronSourcePlacement obj)
    {
        foreach (var task in _tasks)
        {
            task.AddProgressValueWithBorderTargetValue(1);
            task.Save();
        }
    }

    private void LoadAllTask()
    {
        foreach (var task in _tasks)
        {
            task.Load();
        }
    }
}
