using UnityEngine;

namespace Tracking
{
    public class PlayerTrackInfo : MonoBehaviour
    {
        public int wayPoint = 0;
        public int CircleNumber = 0;
        public int Ranking = 1;
        public float ProgressTrack = 0;
        public int FinishRanking = 0;
        public float TrackTime = float.PositiveInfinity;
        public int TrickDone = 0;
    }
}