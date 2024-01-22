using System;
using System.Linq;
using GameManager;
using UnityEngine;

public class CareerProgress : MonoBehaviour
{
    public event Action<ResultDataCareerMission> OnFinish = delegate {  };
    public event Action OnFinishDoNoPass = delegate {  };

    public const int COUNT_LVL_CAREER = 100;
    private const int LVL_CAREER_OFFSET = 3;
    private const int COUNT_LVL_CYCLE = 20;

    private static readonly int[] RewardsValue =
    {
        820,   960,   990,   1080,  1120,  1190,  1240,  1270,  1350,  1460,  1490,  1530,  1570,  1650,  1710,  1770,  1840,  1890,  1950,  2080,
        2140,  2220,  2280,  2360,  2420,  2490,  2540,  2610,  2680,  2750,  2810,  2880,  2940,  3050,  3120,  3190,  3270,  3360,  3440,  3570, 
        3690,  3810,  3990,  4090,  4180,  4270,  4440,  4670,  4890,  4990,  5230,  5460,  5680,  5860,  5980,  6360,  6540,  6750,  6980,  7230,
        7480,  7670,  7940,  8370,  8560,  8790,  8990,  9250,  9470,  9650,  9880,  10120, 10250, 10470, 10650, 10830, 10990, 11240, 11540, 11680,
        11980, 12340, 12560, 12760, 12980, 13240, 13560, 13860, 14130, 14450, 14670, 14980, 15460, 15680, 15920, 16370, 16580, 16950, 17380, 17500
    };
    
    private static readonly float[] TargetsTime = {
        360f, 350f, 340f, 330f, 320f, 280f, 270f, 265f, 263f, 260f, 258f, 256f, 254f, 253f, 251f, 250f, 240f, 238f, 230f, 225f,
        220f, 218f, 215f, 213f, 210f, 208f, 205f, 202f, 200f, 198f, 196f, 195f, 192f, 192f, 191f, 190f, 190f, 190f, 180f, 180f,
        178f, 176f, 175f, 172f, 171f, 170f, 168f, 165f, 164f, 162f, 160f, 158f, 156f, 154f, 152f, 150f, 148f, 145f, 153f, 141f,
        138f, 135f, 134f, 130f, 128f, 125f, 124f, 122f, 121f, 118f, 116f, 115f, 113f, 111f, 110f, 108f, 106f, 105f, 102f, 160f,
        158f, 155f, 153f, 150f, 148f, 145f, 143f, 140f, 139f, 138f, 137f, 136f, 135f, 135f, 135f, 134f, 133f, 132f, 131f, 120f};
    
    public static int CurrentLvl => _indexCareerMap;

    [SerializeField] private ObservePlayerTrackProgress _targetEvents;
    [SerializeField] private TricksController _tricksController;

    private TrickCareerInfo[] _tricks;
    private static int _indexCareerMap;

    public static int GetReward(int careerLvl) => RewardsValue[careerLvl - 1];
    public static int GetIndexScene(int careerLvl) => (careerLvl - 1) % COUNT_LVL_CYCLE + LVL_CAREER_OFFSET;

    public static void SetLevel(int lvlMap)
    {
        _indexCareerMap = lvlMap;
        Debug.Log("Set career Level " + _indexCareerMap);
    }
    
    public static void SetNextLevel()
    {
        _indexCareerMap++;
        Debug.Log("Set career Level " + _indexCareerMap);
    }

#if UNITY_EDITOR
    [ContextMenu("Convert Time")]
    public void ConvertTime()
    {
        string input = "(06:00:00);(05:50:00);(05:40:00);(05:30:00);(05:20:00);(04:40:00);(04:30:00);(04:25:00);(04:23:00);(04:20:00);(04:18:00);(04:16:00);(04:14:00);(04:13:00);(04:11:00);(04:10:00);(04:00:00);(03:58:00);(03:50:00);(03:45:00);(03:40:00);(03:38:00);(03:35:00);(03:33:00);(03:30:00);(03:28:00);(03:25:00);(03:22:00);(03:20:00);(03:18:00);(03:16:00);(03:15:00);(03:12:00);(03:12:00);(03:11:00);(03:10:00);(03:10:00);(03:10:00);(03:00:00);(03:00:00);(02:58:00);(02:56:00);(02:55:00);(02:52:00);(02:51:00);(02:50:00);(02:48:00);(02:45:00);(02:44:00);(02:42:00);(02:40:00);(02:38:00);(02:36:00);(02:34:00);(02:32:00);(02:30:00);(02:28:00);(02:25:00);(02:33:00);(02:21:00);(02:18:00);(02:15:00);(02:14:00);(02:10:00);(02:08:00);(02:05:00);(02:04:00);(02:02:00);(02:01:00);(01:58:00);(01:56:00);(01:55:00);(01:53:00);(01:51:00);(01:50:00);(01:48:00);(01:46:00);(01:45:00);(01:42:00);(02:40:00);(02:38:00);(02:35:00);(02:33:00);(02:30:00);(02:28:00);(02:25:00);(02:23:00);(02:20:00);(02:19:00);(02:18:00);(02:17:00);(02:16:00);(02:15:00);(02:15:00);(02:15:00);(02:14:00);(02:13:00);(02:12:00);(02:11:00);(02:00:00)";
        string result = "";
        string[] times = input.Split(';');
        times = times.Select(s => s.Trim(new char[]{'(',')'})).ToArray();
        for (int i = 0; i < times.Length; i++)
        {
            string[] time = times[i].Split(":");
            float seconds = int.Parse(time[0]) * 60 + int.Parse(time[1]) + float.Parse(time[2]) / 100;
            result += seconds + "f, ";
        }
        Debug.Log(result);
    }
#endif
    
    private void Start()
    {
        _targetEvents.OnTargetFinish += Finish;
        _targetEvents.OnTargetFinishDoNoPass += DoNoPass;
        _tricks = _tricksController.GetTrickList();
        Debug.Log("Start career lvl " + _indexCareerMap);
    }

    private void OnDestroy()
    {
        _targetEvents.OnTargetFinish -= Finish;
        _targetEvents.OnTargetFinishDoNoPass -= DoNoPass;
    }

    private void DoNoPass() 
        => OnFinishDoNoPass.Invoke();

    private void Finish(float time, int prize)
    {
        bool isWinning = _tricks.Aggregate(true, (isDone, trick) => isDone & trick.IsDone);
        float targetTime = TargetsTime[_indexCareerMap - 1];
        isWinning &= time <= targetTime;
        int reward = isWinning ? GetReward(_indexCareerMap) : 0;

        OnFinish.Invoke(new ResultDataCareerMission(isWinning, reward, time, targetTime, _tricks));
        
        if (isWinning && UWorld.playerSaveData.CareerLvl < _indexCareerMap) 
            UWorld.playerSaveData.CareerLvl = _indexCareerMap;
    }
}