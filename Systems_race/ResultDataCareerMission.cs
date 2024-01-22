public struct ResultDataCareerMission
{
    public bool IsWin;
    public int Reward;
    public float PlayerTime;
    public float TargetTime;
    public TrickCareerInfo[] TricksInfo;

    public ResultDataCareerMission(bool isWin, int reward, float playerTime, float targetTime, TrickCareerInfo[] tricksInfo)
    {
        IsWin = isWin;
        Reward = reward;
        PlayerTime = playerTime;
        TargetTime = targetTime;
        TricksInfo = tricksInfo;
    }
}