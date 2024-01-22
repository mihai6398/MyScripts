using UnityEngine;

public class ReturnLastCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ReturnLastCheckPoint " + other.name);
        RespawnCar respawn;
        if (other.TryGetComponent(out respawn))
        {
            respawn.Respawn();
        }
    }
}
