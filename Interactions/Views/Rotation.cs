using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPackages.Silicom.Player.Interactions.Views
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private TransformRotation transformRotation;    
    
        [SerializeField] private Vector3[] rotations;
    
#if UNITY_EDITOR
        private int Length => Mathf.Max(rotations.Length - 1, 0);
        [HideIf("Length", 0), PropertyRange(0, "Length")]
#endif
        [SerializeField] private int defaultRotation;

        [SerializeField] private bool applyDefaultRotationOnAwake;

        public int CurrentIndex { get; private set; }

        private void Awake()
        {
            CurrentIndex = defaultRotation;
            if(applyDefaultRotationOnAwake) transformRotation.Rotate(rotations[CurrentIndex], true);
        }

        public void AddIndex(int value)
        {
            CurrentIndex = (CurrentIndex + value) % rotations.Length;
            Rotate();
        }

        public void SetIndex(int value)
        {
            if (value < 0 || value >= rotations.Length)
            {
                Debug.LogError($"Requested index ({value}) is out of bound !");
                return;
            }

            CurrentIndex = value;
            Rotate();
        }
        
        private void Rotate()
        {
            transformRotation.Rotate(rotations[CurrentIndex]);
        }
    }
}