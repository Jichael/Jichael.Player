using CustomPackages.SilicomPlayer.CursorSystem;
using CustomPackages.SilicomPlayer.Interactions;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Players.MouseKeyboard
{
    public class MouseController : MonoBehaviour
    {

        [SerializeField] private new Camera camera;

        [SerializeField] private float rayLength;

        [SerializeField] private LayerMask layerMask;

        private Ray _ray;
        private RaycastHit _hit;

        private Collider _hitCollider;
        private IMouseInteract[] _currentInteractions;

        private bool _isHovering;

        public void RayCast(Vector2 mousePosition)
        {
            _ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(_ray, out _hit, rayLength, layerMask))
            {
                Collider hitCollider = _hit.collider;

                if (hitCollider == _hitCollider)
                {
                    if(_isHovering) _currentInteractions.HoverStay();
                }
                else
                {
                    if (_isHovering)
                    {
                        _currentInteractions.HoverExit();
                        _currentInteractions = null;
                    }

                    _hitCollider = hitCollider;

                    if (_hitCollider)
                    {
                        _isHovering = true;
                        _currentInteractions = hitCollider.GetComponents<IMouseInteract>();
                        _currentInteractions.HoverEnter();
                        _currentInteractions.SetHoverCursor();
                    }
                    else
                    {
                        _isHovering = false;
                        CursorManager.Instance.ResetDefaultCursor();
                    }
                }
            }
            else
            {
                if (_isHovering)
                {
                    _hitCollider = null;
                    _currentInteractions.HoverExit();
                    _currentInteractions = null;
                    CursorManager.Instance.ResetDefaultCursor();
                }
                _isHovering = false;
            }
        }

        public void LeftClick()
        {
            if(!_isHovering) return;
            _currentInteractions.LeftClick();
        }

        public void RightClick()
        {
            if(!_isHovering) return;
            _currentInteractions.RightClick();
        }
    
        public enum MouseButton
        {
            LeftClick,
            RightClick
        }
    
    }
}