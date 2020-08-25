using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using UnityEngine;

namespace CustomPackages.Silicom.Player.Interactions.Controllers.MouseKeyboard
{
    
    [RequireComponent(typeof(ChangeCursorOnMouseHover))]
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
