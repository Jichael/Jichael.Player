using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomPackages.Silicom.Player.Players
{
    public abstract class InputsHandler : MonoBehaviour
    {
        public static InputsHandler Instance { get; protected set; }
        
        public abstract InputActionReference UseAction { get; }

        public abstract void StartInputProcessing();
        public abstract void StopInputProcessing();
    }
}