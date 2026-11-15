using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ScrollbarRelease : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private UnityEvent onReleased;

    public void OnPointerUp(PointerEventData eventData)
    {
        onReleased?.Invoke();
    }
}