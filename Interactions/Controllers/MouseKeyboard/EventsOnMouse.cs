using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CustomPackages.Silicom.Player.Interactions.Controllers.MouseKeyboard
{
    public class EventsOnMouse : MonoBehaviour, IMouseInteract
    {
        [FormerlySerializedAs("leftClickEvents")] [SerializeField] private UnityEvent[] useEvents;
        [SerializeField] private UnityEvent[] hoverEnterEvents;
        [SerializeField] private UnityEvent[] hoverStayEvents;
        [SerializeField] private UnityEvent[] hoverExitEvents;

        [SerializeField] private bool disableInteractions;

        public bool DisableInteraction
        {
            get => disableInteractions;
            set => disableInteractions = value;
        }

        public void Use(MouseController mouseController)
        {
            for(int i = 0; i < useEvents.Length; i++) useEvents[i]?.Invoke();
        }

        public void HoverEnter(MouseController mouseController)
        {
            for(int i = 0; i < hoverEnterEvents.Length; i++) hoverEnterEvents[i]?.Invoke();
        }

        public void HoverStay(MouseController mouseController)
        {
            for(int i = 0; i < hoverStayEvents.Length; i++) hoverStayEvents[i]?.Invoke();
        }

        public void HoverExit(MouseController mouseController)
        {
            for(int i = 0; i < hoverExitEvents.Length; i++) hoverExitEvents[i]?.Invoke();
        }

    }
}
