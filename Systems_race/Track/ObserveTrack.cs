using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tracking
{
    public class ObserveTrack
    {
        public event Action<PlayerTrackInfo> OnFinishEnter = delegate(PlayerTrackInfo info) {  };
        public event Action<PlayerTrackInfo> OnCheckpointEnter = delegate(PlayerTrackInfo info) {  };

        public List<PlayerTrackInfo> Players = new List<PlayerTrackInfo>();

        private readonly Dictionary<Transform, ObserveTargetTrackProgress> _playersObservers = new Dictionary<Transform, ObserveTargetTrackProgress>();

        private Timer _timer;
        private int _prize = 1;
        TrackWay _track;

        public ObserveTrack(Transform[] players, TrackWay track, Timer timer)
        {
            ObserveTargetTrackProgress observeTarget;
            _timer = timer;

            track.OnCheckpointEnter += CheckpointEnter;
            track.OnFinishEnter += FinishEnter;

            for (int i = 0; i < players.Length; i++)
            {
                observeTarget = new ObserveTargetTrackProgress(players[i], track);
                _playersObservers.Add(players[i], observeTarget);
                Players.Add(_playersObservers[players[i]].Info);
            }
        }

        ~ObserveTrack()
        {
            _track.OnCheckpointEnter -= CheckpointEnter;
            _track.OnFinishEnter += FinishEnter;
        }

        public Dictionary<Transform, ObserveTargetTrackProgress> RefreshState()
        {
            foreach (var item in _playersObservers.Values)
            {
                item.CalculateProgress();
            }

            var sotrProgress = Players.Where(i => i.FinishRanking == 0).OrderBy(info => info.ProgressTrack).ToArray();

            for (int i = 0; i < sotrProgress.Length; i++)
            {
                if (sotrProgress[i].ProgressTrack <= 0)
                    sotrProgress[i].Ranking = Players.Count;
                else
                    sotrProgress[i].Ranking = Players.Count - i;
            }

            return _playersObservers;
        }

        public PlayerTrackInfo TryGetInfo(Transform target)
        {
            if (_playersObservers.ContainsKey(target))
                return _playersObservers[target].Info;

            return null;
        }

        private void CheckpointEnter(TrackWay.PlayerWayInfo player)
        {
            if (!_playersObservers.ContainsKey(player.transform))
                return;

            PlayerTrackInfo playerInfo = _playersObservers[player.transform].Info;
            WriteInfo(player, playerInfo);
            OnCheckpointEnter.Invoke(playerInfo);
        }

        private void WriteInfo(TrackWay.PlayerWayInfo player, PlayerTrackInfo playerInfo)
        {
            playerInfo.wayPoint = player.WayPoint;
            playerInfo.CircleNumber = player.CircleNumber;
        }

        private void FinishEnter(TrackWay.PlayerWayInfo player)
        {
            if (!_playersObservers.ContainsKey(player.transform))
                return;

            PlayerTrackInfo playerInfo = _playersObservers[player.transform].Info;
            playerInfo.FinishRanking = _prize++;
            playerInfo.TrackTime = _timer.CurrentTime;
            OnFinishEnter.Invoke(playerInfo);
        }
    }
}