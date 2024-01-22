using UnityEngine;

public class ApplyWheelRotation : MonoBehaviour
{
    [SerializeField] private RCC_CarControllerV3 _carController;

    private void LateUpdate()
    {
        if (_carController.steeringDirection != null)
            transform.localRotation = _carController.steeringDirection.localRotation;
    }
}
