using GameManager;
using UnityEngine;

public class CareerTasks : MonoBehaviour
{
    [SerializeField] private PlayerTask[] _tasks = new PlayerTask[0];

    private void Start() => StateChange();

    private void StateChange()
    {
        foreach (var task in _tasks) 
            task.AddProgressValueWithBorderTargetValue(UWorld.playerSaveData.CareerLvl);
    }
}