using Tracking;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class RespawnCar : MonoBehaviour
{
    [SerializeField] private Button _onRespawnButton;
    [SerializeField] private TrackController _trackController;
    [SerializeField] private float _yOffset = 0.5f;
    
    private Rigidbody _rigidbody;
    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;
    private Track _track;

    public void Respawn()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        Time.timeScale = 1;
        
        transform.SetPositionAndRotation(_respawnPosition, _respawnRotation);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _onRespawnButton.onClick.AddListener(Respawn);
        
        SavePosition();
        _trackController.OnLoadTrack += SaveStartPosition;
    }

    private void SaveStartPosition(Track track)
    {
        if (_track != null)
            _track.OnCheckpointEnter -= SavePosition;

        _track = track;
        track.OnCheckpointEnter += SavePosition;
        SavePosition();
    }

    private void SavePosition(PlayerTrackInfo obj) 
        => SavePosition();

    private void SavePosition()
    {
        _respawnPosition = transform.position + Vector3.up * _yOffset;
        _respawnRotation = transform.rotation;
    }


    private void OnDestroy()
    {
        _onRespawnButton.onClick.RemoveListener(Respawn);
        _trackController.OnLoadTrack -= SaveStartPosition;
        
        if (_track != null)
            _track.OnCheckpointEnter -= SavePosition;
    }
}
