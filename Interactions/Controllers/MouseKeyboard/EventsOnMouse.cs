using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CustomPackages.Silicom.Player.Interactions.Controllers.MouseKeyboard
{
    [RequireComponent(typeof(ChangeCursorOnMouseHover))]
    public class EventsOnMouse : MonoBehaviour, IMouseInteract
    {
        [FormerlySerializedAs("mouseButton")] [SerializeField] private MouseController.MouseEvent mouseEvent;

        [SerializeField] private UnityEvent[] events;

        [SerializeField] private bool disableInteractions;
        public bool DisableInteraction => disableInteractions;

        public void LeftClick(MouseController mouseController)
        {
            if (mouseEvent != MouseController.MouseEvent.LeftClick) return;
            InvokeEvents();
        }

        public void RightClick(MouseController mouseController)
        {
            if (mouseEvent != MouseController.MouseEvent.RightClick) return;
            InvokeEvents();
        }

        public void HoverEnter(MouseController mouseController)
        { 
            if (mouseEvent != MouseController.MouseEvent.HoverEnter) return;
            InvokeEvents();
        }

        public void HoverStay(MouseController mouseController)
        {
            if (mouseEvent != MouseController.MouseEvent.HoverStay) return;
            InvokeEvents();
        }

        public void HoverExit(MouseController mouseController)
        {
            if (mouseEvent != MouseController.MouseEvent.HoverExit) return;
            InvokeEvents();
        }

        private void InvokeEvents()
        {
            for(int i = 0; i < events.Length; i++) events[i]?.Invoke();
        }
    }
}
