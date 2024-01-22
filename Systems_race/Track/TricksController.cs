using System;
using System.Linq;
using UnityEngine;

public class TricksController : MonoBehaviour
{
    [SerializeField] private TrickInfo[] _trickInfos;

    private void Start()
    {
        foreach (var trickInfo in _trickInfos)
        {
            trickInfo.Trick.SetTrick(trickInfo.Info);
        }
    }

    public TrickCareerInfo[] GetTrickList() 
        => _trickInfos.Select(info => info.Info).ToArray();

    [Serializable]
    private class TrickInfo
    {
        public TrickOnStartSlowMotion Trick;
        public TrickCareerInfo Info;
    }
}