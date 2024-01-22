using UnityEngine;

public class SetActiveOnLastCareerLvl : MonoBehaviour
{
    [SerializeField] private bool _setEnable;
    
    private void Start()
    {
        if(CareerProgress.COUNT_LVL_CAREER >= CareerProgress.CurrentLvl)
            return;

        gameObject.SetActive(_setEnable);
    }
}