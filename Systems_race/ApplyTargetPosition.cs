using UnityEngine;

public class ApplyTargetPosition : MonoBehaviour
{
    [SerializeField] private Transform _transformTarget;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    private void LateUpdate() => transform.position = _transformTarget.position + _offset;
}
