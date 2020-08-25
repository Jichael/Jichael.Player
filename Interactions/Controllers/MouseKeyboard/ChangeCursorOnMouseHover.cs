using CustomPackages.Silicom.Player.CursorSystem;
using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using UnityEngine;

namespace CustomPackages.Silicom.Player.Interactions.Controllers.MouseKeyboard
{
    public class ChangeCursorOnMouseHover : MonoBehaviour, IMouseInteract
    {
        [SerializeField] private CursorInfo cursor;

        public bool DisableInteraction { get; set; }
        public void LeftClick(MouseController mouseController) { }

        public void RightClick(MouseController mouseController) { }
    
        public void HoverEnter(MouseController mouseController)
        {
            CursorManager.Instance.SetCursor(cursor);
        }

        public void HoverStay(MouseController mouseController) { }

        public void HoverExit(MouseController mouseController)
        {
            CursorManager.Instance.ResetDefaultCursor();
        }
    }
}
