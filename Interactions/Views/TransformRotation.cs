using System.Collections;
using CustomPackages.Silicom.Core.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomPackages.Silicom.Player.Interactions.Views
{
    public class TransformRotation : MonoBehaviour
    {

        [SerializeField] private Transform toRotate;

        [SerializeField] private bool smoothRotation = true;
        [FormerlySerializedAs("rotationSpeed")] [SerializeField, ShowIf("smoothRotation")] private float rotationTime = 1;
    
        private Quaternion _startRotation;
        private Quaternion _endRotation;
        private float _rotationTime;

        private bool _isRotating;
        private Coroutine _coroutine;

        private void Awake()
        {
            _rotationTime = rotationTime;
        }

        public void SetRotationTime(float time)
        {
            _rotationTime = time;
        }

        public void Rotate(Vector3 eulers, bool forceNoSmooth = false)
        {
            _endRotation = Quaternion.Euler(eulers);

            if (forceNoSmooth || !smoothRotation)
            {
                Rotate();
            }
            else
            {
                RotateSmooth();
            }
        }

        private void Rotate()
        {
            toRotate.localRotation = _endRotation;
        }

        private void RotateSmooth()
        {
            if (_isRotating) StopCoroutine(_coroutine);
            _isRotating = true;
            _coroutine = StartCoroutine(RotateSmoothCo());
        }

        private IEnumerator RotateSmoothCo()
        {
            _startRotation = toRotate.localRotation;
            float delta = 0;

            while (delta < 1)
            {
                delta += Time.deltaTime / _rotationTime;
                toRotate.localRotation = Quaternion.Slerp(_startRotation, _endRotation, delta);
                yield return Yielders.EndOfFrame;
            }

            _isRotating = false;
        }

    }
}