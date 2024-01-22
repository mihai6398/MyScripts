using System;
using UnityEngine;

namespace Tracking
{
    public class TrackController : MonoBehaviour
    {
        public event Action<Track> OnLoadTrack;

        public Track Track => _track;

        [SerializeField] private Track[] _tracksList;
        [SerializeField, Range(0, 5)] private int _loadingTrack;
        [SerializeField] private bool isLoadOnStart = true;

        [SerializeField] private Transform[] _players;
        private Track _track;

        public void Prepare(int indexTrack)
        {
            _tracksList[indexTrack].Prepare();
        }

        public void SetPlayersInTrack(int indexTrack, params Transform[] players)
        {
            _tracksList[indexTrack].AddPlayers(players);
        }

        public void TrackOn(int indexTrack)
        {
            _track = _tracksList[indexTrack];
            _track.StartTrack();

            OnLoadTrack?.Invoke(_track);
        }

        private void Start()
        {
            if (isLoadOnStart)
            {
                SetPlayersInTrack(_loadingTrack, _players);
                TrackOn(_loadingTrack);
            }
        }
    }
}