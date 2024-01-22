using System.Linq;
using _Assets.Scripts.Wallet;
using UnityEngine;

public class CareerReward : MonoBehaviour
{
    [SerializeField] private SpecialOfferForWindow _winWindow;
    [SerializeField] private CareerProgress _careerProgress;
    [SerializeField] private DoubleRewardAction _rewardAction;

    private void Awake()
    {
        _careerProgress.OnFinish += FinishPlayer;
    }

    private void FinishPlayer(ResultDataCareerMission missionData)
    {
        WalletManager.AddMoney(missionData.Reward, TypeOfCurrensy.Gold);
        int totalExp = missionData.TricksInfo.Select(info => info.Experience).Sum();
        PlayerLevel.AddExperience(totalExp);
        _rewardAction.RewardValue = missionData.Reward;
        _winWindow.SetReward(_rewardAction);
    }
}