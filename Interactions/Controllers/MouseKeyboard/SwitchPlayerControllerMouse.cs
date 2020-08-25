using CustomPackages.Silicom.Player.Players;
using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomPackages.Silicom.Player.Interactions.Controllers.MouseKeyboard
{
    
    [RequireComponent(typeof(ChangeCursorOnMouseHover))]
    public class SwitchPlayerControllerMouse : MonoBehaviour, IMouseInteract
    {

        [SerializeField] private PlayerController playerController;

        [FormerlySerializedAs("switchButton")] [SerializeField] private MouseController.MouseEvent switchEvent;

        public bool DisableInteraction { get; set; }
        public void LeftClick(MouseController mouseController)
        {
            if (switchEvent == MouseController.MouseEvent.LeftClick) playerController.SwitchPlayerController();
        }

        public void RightClick(MouseController mouseController)
        {
            if (switchEvent == MouseController.MouseEvent.RightClick) playerController.SwitchPlayerController();
        }

        public void HoverEnter(MouseController mouseController) { }

        public void HoverStay(MouseController mouseController) { }

        public void HoverExit(MouseController mouseController) { }
    }
}
