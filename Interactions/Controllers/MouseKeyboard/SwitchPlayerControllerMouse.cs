using CustomPackages.SilicomPlayer.Players;
using CustomPackages.SilicomPlayer.Players.MouseKeyboard;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Interactions.Controllers.MouseKeyboard
{
    public class SwitchPlayerControllerMouse : MonoBehaviour, IMouseInteract
    {

        [SerializeField] private PlayerController playerController;

        [SerializeField] private MouseController.MouseButton switchButton;

        public bool DisableInteraction { get; set; }
        public void LeftClick(MouseController mouseController)
        {
            if (switchButton == MouseController.MouseButton.LeftClick) playerController.SwitchPlayerController();
        }

        public void RightClick(MouseController mouseController)
        {
            if (switchButton == MouseController.MouseButton.RightClick) playerController.SwitchPlayerController();
        }

        public void HoverEnter(MouseController mouseController) { }

        public void HoverStay(MouseController mouseController) { }

        public void HoverExit(MouseController mouseController) { }
    }
}
