using System;
using CustomPackages.Silicom.Player.CursorSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomPackages.Silicom.Player.Players.MouseKeyboard
{
    public class InputsHandlerMouseKeyboard : InputsHandler
    {
        
        public static InputsHandlerMouseKeyboard Current { get; private set; }

        public override InputActionReference UseAction => leftClickAction;

        public static event Action<InputAction.CallbackContext> OnEscapeKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnVKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnShiftYKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnRKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnTKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnPKey = delegate {};
        public static event Action<InputAction.CallbackContext> OnMKey = delegate {};
        public static event Action<InputAction.CallbackContext> On1Key = delegate {};
        public static event Action<InputAction.CallbackContext> On2Key = delegate {};
        public static event Action<InputAction.CallbackContext> On3Key = delegate {};
        public static event Action<InputAction.CallbackContext> On4Key = delegate {};

        [SerializeField] private InputActionReference mousePositionAction;
        [SerializeField] private InputActionReference leftClickAction;
        [SerializeField] private InputActionReference rightClickAction;
    
        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference thirdAxisMovementAction;
        [SerializeField] private InputActionReference rotationAction;

        [SerializeField] private InputActionReference escapeAction;
        [SerializeField] private InputActionReference vAction;
        [SerializeField] private InputActionReference shiftYAction;
        [SerializeField] private InputActionReference rAction;
        [SerializeField] private InputActionReference tAction;
        [SerializeField] private InputActionReference pAction;
        [SerializeField] private InputActionReference mAction;
        [SerializeField] private InputActionReference oneAction;
        [SerializeField] private InputActionReference twoAction;
        [SerializeField] private InputActionReference threeAction;
        [SerializeField] private InputActionReference fourAction;

        [SerializeField] private float mouseSensitivity = 1;

        [SerializeField] private MouseController mouseController;

        private Vector2 _mousePosition;
        private bool _mousePositionNeedUpdate;
        private Vector2 _movementInput;
        private float _thirdAxisMovement;
        private Vector3 _movement;
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
            mousePositionAction.action.Enable();
        }
        private void OnDisable()
        {
            Current = null;
            StopInputProcessing();
            mousePositionAction.action.Disable();
            if(_mousePositionNeedUpdate) mousePositionAction.action.performed -= OnMousePositionUpdate;
        }

        private void Start()
        {
            CursorManager.Instance.OnLockStateChanged += OnLockStateChanged;
        }

        private void OnDestroy()
        {
            CursorManager.Instance.OnLockStateChanged -= OnLockStateChanged;
        }

        private void OnLockStateChanged(bool lockState)
        {
            if (lockState || _mousePositionNeedUpdate) return;
            _mousePositionNeedUpdate = true;
            mousePositionAction.action.performed += OnMousePositionUpdate;
        }

        public override void StartInputProcessing()
        {
            if(_listening) return;
            
            movementAction.action.Enable();
            sprintAction.action.Enable();
            sprintAction.action.performed += OnSprint;
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
            shiftYAction.action.Enable();
            shiftYAction.action.performed += OnShiftY;
            rAction.action.Enable();
            rAction.action.performed += OnR;
            tAction.action.Enable();
            tAction.action.performed += OnT;
            pAction.action.Enable();
            pAction.action.performed += OnP;
            mAction.action.Enable();
            mAction.action.performed += OnM;

            oneAction.action.Enable();
            oneAction.action.performed += On1;
            twoAction.action.Enable();
            twoAction.action.performed += On2;
            threeAction.action.Enable();
            threeAction.action.performed += On3;
            fourAction.action.Enable();
            fourAction.action.performed += On4;

            _listening = true;
        }
        
        public override void StopInputProcessing()
        {
            if(!_listening) return;
            
            movementAction.action.Disable();
            sprintAction.action.Disable();
            sprintAction.action.performed -= OnSprint;
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
            shiftYAction.action.Disable();
            shiftYAction.action.performed -= OnShiftY;
            rAction.action.Disable();
            rAction.action.performed -= OnR;
            tAction.action.Disable();
            tAction.action.performed -= OnT;
            pAction.action.Disable();
            pAction.action.performed -= OnP;
            mAction.action.Disable();
            mAction.action.performed -= OnM;
            
            oneAction.action.Disable();
            oneAction.action.performed -= On1;
            twoAction.action.Disable();
            twoAction.action.performed -= On2;
            threeAction.action.Disable();
            threeAction.action.performed -= On3;
            fourAction.action.Disable();
            fourAction.action.performed -= On4;

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
                if (!_mousePositionNeedUpdate) _mousePosition = mousePositionAction.action.ReadValue<Vector2>();
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
                _thirdAxisMovement = -thirdAxisMovementAction.action.ReadValue<float>();
            
                _rotationInput = rotationAction.action.ReadValue<Vector2>() * mouseSensitivity;

                _movement = _movementInput;
                _movement.z = _thirdAxisMovement;
            
                PlayerController.Current.Move(_movement * Time.deltaTime, _sprint);
                PlayerController.Current.Rotate(_rotationInput);
                
                if (!PlayerController.Current.Locked3DInteractions)
                {
                    // TODO : wait for a fix in new input system :
                    // when we lock the cursor, the mousePosition is only updated after the first next event 
                    // -> not moving the mouse makes that the raycast is made from the previous unlock mouse position
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
            mousePositionAction.action.performed -= OnMousePositionUpdate;
        }


        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            bool clicked = Mathf.Approximately(ctx.ReadValue<float>(), 1);
        
            CursorManager.Instance.SetClickState(clicked);
        
            if (clicked && PlayerController.Current && !PlayerController.Current.Locked3DInteractions && !PlayerController.Switching)
            {
                mouseController.LeftClick();
            }
        }

        private void OnRightClick(InputAction.CallbackContext ctx)
        {
            /*
                bool clicked = Mathf.Approximately(ctx.ReadValue<float>(), 1);
    
                if (clicked && PlayerController.Current && !PlayerController.Current.Locked3DInteractions && !PlayerController.Switching)
                {
                    mouseController.RightClick();
                }
            */
            }
        private void OnSprint(InputAction.CallbackContext ctx)
        {
            _sprint = Mathf.Approximately(ctx.ReadValue<float>(), 1);
        }

        private void OnEscape(InputAction.CallbackContext ctx)
        {
            OnEscapeKey?.Invoke(ctx);
        }
        
        private void OnV(InputAction.CallbackContext ctx)
        {
            OnVKey?.Invoke(ctx);
        }

        private void OnShiftY(InputAction.CallbackContext ctx)
        {
            OnShiftYKey?.Invoke(ctx);
        }
        
        private void OnR(InputAction.CallbackContext ctx)
        {
            OnRKey?.Invoke(ctx);
        }
        
        private void OnT(InputAction.CallbackContext ctx)
        {
            OnTKey?.Invoke(ctx);
        }
        
        private void OnP(InputAction.CallbackContext ctx)
        {
            OnPKey?.Invoke(ctx);
        }
        
        private void OnM(InputAction.CallbackContext ctx)
        {
            OnMKey?.Invoke(ctx);
        }
        
        private void On1(InputAction.CallbackContext ctx)
        {
            On1Key?.Invoke(ctx);
        }
        
        private void On2(InputAction.CallbackContext ctx)
        {
            On2Key?.Invoke(ctx);
        }
        
        private void On3(InputAction.CallbackContext ctx)
        {
            On3Key?.Invoke(ctx);
        }
        
        private void On4(InputAction.CallbackContext ctx)
        {
            On4Key?.Invoke(ctx);
        }

    }
}