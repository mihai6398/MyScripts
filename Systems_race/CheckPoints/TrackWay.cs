using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tracking
{
    public class TrackWay : MonoBehaviour
    {
        public event Action<PlayerWayInfo> OnFinishEnter = delegate {  };
        public event Action<PlayerWayInfo> OnCheckpointEnter = delegate {  };
        public event Action<Transform> OnFinishEnterDoNoPassAllCheckPoint = delegate {  };
        
        public WayInfo WayInfo => _wayInfo;

        [Tooltip("Last check point in list - finish")]
        [SerializeField] private CheckPoint[] _way;
        [Tooltip("For sprints set value 1")]
        [SerializeField, Min(1)] private int _countCircle = 1;

        private int _prize = 1;
        private readonly Dictionary<Transform, PlayerWayInfo> _playerProgress = new Dictionary<Transform, PlayerWayInfo>();
        private readonly HashSet<CheckPoint> _checkPoints = new HashSet<CheckPoint>();
        private WayInfo _wayInfo;

        private void Awake()
        {
            AddListeners();
            _wayInfo = new WayInfo(_way, _countCircle);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            foreach (var checkPoint in _way)
            {
                if (_checkPoints.Contains(checkPoint)) continue;
                
                _checkPoints.Add(checkPoint);
                checkPoint.OnEnter += PlayerEnter;
            }
        }
        
        private void RemoveListeners()
        {
            foreach (var checkPoint in _checkPoints) 
                checkPoint.OnEnter -= PlayerEnter;

            _checkPoints.Clear();
        }

        private void PlayerEnter(CheckPointEnterInfo checkEnterInfo)
        {
            RegisterInTrack(checkEnterInfo);
            var player = SetNextPoint(checkEnterInfo);
            CheckFinish(player);
        }

        private void RegisterInTrack(CheckPointEnterInfo checkEnterInfo)
        {
            if (_playerProgress.ContainsKey(checkEnterInfo.EnteredObject)) return;

            _playerProgress.Add(checkEnterInfo.EnteredObject, new PlayerWayInfo(checkEnterInfo.EnteredObject));
        }

        private PlayerWayInfo SetNextPoint(CheckPointEnterInfo checkPointInfo)
        {
            var player = _playerProgress[checkPointInfo.EnteredObject];
            if(player.Finished != 0)
                return player;
            
            if (_way[player.WayPoint] == checkPointInfo.CheckPoint)
            {
                player.WayPoint++;
                if (player.WayPoint >= _way.Length)
                {
                    player.WayPoint = 0;
                    player.CircleNumber++;
                }
                OnCheckpointEnter.Invoke(player);
            }
            else if (_way[^1] == checkPointInfo.CheckPoint && player.WayPoint > 0 && _way[player.WayPoint - 1] != checkPointInfo.CheckPoint)
            {
                OnFinishEnterDoNoPassAllCheckPoint.Invoke(player.transform);
            }

            return player;
        }

        private void CheckFinish(PlayerWayInfo player)
        {
            if(player.CircleNumber >= _countCircle && player.Finished == 0)
                Finish(player);
        }

        private void Finish(PlayerWayInfo player)
        {
            player.Finished = _prize++;
            OnFinishEnter.Invoke(player);
        }

        public class PlayerWayInfo
        {
            public readonly Transform transform;
            public int WayPoint = 0;
            public int CircleNumber = 0;
            public int Finished = 0;

            public PlayerWayInfo(Transform playerTransform)
            {
                transform = playerTransform;
            }
        }
    }
}