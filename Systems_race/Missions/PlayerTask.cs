using System;
using _Assets.Scripts.Wallet;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Task", menuName = "ScriptableObjects/New Task")]
public class PlayerTask : ScriptableObject
{
    public bool IsCompleted => !_moreIsBetter ^ (ProgressValue >= TargetValue);
    public bool CanGetReward => IsCompleted && !isClaimedReward;
    public bool CanGetDoubleReward => IsCompleted && !isClaimedDoubleReward;

    public string TaskName;
    [Min(0)] public float TargetValue;
    public int AmountReward;
    public TypeOfCurrensy RewardTypeCurrensy;
    [SerializeField] private bool _moreIsBetter = true;
    public TypeValueTask TypeTask;
    public Sprite Icon;
    [HideInInspector] public float ProgressValue;

    private bool isClaimedReward = false;
    private bool isClaimedDoubleReward = false;

    public bool TryGetReward()
    {
        isClaimedReward = PlayerPrefs.GetFloat(TaskName, 0) < 0;

        if (isClaimedReward)
        {
            Debug.Log(TaskName + " Reward has already been received");
            return false;
        }
        else if (IsCompleted)
        {
            MoneyAccrual();
            return true;
        }

        Debug.Log(TaskName + " Task is not completed");
        return false;
    }
    
    public bool TryGetDoubleReward()
    {
        isClaimedDoubleReward = ReadLastBit(PlayerPrefs.GetFloat(TaskName, 0));

        if (isClaimedDoubleReward)
        {
            Debug.Log(TaskName + " Double reward has already been received");
            return false;
        }
        else if (IsCompleted)
        {
            IronSourceManager.Instance.CallAddAndExecuteFunctionAfterSuccess(RewardAdsPlacement.TASK_DOUBLE_REWARD, MoneyAccrual);
            return true;
        }

        Debug.Log(TaskName + " Task is not completed");
        return false;
    }

    public void Save()
    {
        float saveValue = isClaimedReward ? -ProgressValue : ProgressValue;
        saveValue = SetLastBit(saveValue, isClaimedDoubleReward);
        PlayerPrefs.SetFloat(TaskName, saveValue);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        float saveValue = PlayerPrefs.GetFloat(TaskName, 0);
        isClaimedReward = saveValue < 0;
        isClaimedDoubleReward = ReadLastBit(saveValue);
        ProgressValue = SetLastBit(Mathf.Abs(saveValue), false) ;
        Debug.Log(String.Format("{0,-20}\tsaved Value: {1,-7}\tLoad data -> isTakeReward: {2};\tProgress Value: {3}", TaskName, saveValue, isClaimedReward, ProgressValue));
    }

    //TODO: check _moreIsBetter
    public void AddProgressValueWithBorderTargetValue(int amount)
    {
        if (IsCompleted)
        {
            ProgressValue = TargetValue;
            return;
        }

        ProgressValue = ProgressValue + amount;
        if (ProgressValue > TargetValue)
        {
            ProgressValue = TargetValue;
        }
    }

    //TODO: check _moreIsBetter
    public void WriteProgressValueWithBorderTargetValue(int amount)
    {
        if (IsCompleted)
        {
            ProgressValue = TargetValue;
            return;
        }

        ProgressValue = amount > TargetValue ? TargetValue : amount;
    }

    private void MoneyAccrual()
    {
        if (CanGetReward)
        {
            isClaimedReward = true;
            WalletManager.AddMoney(AmountReward, RewardTypeCurrensy);
            Debug.Log(TaskName + $" Reward {AmountReward} {RewardTypeCurrensy} accrued");
        }
        else if (CanGetDoubleReward)
        {
            isClaimedDoubleReward = true;
            WalletManager.AddMoney(AmountReward, RewardTypeCurrensy);
            Debug.Log(TaskName + $" Double reward {AmountReward} {RewardTypeCurrensy} accrued");
        }
        
        Save();
    }

    private float SetLastBit(float value, bool newState)
    {
        int bits;
        
        if (newState)
        {
            int mask = 0b0000_0000_0000_0000_0000_0000_0000_0001;
            bits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            bits |= mask;
        }
        else
        {
            int mask = unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_1110);
            bits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            bits &= mask;
        }

        return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
    }
    
    private bool ReadLastBit(float number)
    {
        int bits = BitConverter.ToInt32(BitConverter.GetBytes(number), 0);
        return (bits & 0x00000001) == 0x00000001;
    }

    public enum TypeValueTask
    {
        Count,
        TimeMinutes,
        TimeSeconds
    }
}
