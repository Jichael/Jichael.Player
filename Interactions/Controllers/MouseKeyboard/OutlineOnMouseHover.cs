using CustomPackages.SilicomPlayer.Players.MouseKeyboard;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Interactions.Controllers.MouseKeyboard
{
    public class OutlineOnMouseHover : MonoBehaviour, IMouseInteract
    {
        [SerializeField] private Outline outline;
        public bool DisableInteraction { get; set; }
        public void LeftClick(MouseController mouseController) { }

        public void RightClick(MouseController mouseController) { }

        public void HoverEnter(MouseController mouseController)
        {
            outline.enabled = true;
        }

        public void HoverStay(MouseController mouseController) { }

        public void HoverExit(MouseController mouseController)
        {
            outline.enabled = false;
        }
    }
}
