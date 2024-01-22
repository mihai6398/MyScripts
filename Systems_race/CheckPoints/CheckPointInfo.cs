using UnityEngine;

namespace Tracking
{
    public partial class WayInfo
    {
        public class CheckPointInfo
        {
            public float Position => _position;
            public float Distance => _distance;
            public Vector3 Сoordinates => _coordinates;
 
            private float _position;
            private float _distance;
            private Vector3 _coordinates;

            public CheckPointInfo(Vector3 coordinates, float position, float distance)
            {
                _position = position;
                _distance = distance;
                _coordinates = coordinates;
            }

            public override string ToString()
            {
                return $"Position {Position.ToString("P")},\tDistance {Distance.ToString("0.00")},\t Сoordinates {Сoordinates}";
            }
        }
    }
}