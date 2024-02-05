using System;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class ZoneChecker : MonoBehaviour
{
    private Collider targetZone; // Set this from the Unity Inspector

    private void Start() => targetZone = gameObject.GetComponent<BoxCollider>();


    private void OnTriggerEnter(Collider other)
    {
        if (other == targetZone)
        {
            Unit unit = other.gameObject.GetComponent<Unit>();
            if (unit != null)
            {

                if (TutorialSystem.Instance != null)
                {
                    if (TutorialSystem.Instance.goToboombArea)
                    {
                        TutorialSystem.Instance.CompleteGoToBoombArea();
                    }
                }
            }
        }
    }
}