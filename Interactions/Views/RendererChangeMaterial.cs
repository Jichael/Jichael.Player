using UnityEngine;

namespace CustomPackages.Silicom.Player.Interactions.Views
{
    public class RendererChangeMaterial : MonoBehaviour
    {

        [SerializeField] private new Renderer renderer;

        public void ChangeMaterial(Material material)
        {
            renderer.material = material;
        }
        
        private void OnValidate()
        {
            renderer = GetComponentInChildren<Renderer>();
        }
    }
}