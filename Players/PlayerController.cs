using System;
using Cinemachine;
using CustomPackages.SilicomPlayer.CursorSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomPackages.SilicomPlayer.Players
{
    public class PlayerController : MonoBehaviour
    {
    
        public static PlayerController Current { get; private set; }

        [SerializeField] private CharacterController characterController;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private bool isDefaultPlayerInScene;

        public PlayerControllerSettings settings;

        [SerializeField] private UnityEvent onPlayerEnter;
        [SerializeField] private UnityEvent onPlayerExit;

        [SerializeField] private bool lockedMovement;
        [SerializeField] private bool lockedRotation;
        [SerializeField] private bool lockedInteractions;

        public bool LockedMovement { get; private set; }
        private bool _prevLockMovement;
        public bool LockedRotation { get; private set; }
        private bool _prevLockRotation;
        public bool LockedInteractions { get; private set; }
        private bool _prevLockInteractions;

        private Transform _transform;
        private Transform _cameraTransform;
        private Vector3 _movement;
        private Vector2 _rotation;
        private Vector3 _currentBodyRotation;
        private Vector3 _currentCameraRotation;

        private Bounds _bounds;

        private void Awake()
        {
            _transform = transform;
            _cameraTransform = virtualCamera.transform;

            _currentBodyRotation = _transform.localEulerAngles;
            _currentCameraRotation = _cameraTransform.localEulerAngles;

            if (settings.limitMovementX || settings.limitMovementY || settings.limitMovementZ)
            {
                Vector3 pos = _transform.position;
                _bounds = new Bounds
                {
                    x = {x = pos.x + settings.xAxisBounds.x, y = pos.x + settings.xAxisBounds.y},
                    y = {x = pos.y + settings.yAxisBounds.x, y = pos.y + settings.yAxisBounds.y},
                    z = {x = pos.z + settings.zAxisBounds.x, y = pos.z + settings.zAxisBounds.y}
                };
            }

            LockedMovement = lockedMovement;
            LockedRotation = lockedRotation;
            LockedInteractions = lockedInteractions;
            _prevLockMovement = LockedMovement;
            _prevLockRotation = LockedRotation;
            _prevLockInteractions = LockedInteractions;

            if (isDefaultPlayerInScene)
            {
#if UNITY_EDITOR
                if (Current != null)
                {
                    Debug.LogError("There is multiple PlayerController set as default. This is not allowed and will produce unexpected results.", this);
                }
#endif
                SwitchPlayerController();
            }
            else
            {
                virtualCamera.Priority = 10;
                characterController.enabled = false;
            }
        }


        public void Move(Vector2 movementVector)
        {
            if (LockedMovement) return;

            _movement = settings.movementSpeedMultiplier * movementVector.y * _transform.forward +
                        settings.movementSpeedMultiplier * movementVector.x * _transform.right;

            _movement += settings.gravity * Time.deltaTime;

            if (settings.limitMovementX || settings.limitMovementY || settings.limitMovementZ)
            {

                Vector3 pos = _transform.position;
                _movement = pos + _movement;
            
                if (settings.limitMovementX)
                {
                    _movement.x = Mathf.Clamp(_movement.x, _bounds.x.x, _bounds.x.y);
                }
            
                if (settings.limitMovementY)
                {
                    _movement.y = Mathf.Clamp(_movement.y, _bounds.y.x, _bounds.y.y);
                }
            
                if (settings.limitMovementZ)
                {
                    _movement.z = Mathf.Clamp(_movement.z, _bounds.z.x, _bounds.z.y);
                }

                _movement -= pos;
            }

            characterController.Move(_movement);
        }

        public void Rotate(Vector2 rotationVector)
        {
            if (LockedRotation) return;

            _rotation = settings.rotationSpeedMultiplier * rotationVector;
        
            _currentBodyRotation.y += _rotation.x;

            if (settings.clampRotationY)
            {
                _currentBodyRotation.y = Mathf.Clamp(_currentBodyRotation.y, settings.minRotationY,
                    settings.maxRotationY);
            }
        
            _transform.localEulerAngles = _currentBodyRotation;
        
            _currentCameraRotation.x -= _rotation.y;

            if (settings.clampRotationX)
            {
                _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, settings.minRotationX,
                    settings.maxRotationX);
            }
        
            _cameraTransform.localEulerAngles = _currentCameraRotation;

        }

        public void Teleport(Vector3 destination)
        {
            characterController.enabled = false;
            _transform.position = destination;
            characterController.enabled = true;
        }

        public void SwitchPlayerController()
        {
            if (Current != null)
            {
                Current.ExitPlayer();
            }

            Current = this;
        
            Current.EnterPlayer();
        }

        public void LockMovement()
        {
            if (LockedMovement) return;
            
            _prevLockMovement = LockedMovement;
            LockedMovement = true;
        }

        public void UnlockMovement()
        {
            if (!LockedMovement) return;
            LockedMovement = _prevLockMovement;
        }
        
        public void LockRotation()
        {
            if (LockedRotation) return;
            
            _prevLockRotation = LockedRotation;
            LockedRotation = true;
        }

        public void UnlockRotation()
        {
            if (!LockedRotation) return;
            
            LockedRotation = _prevLockRotation;
        }
        
        public void LockInteractions()
        {
            if (LockedInteractions) return;
            
            _prevLockInteractions = LockedInteractions;
            LockedInteractions = true;
        }

        public void UnlockInteractions()
        {
            if (!LockedInteractions) return;
            
            LockedInteractions = _prevLockInteractions;
        }

        private void EnterPlayer()
        {
            characterController.enabled = true;
            onPlayerEnter.Invoke();
            virtualCamera.Priority = 11;
            CursorManager.Instance.SetLockState(settings.lockedCursor);
        }

        private void ExitPlayer()
        {
            characterController.enabled = false;
            onPlayerExit.Invoke();
            virtualCamera.Priority = 10;
        }

    }


    [Serializable]
    public class PlayerControllerSettings
    {
        public bool clampRotationX;
        [ShowIf("clampRotationX")] public float minRotationX;
        [ShowIf("clampRotationX")] public float maxRotationX;

        public bool clampRotationY;
        [ShowIf("clampRotationY")] public float minRotationY;
        [ShowIf("clampRotationY")] public float maxRotationY;

        public Vector3 gravity;

        public bool limitMovementX;
        [ShowIf("limitMovementX")] public Vector2 xAxisBounds;

        public bool limitMovementY;
        [ShowIf("limitMovementY")] public Vector2 yAxisBounds;

        public bool limitMovementZ;
        [ShowIf("limitMovementZ")] public Vector2 zAxisBounds;

        public float movementSpeedMultiplier = 1;
        public float rotationSpeedMultiplier = 1;

        public bool lockedCursor;

        public float raycastLength = 2;
    }

    public class Bounds
    {
        public Vector2 x;
        public Vector2 y;
        public Vector2 z;
    }
}