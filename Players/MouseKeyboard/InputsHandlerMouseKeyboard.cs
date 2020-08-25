using System;
using CustomPackages.Silicom.Player.CursorSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomPackages.Silicom.Player.Players.MouseKeyboard
{
    public class InputsHandlerMouseKeyboard : InputsHandler
    {

        public static event Action<InputAction.CallbackContext> OnEscapeKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnVKey = delegate {};

        [SerializeField] private InputActionReference mousePositionAction;
        [SerializeField] private InputActionReference leftClickAction;
        [SerializeField] private InputActionReference rightClickAction;
    
        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference thirdAxisMovementAction;
        [SerializeField] private InputActionReference rotationAction;

        [SerializeField] private InputActionReference escapeAction;
        [SerializeField] private InputActionReference vAction;

        [SerializeField] private float mouseSensitivity = 1;

        [SerializeField] private MouseController mouseController;

        private Vector2 _mousePosition;
        private Vector2 _movementInput;
        private float _thirdAxisMovement;
        private Vector3 _movement;
        private Vector2 _rotationInput;

        private void OnEnable()
        {
            Current = this;
            mousePositionAction.action.Enable();
            movementAction.action.Enable();
            thirdAxisMovementAction.action.Enable();
            rotationAction.action.Enable();
            leftClickAction.action.Enable();
            leftClickAction.action.performed += OnLeftClick;
            rightClickAction.action.Enable();
            rightClickAction.action.performed += OnRightClick;
            
            // Events
            escapeAction.action.Enable();
            escapeAction.action.performed += OnEscape;
            vAction.action.Enable();
            vAction.action.performed += OnV;
        }

        private void OnDisable()
        {
            Current = null;
            mousePositionAction.action.Disable();
            movementAction.action.Disable();
            thirdAxisMovementAction.action.Disable();
            rotationAction.action.Disable();
            leftClickAction.action.performed -= OnLeftClick;
            leftClickAction.action.Disable();
            rightClickAction.action.performed -= OnRightClick;
            rightClickAction.action.Disable();
            
            
            escapeAction.action.Disable();
            escapeAction.action.performed -= OnEscape;
            vAction.action.Disable();
            vAction.action.performed -= OnV;
        }

        private void Update()
        {
            _mousePosition = mousePositionAction.action.ReadValue<Vector2>();

            CursorManager.Instance.SetCursorPosition(_mousePosition);

            if (!PlayerController.Current) return;
            
            _movementInput = movementAction.action.ReadValue<Vector2>();
            _thirdAxisMovement = -thirdAxisMovementAction.action.ReadValue<float>();
            
            _rotationInput = rotationAction.action.ReadValue<Vector2>() * mouseSensitivity;

            _movement = _movementInput;
            _movement.z = _thirdAxisMovement;
            
            PlayerController.Current.Move(_movement * Time.deltaTime);
            PlayerController.Current.Rotate(_rotationInput);

            if (!PlayerController.Current.LockedInteractions)
            {
                // TODO : wait for a fix in new input system :
                // when we lock the cursor, the mousePosition is only updated after the first next event 
                // -> not moving the mouse makes that the raycast is made from the previous unlock mouse position
                mouseController.RayCast(_mousePosition, PlayerController.Current.settings.raycastLength);
            }

        }

        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            bool clicked = Mathf.Approximately(ctx.ReadValue<float>(), 1);
        
            CursorManager.Instance.SetClickState(clicked);
        
            if (clicked && PlayerController.Current && !PlayerController.Current.LockedInteractions)
            {
                mouseController.LeftClick();
            }
        }

        private void OnRightClick(InputAction.CallbackContext ctx)
        {
            bool clicked = Mathf.Approximately(ctx.ReadValue<float>(), 1);

            if (clicked && PlayerController.Current && !PlayerController.Current.LockedInteractions)
            {
                mouseController.RightClick();
            }
        }

        private void OnEscape(InputAction.CallbackContext ctx)
        {
            OnEscapeKey?.Invoke(ctx);
        }
        
        private void OnV(InputAction.CallbackContext ctx)
        {
            OnVKey?.Invoke(ctx);
        }
    
    }
}