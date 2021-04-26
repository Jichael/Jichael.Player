using CustomPackages.Silicom.Player.Players;
using UnityEngine;

namespace CustomPackages.Silicom.Player.Interactions.Views
{
    public class SwitchPlayerController : MonoBehaviour
    {

        [SerializeField] private PlayerController playerController;

        [SerializeField] private bool switchBack;

        private PlayerController _playerController;
        private bool _hasSwitched;
        
        public void Switch()
        {
            if (PlayerController.Switching) return;
            if (switchBack)
            {
                if (_hasSwitched)
                {
                    _playerController.SwitchPlayerController();
                }
                else
                {
                    _playerController = PlayerController.Current;
                    playerController.SwitchPlayerController();
                }
                _hasSwitched = !_hasSwitched;
            }
            else
            {
                
                playerController.SwitchPlayerController();
            }
        }
    }
}
