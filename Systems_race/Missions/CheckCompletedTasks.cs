using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCompletedTasks : MonoBehaviour
{
    [SerializeField] private PlayerTask[] _traceableTasks = new PlayerTask[0];
    [SerializeField] private PlayerTask[] _verificationTasks = new PlayerTask[0];
    [SerializeField] private bool _includingThisTask = false;

    private int _completed = 0;

    private void OnEnable()
    {
        _completed = 0;

        if (_includingThisTask)
            _completed++;

        foreach (var task in _traceableTasks)
        {
            if (task.IsCompleted)
            {
                _completed++;
            }
        }

        foreach (var task in _verificationTasks)
        {
            task.WriteProgressValueWithBorderTargetValue(_completed);
        }
    }
}
