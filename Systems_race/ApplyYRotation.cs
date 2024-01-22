using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyYRotation : MonoBehaviour
{
    [SerializeField] private Transform _transformTarget;
    [SerializeField] private bool _isApplyTransform = true;
    [SerializeField] private bool _useLocalRotation = false;
    [SerializeField] private float _yOffset = 0f;
    [SerializeField] private AxisAply _axisAply = AxisAply.Y;

    Vector3 _eulerRotations;

    private void Awake() => _eulerRotations = _useLocalRotation ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;

    private void LateUpdate()
    {
        switch (_axisAply)
        {
            case AxisAply.X:
                ApplyX();
                return;
            
            case AxisAply.Y:
                ApplyY();
                return;
            
            case AxisAply.Z:
                Applyz();
                return;
        }
        
    }

    private void ApplyX()
    {
        if (_useLocalRotation)
        {
            _eulerRotations = new Vector3(_transformTarget.rotation.eulerAngles.y - transform.parent.rotation.eulerAngles.x - _yOffset, _eulerRotations.y , _eulerRotations.z);
            transform.localRotation = Quaternion.Euler(_eulerRotations);
        }
        else
        {
            _eulerRotations = new Vector3(_transformTarget.rotation.eulerAngles.y + _yOffset, _eulerRotations.y, _eulerRotations.z);
            transform.rotation = Quaternion.Euler(_eulerRotations);
        }

        if (_isApplyTransform)
            transform.position = _transformTarget.position;
    }
    
    private void ApplyY()
    {
        
        if (_useLocalRotation)
        {
            _eulerRotations = new Vector3(_eulerRotations.x, _transformTarget.rotation.eulerAngles.y - transform.parent.rotation.eulerAngles.y - _yOffset, _eulerRotations.z);
            transform.localRotation = Quaternion.Euler(_eulerRotations);
        }
        else
        {
            _eulerRotations = new Vector3(_eulerRotations.x, _transformTarget.rotation.eulerAngles.y + _yOffset, _eulerRotations.z);
            transform.rotation = Quaternion.Euler(_eulerRotations);
        }

        if (_isApplyTransform)
            transform.position = _transformTarget.position;
    }
    
    private void Applyz()
    {
        
        if (_useLocalRotation)
        {
            _eulerRotations = new Vector3(_eulerRotations.x, _eulerRotations.y, _transformTarget.rotation.eulerAngles.y - transform.parent.rotation.eulerAngles.z - _yOffset);
            transform.localRotation = Quaternion.Euler(_eulerRotations);
        }
        else
        {
            _eulerRotations = new Vector3(_eulerRotations.x, _eulerRotations.y, _transformTarget.rotation.eulerAngles.y + _yOffset);
            transform.rotation = Quaternion.Euler(_eulerRotations);
        }

        if (_isApplyTransform)
            transform.position = _transformTarget.position;
    }
    
    private enum AxisAply
    {
        X,
        Y,
        Z
    }
}
