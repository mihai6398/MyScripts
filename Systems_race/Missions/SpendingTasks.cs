using System;
using System.Collections;
using System.Collections.Generic;
using _Assets.Scripts.Wallet;
using GameManager;
using UnityEngine;

public class SpendingTasks : MonoBehaviour
{
    [SerializeField, Header("Coins")] private PlayerTask[] _tasksCoins = new PlayerTask[0];
    [SerializeField, Header("Gems")] private PlayerTask[] _tasksGems = new PlayerTask[0];

    void Start()
    {
        UWorld.transaction.OnPay += Spending;
        LoadAllTask(_tasksCoins);
        LoadAllTask(_tasksGems);
    }

    void OnDestroy() => UWorld.transaction.OnPay -= Spending;

    private void Spending(int amount, TypeOfCurrensy typeCurrensy)
    {
        Debug.Log("Spending " + amount + typeCurrensy);

        if(typeCurrensy == TypeOfCurrensy.Gold)
        {
            Write(_tasksCoins, amount);
        }
        else
        {
            Write(_tasksGems, amount);
        }
    }

    private void Write(ICollection<PlayerTask> taskCollection, int amount)
    {
        foreach (var task in taskCollection)
        {
            task.AddProgressValueWithBorderTargetValue(amount);
            task.Save();
        }
    }

    private void LoadAllTask(ICollection<PlayerTask> taskCollection)
    {
        foreach (var task in taskCollection)
        {
            task.Load();
        }
    }
}
