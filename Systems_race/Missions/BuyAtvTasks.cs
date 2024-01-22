using System.Linq;
using UnityEngine;
using GameManager;

public class BuyAtvTasks : MonoBehaviour
{
    [SerializeField] private PlayerTask[] _tasks = new PlayerTask[0];
    [SerializeField] private bool _firstDoesNotCount;

    private void OnEnable() => CheckState();

    private void CheckState()
    {
        int amount = UWorld.playerSaveData.purchasedCar.Where(pur => pur).ToArray().Length;
        
        if(_firstDoesNotCount)
            amount--;
        
        Debug.Log("ATV purchasing " + amount);
        foreach (var task in _tasks)
        {
            task.WriteProgressValueWithBorderTargetValue(amount);
        }
    }
}
