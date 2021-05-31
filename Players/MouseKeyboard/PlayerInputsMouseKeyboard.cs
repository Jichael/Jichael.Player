using CustomPackages.Silicom.Player.CursorSystem;
using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomPackages.Silicom.Player.Players
{
    public class PlayerInputsMouseKeyboard : InputsHandler
    {
        
        public static PlayerInputsMouseKeyboard Current { get; private set; }

        public override InputActionReference UseAction => useAction;

        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference movement3rdAxisAction;
        [SerializeField] private InputActionReference rotationAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference useAction;
        [SerializeField] private InputActionReference interactionPosition;
        [SerializeField] private InputActionReference primaryAction;
        [SerializeField] private InputActionReference secondaryAction;

        [SerializeField] private float mouseSensitivity = 1;

        [SerializeField] private MouseController mouseController;

        private Vector2 _mousePosition;
        private bool _mousePositionNeedUpdate;
        private Vector2 _movementInput;
        private Vector3 _movement;
        private float _movement3rdAxis;
        private bool _sprint;
        private Vector2 _rotationInput;

        private bool _listening;

        public Vector2 MousePosition => _mousePosition;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            Current = this;
            StartInputProcessing();
            interactionPosition.action.Enable();
        }
        private void OnDisable()
        {
            Current = null;
            StopInputProcessing();
            interactionPosition.action.Disable();
            if(_mousePositionNeedUpdate) interactionPosition.action.performed -= OnMousePositionUpdate;
        }

        private void Start()
        {
            CursorManager.Instance.OnLockStateChanged += OnLockStateChanged;
        }

        private void OnDestroy()
        {
            if(CursorManager.Instance) CursorManager.Instance.OnLockStateChanged -= OnLockStateChanged;
        }

        private void OnLockStateChanged(bool lockState)
        {
            if (lockState || _mousePositionNeedUpdate) return;
            _mousePositionNeedUpdate = true;
            interactionPosition.action.performed += OnMousePositionUpdate;
        }


        public override void StartInputProcessing()
        {
            if(_listening) return;
            _listening = true;
            
            movementAction.action.Enable();
            movement3rdAxisAction.action.Enable();
            rotationAction.action.Enable();
            
            sprintAction.action.Enable();
            sprintAction.action.performed += OnSprint;
            
            useAction.action.Enable();
            useAction.action.performed += OnUse;

            primaryAction.action.Enable();
            secondaryAction.action.Enable();
            
        }
        
        public override void StopInputProcessing()
        {
            if(!_listening) return;
            
            movementAction.action.Disable();
            movement3rdAxisAction.action.Disable();
            rotationAction.action.Disable();
            
            sprintAction.action.Disable();
            sprintAction.action.performed -= OnSprint;
            
            useAction.action.Disable();
            useAction.action.performed -= OnUse;
            
            primaryAction.action.Disable();
            secondaryAction.action.Disable();

            _listening = false;
        }

        private void Update()
        {
            if (CursorManager.Instance.IsLocked)
            {
                _mousePosition = CursorManager.Instance.CenterScreen;
            }
            else
            {
                if (!_mousePositionNeedUpdate) _mousePosition = interactionPosition.action.ReadValue<Vector2>();
                CursorManager.Instance.SetCursorPosition(_mousePosition);
            }

            if (!PlayerController.Current) return;

            if (PlayerController.Switching)
            {
                mouseController.Reset();
            }
            else
            {
                _movementInput = movementAction.action.ReadValue<Vector2>();
                _movement3rdAxis = movement3rdAxisAction.action.ReadValue<float>();
            
                _rotationInput = rotationAction.action.ReadValue<Vector2>() * mouseSensitivity;

                _movement = _movementInput;
                _movement.z = _movement3rdAxis;
            
                PlayerController.Current.Move(_movement * Time.deltaTime, _sprint);
                PlayerController.Current.Rotate(_rotationInput);
                
                if (!PlayerController.Current.Locked3DInteractions)
                {
                    mouseController.RayCast(_mousePosition, PlayerController.Current.settings.raycastLength);
                }
                else
                {
                    mouseController.Reset();
                }
            }
        }
        
        private void OnMousePositionUpdate(InputAction.CallbackContext ctx)
        {
            _mousePositionNeedUpdate = false;
            interactionPosition.action.performed -= OnMousePositionUpdate;
        }

        private void OnUse(InputAction.CallbackContext ctx)
        {
            bool clicked = Mathf.Approximately(ctx.ReadValue<float>(), 1);
        
            CursorManager.Instance.SetClickState(clicked);
        
            if (clicked && PlayerController.Current && !PlayerController.Current.Locked3DInteractions && !PlayerController.Switching)
            {
                mouseController.LeftClick();
            }
        }

        private void OnSprint(InputAction.CallbackContext ctx)
        {
            _sprint = Mathf.Approximately(ctx.ReadValue<float>(), 1);
        }

    }
}