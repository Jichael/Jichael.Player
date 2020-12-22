using UnityEngine;

namespace CustomPackages.Silicom.Player.Players
{
    public class InputsHandler : MonoBehaviour
    {
        public static InputsHandler Instance { get; protected set; }

        public virtual void StartInputProcessing() {}
        public virtual void StopInputProcessing() {}
    }
}