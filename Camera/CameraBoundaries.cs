using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    public BoxCollider restrictor;
    public GameObject positionChanger;
    public Vector3 desiredRotation;
    public bool applyDesiredRotation = false; // A flag to control when to apply the desired rotation

    private void Start()
    {
        if (restrictor == null)
        {
            restrictor = GetComponent<BoxCollider>();
        }

        if (positionChanger == null)
        {
            positionChanger = FindObjectOfType<CameraController>().gameObject;
        }
    }

    private void LateUpdate()
    {
        if (restrictor != null && positionChanger != null)
        {
            Bounds bounds = restrictor.bounds;
            Vector3 restrictedPosition = positionChanger.transform.position;

            restrictedPosition.x = Mathf.Clamp(restrictedPosition.x, bounds.min.x, bounds.max.x);
            restrictedPosition.y = Mathf.Clamp(restrictedPosition.y, bounds.min.y, bounds.max.y);
            restrictedPosition.z = Mathf.Clamp(restrictedPosition.z, bounds.min.z, bounds.max.z);

            positionChanger.transform.position = restrictedPosition;

            if (applyDesiredRotation)
            {
                positionChanger.transform.rotation = Quaternion.Euler(desiredRotation);
            }
        }
    }

    // Call this method to toggle the desired rotation
    public void ToggleDesiredRotation(bool applyRotation)
    {
        applyDesiredRotation = applyRotation;
        if (applyRotation)
        {
            positionChanger.transform.rotation = Quaternion.Euler(desiredRotation);
        }
    }
}