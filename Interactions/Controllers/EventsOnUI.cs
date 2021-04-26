using CustomPackages.Silicom.Player.Players;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventsOnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent[] pointerEnterEvents;
    public UnityEvent[] pointerExitEvents;
    public UnityEvent[] pointerClickEvents;
    public UnityEvent[] pointerDownEvents;
    public UnityEvent[] pointerUpEvents;

    public bool disableInteractions;

    private bool _isHovering;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disableInteractions) return;
        if(PlayerController.Current && PlayerController.Current.LockedUIInteractions) return;
        _isHovering = true;
        for(int i = 0; i < pointerEnterEvents.Length; i++) pointerEnterEvents[i].Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (disableInteractions) return;
        if(PlayerController.Current && PlayerController.Current.LockedUIInteractions) return;
        _isHovering = false;
        for(int i = 0; i < pointerExitEvents.Length; i++) pointerExitEvents[i].Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (disableInteractions) return;
        if(PlayerController.Current && PlayerController.Current.LockedUIInteractions) return;
        for(int i = 0; i < pointerClickEvents.Length; i++) pointerClickEvents[i].Invoke();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (disableInteractions) return;
        if(PlayerController.Current && PlayerController.Current.LockedUIInteractions) return;
        for(int i = 0; i < pointerDownEvents.Length; i++) pointerDownEvents[i].Invoke();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (disableInteractions) return;
        if(PlayerController.Current && PlayerController.Current.LockedUIInteractions) return;
        for(int i = 0; i < pointerUpEvents.Length; i++) pointerUpEvents[i].Invoke();
    }

    private void OnDisable()
    {
        if (_isHovering)
        {
            _isHovering = false;
            for(int i = 0; i < pointerExitEvents.Length; i++) pointerExitEvents[i].Invoke();
        }
    }

}
