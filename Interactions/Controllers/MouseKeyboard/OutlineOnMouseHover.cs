using CustomPackages.SilicomPlayer.CursorSystem;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Interactions.Controllers.MouseKeyboard
{
    public class OutlineOnMouseHover : MonoBehaviour, IMouseInteract
    {
        [SerializeField] private Outline outline;

        [SerializeField] private CursorInfo cursorInfo;

        public bool DisableInteraction { get; set; }
        public void LeftClick() { }

        public void RightClick() { }

        public void HoverEnter()
        {
            outline.enabled = true;
        }

        public void HoverStay() { }

        public void HoverExit()
        {
            outline.enabled = false;
        }

        public CursorInfo HoverCursor => cursorInfo;
    }
}
