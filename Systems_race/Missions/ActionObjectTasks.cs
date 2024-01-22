using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObjectTasks : MonoBehaviour
{
    [SerializeField] private ActionObject _action;
    [SerializeField] private PlayerTask[] _tasks = new PlayerTask[0];

    private void Awake() => _action.OnSuccessfully += StateChange;

    private void OnDestroy() => _action.OnSuccessfully -= StateChange;

    private void StateChange()
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
