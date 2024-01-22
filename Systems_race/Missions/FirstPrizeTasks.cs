using UnityEngine;

public class FirstPrizeTasks : MonoBehaviour
{
    [SerializeField] CareerProgress _progress;
    [SerializeField] private PlayerTask[] _tasks = new PlayerTask[0];

    private void Start() => _progress.OnFinish += Finish;

    private void OnDestroy() => _progress.OnFinish -= Finish;

    private void Finish(ResultDataCareerMission missionData)
    {
        if (!missionData.IsWin)
            return;

        foreach (var task in _tasks)
        {
            task.AddProgressValueWithBorderTargetValue(1);
            task.Save();
        }
    }
}
