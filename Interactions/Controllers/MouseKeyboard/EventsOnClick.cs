using CustomPackages.SilicomPlayer.Interactions;
using CustomPackages.SilicomPlayer.Players.MouseKeyboard;
using UnityEngine;
using UnityEngine.Events;

public class EventsOnClick : MonoBehaviour, IMouseInteract
{
    [SerializeField] private MouseController.MouseButton mouseButton;

    [SerializeField] private UnityEvent[] events;

    public bool DisableInteraction { get; set; }

    public void LeftClick(MouseController mouseController)
    {
        if (mouseButton != MouseController.MouseButton.LeftClick) return;
        for(int i = 0; i < events.Length; i++) events[i]?.Invoke();
    }

    public void RightClick(MouseController mouseController)
    {
        if (mouseButton != MouseController.MouseButton.RightClick) return;
        for(int i = 0; i < events.Length; i++) events[i]?.Invoke();
    }

    public void HoverEnter(MouseController mouseController) { }

    public void HoverStay(MouseController mouseController) { }

    public void HoverExit(MouseController mouseController) { }
}
