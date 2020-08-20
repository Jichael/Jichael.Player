using CustomPackages.SilicomPlayer.Players.MouseKeyboard;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Interactions.Controllers.MouseKeyboard
{
    public class RotationMouse : MonoBehaviour, IMouseInteract
    {
        [SerializeField] private Views.TransformRotation transformRotation;    
    
        [SerializeField] private Vector3[] rotations;
    
#if UNITY_EDITOR
        private int Length => Mathf.Max(rotations.Length - 1, 0);
        [HideIf("Length", 0), PropertyRange(0, "Length")]
#endif
        [SerializeField] private int defaultRotation;

        [SerializeField] private bool applyDefaultRotationOnAwake;

        private int _currentIndex;

        private void Awake()
        {
            _currentIndex = defaultRotation;
            if(applyDefaultRotationOnAwake) transformRotation.Rotate(rotations[_currentIndex], true);
        }

        public void AddIndex(int value)
        {
            _currentIndex = (_currentIndex + value) % rotations.Length;
        }

        public void SetIndex(int value)
        {
            if (value < 0 || value >= rotations.Length)
            {
                Debug.LogError($"Requested index ({value}) is out of bound !");
                return;
            }

            _currentIndex = value;
        }
        
        public void Rotate()
        {
            transformRotation.Rotate(rotations[_currentIndex]);
        }

        [SerializeField] private bool disableInteraction;

        public bool DisableInteraction
        {
            get => disableInteraction;
            set => disableInteraction = value;
        }

        public void LeftClick(MouseController mouseController)
        {
            AddIndex(1);
            transformRotation.Rotate(rotations[_currentIndex]);
        }

        public void RightClick(MouseController mouseController) { }

        public void HoverEnter(MouseController mouseController) { }

        public void HoverStay(MouseController mouseController) { }

        public void HoverExit(MouseController mouseController) { }
    }
}