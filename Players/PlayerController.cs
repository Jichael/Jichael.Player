using System;
using System.Collections;
using Cinemachine;
using CustomPackages.Silicom.Player.CursorSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomPackages.Silicom.Player.Players
{
    public class PlayerController : MonoBehaviour
    {
    
        public static PlayerController Current { get; private set; }

        [SerializeField] private CharacterController characterController;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private bool isDefaultPlayerInScene;

        public PlayerControllerMode normalMode;
        public PlayerControllerMode uiMode;
        
        public PlayerControllerSettings settings;

        [Serializable]
        private class DelayedEvents
        {
            public UnityEvent events;
            public float delay;
            public IEnumerator Invoke()
            {
                yield return new WaitForSeconds(delay);
                events.Invoke();
            }
        }
        [SerializeField] private DelayedEvents[] onPlayerEnterEvents;
        [SerializeField] private DelayedEvents[] onPlayerExitEvents;

        public bool LockedMovement { get; private set; }
        public bool LockedRotation { get; private set; }
        public bool Locked3DInteractions { get; private set; }
        public bool LockedUIInteractions { get; private set; }
        
        public bool LockMovementOverride { get; set; }
        public bool LockRotationOverride { get; set; }
        public bool Lock3DInteractionsOverride { get; set; }
        public bool LockUIInteractionsOverride { get; set; }

        private static bool _isNormalMode;

        private Transform _transform;
        public Transform CameraTransform { get; private set; }
        private Vector3 _movement;
        private Vector2 _rotation;
        private Vector3 _currentBodyRotation;
        private Vector3 _currentCameraRotation;

        private Bounds _bounds;

        private Vector3 _defaultRotation;
        private Vector3 _defaultPosition;

        public static bool Switching => _switchDelay >= Time.time;
        private static float _switchDelay = -1;
        private const float SWITCH_DELAY = 1;

        private void Awake()
        {
            _transform = transform;
            CameraTransform = virtualCamera.transform;

            _currentBodyRotation = _transform.localEulerAngles;
            _currentCameraRotation = CameraTransform.localEulerAngles;

            _defaultPosition = _transform.position;
            _defaultRotation = _transform.eulerAngles;

            if (settings.limitMovementX || settings.limitMovementY || settings.limitMovementZ)
            {
                Vector3 pos = _transform.position;
                _bounds = new Bounds
                {
                    X = {x = pos.x + settings.xAxisBounds.x, y = pos.x + settings.xAxisBounds.y},
                    Y = {x = pos.y + settings.yAxisBounds.x, y = pos.y + settings.yAxisBounds.y},
                    Z = {x = pos.z + settings.zAxisBounds.x, y = pos.z + settings.zAxisBounds.y}
                };
            }

            if (!isDefaultPlayerInScene)
            {
                virtualCamera.Priority = 10;
                characterController.enabled = false;
            }
        }

        private void Start()
        {
            if (isDefaultPlayerInScene)
            {
#if UNITY_EDITOR
                if (Current != null)
                {
                    Debug.LogError("There is multiple PlayerController set as default. This is not allowed and will produce unexpected results.", this);
                    return;
                }
#endif
                SwitchPlayerController();
            }
        }


        public void Move(Vector3 movementVector, bool sprinting)
        {
            if (LockedMovement || Switching) return;

            _movement = settings.movementSpeedMultiplier * movementVector.y * _transform.forward +
                        settings.movementSpeedMultiplier * movementVector.x * _transform.right;
            if (sprinting) _movement *= settings.sprintMultiplier;

            if (settings.allowThirdAxisMovement)
            {
                _movement += settings.thirdAxisSpeedMultiplier * movementVector.z * _transform.up;
            }

            _movement += settings.gravity * Time.deltaTime;

            if (settings.limitMovementX || settings.limitMovementY || settings.limitMovementZ)
            {

                Vector3 pos = _transform.position;
                _movement = pos + _movement;
            
                if (settings.limitMovementX)
                {
                    _movement.x = Mathf.Clamp(_movement.x, _bounds.X.x, _bounds.X.y);
                }
            
                if (settings.limitMovementY)
                {
                    _movement.y = Mathf.Clamp(_movement.y, _bounds.Y.x, _bounds.Y.y);
                }
            
                if (settings.limitMovementZ)
                {
                    _movement.z = Mathf.Clamp(_movement.z, _bounds.Z.x, _bounds.Z.y);
                }

                _movement -= pos;
            }

            characterController.Move(_movement);
        }

        public void Rotate(Vector2 rotationVector)
        {
            if (LockedRotation || Switching) return;

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
        
            CameraTransform.localEulerAngles = _currentCameraRotation;

        }

        public void Teleport(Vector3 destination)
        {
            characterController.enabled = false;
            _transform.position = destination;
            characterController.enabled = true;
        }
        
        public void Teleport(Vector3 destination, Quaternion rotation)
        {
            characterController.enabled = false;
            _transform.position = destination;
            _transform.rotation = rotation;
            _currentBodyRotation = _transform.localEulerAngles;
            _currentCameraRotation = Vector3.zero;
            CameraTransform.localEulerAngles = _currentCameraRotation;
            characterController.enabled = true;
        }

        public void Teleport(Transform destination)
        {
            Teleport(destination.position, destination.rotation);
        }

        public void SwitchPlayerController()
        {
            if(Current == this || Switching) return;

            _switchDelay = Time.time + SWITCH_DELAY;
            
            if (Current != null)
            {
                Current.ExitPlayer();
            }

            Current = this;
        
            Current.EnterPlayer();
        }

        private void EnterPlayer()
        {
            characterController.enabled = true;
            for (int i = 0; i < onPlayerEnterEvents.Length; i++)
            {
                if(Mathf.Approximately(onPlayerEnterEvents[i].delay, 0))
                {
                    onPlayerEnterEvents[i].events.Invoke();
                }
                else
                {
                    StartCoroutine(onPlayerEnterEvents[i].Invoke());
                }
            }
            virtualCamera.Priority = 11;
            if(_isNormalMode) SetModeNormal();
            else SetModeUI();
        }

        private void ExitPlayer()
        {
            characterController.enabled = false;
            for (int i = 0; i < onPlayerExitEvents.Length; i++)
            {
                if(Mathf.Approximately(onPlayerExitEvents[i].delay, 0))
                {
                    onPlayerExitEvents[i].events.Invoke();
                }
                else
                {
                    StartCoroutine(onPlayerExitEvents[i].Invoke());
                }
            }
            virtualCamera.Priority = 9;
        }

        public void ResetPosition()
        {
            _transform.position = _defaultPosition;
        }

        public void ResetRotation()
        {
            _currentBodyRotation.y = _defaultRotation.y;
            _currentCameraRotation.x = _defaultRotation.x;
        }

        public void SetModeNormal()
        {
            CursorManager.Instance.SetLockState(normalMode.lockedCursor);
            LockedMovement = normalMode.lockedMovement;
            LockedRotation = normalMode.lockedRotation;
            Locked3DInteractions = normalMode.locked3DInteraction;
            LockedUIInteractions = normalMode.lockedUIInteraction;
            _isNormalMode = true;
        }

        public void SetModeUI()
        {
            CursorManager.Instance.SetLockState(uiMode.lockedCursor);
            LockedMovement = uiMode.lockedMovement;
            LockedRotation = uiMode.lockedRotation;
            Locked3DInteractions = uiMode.locked3DInteraction;
            LockedUIInteractions = uiMode.lockedUIInteraction;
            _isNormalMode = false;
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

        public Vector3 gravity = new Vector3(0, -9.81f, 0);

        public bool allowThirdAxisMovement;
        [ShowIf("allowThirdAxisMovement")] public float thirdAxisSpeedMultiplier = 1;

        public bool limitMovementX;
        [ShowIf("limitMovementX")] public Vector2 xAxisBounds;

        public bool limitMovementY;
        [ShowIf("limitMovementY")] public Vector2 yAxisBounds;

        public bool limitMovementZ;
        [ShowIf("limitMovementZ")] public Vector2 zAxisBounds;

        public float movementSpeedMultiplier = 1;
        public float sprintMultiplier = 1.5f;
        public float rotationSpeedMultiplier = 1;
        public float raycastLength = 2;
    }

    [Serializable]
    public class PlayerControllerMode
    {
        public bool lockedCursor;
        public bool lockedMovement;
        public bool lockedRotation;
        public bool locked3DInteraction;
        public bool lockedUIInteraction;
    }

    public class Bounds
    {
        public Vector2 X;
        public Vector2 Y;
        public Vector2 Z;
    }
}