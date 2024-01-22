using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tracking
{
    public class Track : MonoBehaviour
    {
        public event Action<Transform, float, int> OnFinishEnter = delegate {  };
        public event Action<PlayerTrackInfo> OnCheckpointEnter = delegate {  };
        public event Action<Transform> OnFinishEnterDoNoPassAllCheckPoint = delegate {  };

        public int CountMembers => _players.Count;
        public List<PlayerTrackInfo> Players => _players;
        public WayInfo Way => _track.WayInfo;

        [Header("Way")]
        [SerializeField] private StartPositions _startPositions;
        [SerializeField] private TrackWay _track;
        [SerializeField] private RCC_AIWaypointsContainer _aiWaypoints;
        [Header("Settings")]
        [Range(0, 10)] private float _startDelay = 5f;
        [SerializeField] private СountdownBoard _сountdown;
        [SerializeField] private Timer _timer;

        private readonly Dictionary<Transform, RCC_CarControllerV3> _playersControllers = new Dictionary<Transform, RCC_CarControllerV3>();
        private List<PlayerTrackInfo> _players = new List<PlayerTrackInfo>();
        private ObserveTrack _observeTrack;
        private Coroutine _observeRefresh;

        public void AddPlayers(params Transform[] players)
        {
            if (players == null)
                return;

            AddToTrack(players);
            StopPlayers(players);
        }

        public void AddPlayers(params RCC_CarControllerV3[] players)
        {
            if (players == null)
                return;

            AddToTrack(players);
            StopPlayers(players);
        }

        public void StartTrack()
        {
            SetObserveTrack();

            _timer.StartTimer(_startDelay);
            StartCoroutine(PlayersOnAfter(_startDelay));

            if (_observeRefresh != null)
                StopCoroutine(_observeRefresh);

            _observeRefresh = StartCoroutine(Observe());

            if (_сountdown != null)
                _сountdown.StartСountdown(_startDelay);
        }

        public void Prepare()
        {
            _startPositions.Clear();
            _playersControllers.Clear();
            _players.Clear();
            _observeTrack = null;
        }

        public PlayerTrackInfo TryGetInfo(Transform target)
        {
            return _observeTrack.TryGetInfo(target);
        }

        private void Start() 
            => _track.OnFinishEnterDoNoPassAllCheckPoint += DoNoPass;

        private void SetObserveTrack()
        {
            if (_observeTrack != null)
            {
                _observeTrack.OnFinishEnter -= FinishEnter;
                _observeTrack.OnCheckpointEnter -= CheckpointEnter;
            }

            _observeTrack = new ObserveTrack(_playersControllers.Keys.ToArray(), _track, _timer);
            _observeTrack.OnFinishEnter += FinishEnter;
            _observeTrack.OnCheckpointEnter += CheckpointEnter;
            _players = _observeTrack.Players;
        }

        private IEnumerator Observe()
        {
            while(true)
            {
                yield return new WaitForSeconds(0.1f);
                _observeTrack.RefreshState();
            }
        }

        private void DoNoPass(Transform obj) => OnFinishEnterDoNoPassAllCheckPoint.Invoke(obj);

        private void CheckpointEnter(PlayerTrackInfo playerInfo) 
            => OnCheckpointEnter.Invoke(playerInfo);

        private void FinishEnter(PlayerTrackInfo player)
        {
            StartCoroutine(Braking(_playersControllers[player.transform]));
            OnFinishEnter.Invoke(player.transform, _timer.CurrentTime, player.FinishRanking);
        }

        private void TrySetAiWay(RCC_CarControllerV3 controller)
        {
            RCC_AICarController aiCarController;

            if (controller.TryGetComponent<RCC_AICarController>(out aiCarController))
            {
                aiCarController.waypointsContainer = _aiWaypoints;
            }
        }

        private void SetAiWay(RCC_AICarController aiController)
        {
            aiController.waypointsContainer = _aiWaypoints;
        }

        private IEnumerator PlayersOnAfter(float startDelay)
        {
            yield return new WaitForSeconds(startDelay);
            AllPlayersOn();
        }

        private void AllPlayersOn()
        {
            foreach (var player in _playersControllers.Values)
            {
                EnablePlayerControl(player);
            }
        }

        private void AddToTrack(params Transform[] players)
        {
            RCC_CarControllerV3 carController;

            foreach (var player in players)
            {
                if (player.TryGetComponent<RCC_CarControllerV3>(out carController))
                {
                    if (!_playersControllers.ContainsKey(player))
                    {
                        AddToList(player, carController);
                        player.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void AddToTrack(params RCC_CarControllerV3[] players)
        {
            foreach (var player in players)
            {
                if (!_playersControllers.ContainsKey(player.transform))
                {
                    AddToList(player.transform, player);
                }
            }
        }

        private void AddToList(Transform playerTransform, RCC_CarControllerV3 carController)
        {
            _playersControllers.Add(playerTransform, carController);
            _startPositions.SetInPlace(playerTransform);
            TrySetAiWay(carController);
        }

        private void StopPlayers(Transform[] players)
        {
            foreach (var player in players)
            {
                if (_playersControllers.ContainsKey(player))
                    StopPlayer(_playersControllers[player]);
            }
        }
        
        private void StopPlayers(RCC_CarControllerV3[] players)
        {
            foreach (var player in players)
            {
                StopPlayer(player);
            }
        }

        private void StopPlayer(Transform player) => StopPlayer(_playersControllers[player]);

        private void StopPlayer(RCC_CarControllerV3 player) => DisablePlayerControl(player);

        private IEnumerator Braking(RCC_CarControllerV3 player)
        {
            DisablePlayerControl(player);

            var rb = player.GetComponent<Rigidbody>();
            float timer = 0f;

            while (timer < 2f)
            {   
                timer += Time.deltaTime;
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.7f * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        }

        private void EnablePlayerControl(RCC_CarControllerV3 player)
        {
            player.handbrakeInput = 0f;
            player.SetCanControl(true);
        }

        private void DisablePlayerControl(RCC_CarControllerV3 player)
        {
            player.SetCanControl(false);
            player.handbrakeInput = 1f;
        }

        private void OnDestroy()
        {
            _track.OnFinishEnterDoNoPassAllCheckPoint -= DoNoPass;
            
            if (_observeTrack != null)
            {
                _observeTrack.OnFinishEnter -= FinishEnter;
                _observeTrack.OnCheckpointEnter -= CheckpointEnter;
            }
        }
    }
}