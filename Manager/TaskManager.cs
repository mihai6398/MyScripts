using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [SerializeField] private Transform taskLisTransform;
    [SerializeField] private GameObject taskListTeamplate; // recomand Prefab
    
    [SerializeField] private List<TaskData> taskListTaskData; // recomand Prefab
    
    public List<GameTask> activeTasks;

    private void Start()
    {
        for (int i = 0; i < activeTasks.Count; i++)
        {
            var gameObject = Instantiate(taskListTeamplate, taskLisTransform);
            taskListTaskData.Add(gameObject.GetComponent<TaskData>());
        }

        for (int i = 0; i < taskListTaskData.Count; i++)
        {

            taskListTaskData[i].textContent.text = activeTasks[i].taskContent;
            taskListTaskData[i].textProgress.text = $"{activeTasks[i].currentProgress}/{activeTasks[i].progress}";
        }
    }

    public void TaskProgress(string taskName)
    {
        
        for (int i = 0; i < activeTasks.Count; i++)
        {
            
            if (activeTasks[i].task == taskName)
            {
                ++activeTasks[i].currentProgress;
                taskListTaskData[i].textContent.text = activeTasks[i].taskContent;
                taskListTaskData[i].textProgress.text = $"{activeTasks[i].currentProgress}/{activeTasks[i].progress}";
                
            

                if (activeTasks[i].currentProgress == activeTasks[i].progress || activeTasks[i].currentProgress <= activeTasks[i].progress) // Done verification
                {
                    activeTasks[i].done = true;
                    SetGameStatus();
                }
                break;
            }
        }
    }

    
    private void SetGameStatus()
    {
        if (CheckAllTasksDone())
        {
            WinSystem.Instance.SetGameStatusWin();
        }
    }
    private bool CheckAllTasksDone()
    {
        // Verifică dacă toate task-urile sunt cu status done
        for (int i = 0; i < activeTasks.Count; i++)
        {
            if (!activeTasks[i].done)
            {
                return false; 
            }
        }
        return true; 
    }
    
    [System.Serializable]
    public class GameTask
    {
        public string task;
        public string taskContent;
        public int currentProgress;
        public int progress;
        public bool done;
    }

}
