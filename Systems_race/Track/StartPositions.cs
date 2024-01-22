using System;
using System.Collections;
using UnityEngine;

namespace Tracking
{
    public class StartPositions : MonoBehaviour
    {
        [SerializeField] private Transform[] _startPoints;

        private Transform[] _players;
        private int _countPlayers;

        public void SetInPlace(params Transform[] players)
        {
            foreach (var player in players)
            {
                if (_countPlayers >= _startPoints.Length)
                    ResolveConflict();

                player.SetPositionAndRotation(_startPoints[_countPlayers].position, _startPoints[_countPlayers].rotation);
                _countPlayers++;
            }
        }

        public void Clear()
        {
            _countPlayers = 0;
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = null;
            }
        }

        private void Awake() => _players = new Transform[_startPoints.Length];

        private void ResolveConflict()
        {
        }
    }
}