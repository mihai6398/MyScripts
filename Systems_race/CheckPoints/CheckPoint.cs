using System;
using UnityEngine;

namespace Tracking
{
    [RequireComponent(typeof(Collider))]
    public class CheckPoint : MonoBehaviour
    {
        public event Action<CheckPointEnterInfo> OnEnter = delegate(CheckPointEnterInfo info) {  };

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) 
            => OnEnter.Invoke(new CheckPointEnterInfo(this, other.transform));
    }

    public class CheckPointEnterInfo
    {
        public readonly CheckPoint CheckPoint;
        public readonly Transform EnteredObject;

        public CheckPointEnterInfo(CheckPoint checkPoint, Transform enterObject)
        {
            CheckPoint = checkPoint;
            EnteredObject = enterObject;
        }
    }
}