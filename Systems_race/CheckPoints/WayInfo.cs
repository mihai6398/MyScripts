using System;
using UnityEngine;

namespace Tracking
{
    public partial class WayInfo
    {
        public float TotalDistance => _totalDistance;
        public CheckPointInfo[] CheckPoints;
        public int Circle => _circle;
        public int CircleCheckPointCount => _circleCheckPointCount;

        private float _totalDistance = 0f;
        private int _circle = 0;
        private int _circleCheckPointCount = 0;

        public WayInfo(CheckPoint[] way, int circle)
        {
            if (way == null || way.Length == 0 || circle <= 0)
                return;

            _circle = circle;
            _circleCheckPointCount = way.Length;
            _totalDistance = CalculateDistanceWay(way, circle);
            CheckPoints = WriteCheckPoinInfo(way, circle, _totalDistance);
        }

        private CheckPointInfo[] WriteCheckPoinInfo(CheckPoint[] way, int circle, float totalDistance)
        {
            var CheckPointsResult = new CheckPointInfo[way.Length * circle];

            float currentDistance = 0f;
            float distance = 0f;

            for (int c = 0; c < circle; c++)
            {
                if (c > 0)
                    distance = DistanseBetweenPoints(way[^1], way[0]);

                currentDistance += distance;
                CheckPointsResult[c * way.Length] = new CheckPointInfo(way[0].transform.position, currentDistance / totalDistance, distance);

                for (int i = 1; i < way.Length; i++)
                {
                    distance = DistanseBetweenPoints(way[i], way[i - 1]);
                    currentDistance += distance;
                    CheckPointsResult[i + c * way.Length] = new CheckPointInfo(way[i].transform.position, currentDistance / totalDistance, distance);
                }
            }

            return CheckPointsResult;
        }

        private float CalculateDistanceWay(CheckPoint[] way, int circle)
        {
            float totalDistance = 0;

            for (int c = 0; c < circle; c++)
            {
                if (c > 0)
                    totalDistance += DistanseBetweenPoints(way[^1], way[0]);

                for (int i = 1; i < way.Length; i++)
                {
                    totalDistance += DistanseBetweenPoints(way[i], way[i - 1]);
                }
            }

            return totalDistance;
        }

        private float DistanseBetweenPoints(CheckPoint point1, CheckPoint point2)
        {
            return (point1.transform.position - point2.transform.position).magnitude;
        }

        public WayInfo(Transform[] way)
        {
            if (way == null || way.Length == 0)
                return;

            CheckPoints = new CheckPointInfo[way.Length];

            CheckPoints[0] = new CheckPointInfo(way[0].position, 0f, 0f);

            for (int i = 1; i < way.Length; i++)
            {
                _totalDistance = (way[i].position - way[i - 1].position).magnitude;
            }

            float currentDistance = 0f;
            float distance;
            for (int i = 1; i < way.Length; i++)
            {
                distance = (way[i].position - way[i - 1].position).magnitude;
                currentDistance += distance;
                CheckPoints[i] = new CheckPointInfo(way[i].position, currentDistance / _totalDistance, distance);
            }
        }

        public override string ToString()
        {
            return $"TotalDistance {TotalDistance}, CheckPoints {CheckPoints.Length}";
        }
    }
}