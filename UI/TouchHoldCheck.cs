// Warning use EventSystem in scene 
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHoldCheck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pressing;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        pressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        pressing = false;
    }

    void OnDisable()
    {
        pressing = false;
    }
}