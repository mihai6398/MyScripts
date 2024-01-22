using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(TrickSlowMotion))]
public class TrickOnStartSlowMotion : MonoBehaviour
{
    public event Action<TrickCareerInfo> OnStartTrick = delegate(TrickCareerInfo info) {  };

    [SerializeField] private TricksButtons _buttons;
    [SerializeField, Range(0, 10)] private int _indexTrick;
    
    private TrickSlowMotion _slowMotion;
    private TrickCareerInfo _trickInfo;

    public void SetTrick(TrickCareerInfo trickInfo)
    {
        _trickInfo = trickInfo;
        _indexTrick = _trickInfo.Index;
    }
    
    private void Awake()
    {
        _slowMotion = GetComponent<TrickSlowMotion>();
        _slowMotion.OnStartSlowMotion += StartSlowMotion;
        _slowMotion.OnEndSlowMotion += EndSlowMotion;
    }

    private void OnDestroy()
    {
        _slowMotion.OnStartSlowMotion -= StartSlowMotion;
        _slowMotion.OnEndSlowMotion -= EndSlowMotion;
    }

    private void StartSlowMotion()
    {
        _buttons.onClick += ClickButtons;
        _buttons.SetTrick(_trickInfo);
    }

    private void EndSlowMotion()
    {
        _buttons.onClick -= ClickButtons;
        _buttons.HideButtons();
    }

    private void ClickButtons(int i) => StartTrick();

    private void StartTrick()
    {
        _buttons.onClick -= ClickButtons;
        
        _trickInfo ??= new TrickCareerInfo(_indexTrick);

        _trickInfo.IsDone = true;
        OnStartTrick.Invoke(_trickInfo);
    }
    
    private class TrickSettings
    {
        public int IndexTrick;
    }
}