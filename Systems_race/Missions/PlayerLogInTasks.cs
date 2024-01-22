using UnityEngine;
using System;

public class PlayerLogInTasks : MonoBehaviour
{
    [SerializeField] private PlayerTask[] _tasksDayInGame = new PlayerTask[0];
    [SerializeField] private PlayerTask[] _tasksTimeInGame = new PlayerTask[0];

    private void Awake()
    {
        foreach (var task in _tasksDayInGame)
        {
            SaveTaskState(task, PlayerStatistics.DayInGameRow);
        }
    }

    private void OnEnable()
    {
        int minutes = (DateTime.Now.ToUniversalTime() - PlayerStatistics.StartGameSession).Minutes;
        Debug.Log("Player in game " +  StringExtensions.MinutesToTimeFormat(minutes));

        foreach (var task in _tasksTimeInGame)
        {
            SaveTaskState(task, minutes);
        }
    }

    private void SaveTaskState(PlayerTask task, int value)
    {
        task.WriteProgressValueWithBorderTargetValue(value);
        if (task.IsCompleted)
            task.Save();
    }
}
