using CustomPackages.SilicomPlayer.CursorSystem;
using CustomPackages.SilicomPlayer.Players;
using CustomPackages.SilicomPlayer.Players.MouseKeyboard;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Interactions.Controllers.MouseKeyboard
{
    public class SwitchPlayerControllerMouse : MonoBehaviour, IMouseInteract
    {

        [SerializeField] private PlayerController playerController;

        [SerializeField] private MouseController.MouseButton switchButton;

        [SerializeField] private CursorInfo hoverCursor;

        public bool DisableInteraction { get; set; }
        public void LeftClick()
        {
            if (switchButton == MouseController.MouseButton.LeftClick)
            {
                PlayerController.SwitchPlayerController(playerController);
            }
        }

        public void RightClick()
        {
            if (switchButton == MouseController.MouseButton.RightClick)
            {
                PlayerController.SwitchPlayerController(playerController);
            }
        }

        public void HoverEnter() { }

        public void HoverStay() { }

        public void HoverExit() { }

        public CursorInfo HoverCursor => hoverCursor;
    }
}
